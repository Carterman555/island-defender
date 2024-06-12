using IslandDefender.Environment;
using IslandDefender.Management;
using TarodevController;
using UnityEngine;

namespace IslandDefender {
	public class PlayerBuild : MonoBehaviour {

        [SerializeField] private Vector2 buildOffset;

        [Header("Buildings")]
        [SerializeField] private ScriptableBuilding scriptableWall;

        private GameObject activePlaceVisual;

        private PlayerController playerController;

        private bool PlacingBuilding => activePlaceVisual != null;

        private void Awake() {
            playerController = GetComponent<PlayerController>();
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Q)) {
                if (!PlacingBuilding && CanAffordBuilding(scriptableWall)) {
                    ShowPlaceVisual();
                }
                else if (PlacingBuilding) {
                    HidePlaceVisual();
                }
            }


            if (PlacingBuilding && CanAffordBuilding(scriptableWall)) {

                UpdateVisualPosition();

                if (Input.GetMouseButtonDown(1)) {
                    CreateBuilding();
                }
            }
        }

        private void ShowPlaceVisual() {
            activePlaceVisual = ObjectPoolManager.SpawnObject(scriptableWall.PlaceVisualPrefab,
                Vector3.zero, // position will be updated
                Quaternion.identity,
                Containers.Instance.Buildings);
        }

        private void HidePlaceVisual() {
            ObjectPoolManager.ReturnObjectToPool(activePlaceVisual);
            activePlaceVisual = null;
        }

        private void UpdateVisualPosition() {
            int direction = playerController.IsFacingRight ? 1 : -1;
            Vector2 directionalOffset = new Vector2(buildOffset.x * direction, buildOffset.y);

            activePlaceVisual.transform.position = transform.position + (Vector3)directionalOffset;
        }

        private void CreateBuilding() {
            ObjectPoolManager.SpawnObject(scriptableWall.Prefab,
                activePlaceVisual.transform.position,
                Quaternion.identity,
                Containers.Instance.Buildings);

            HidePlaceVisual();

            CostPlayer(scriptableWall);
        }

        private bool CanAffordBuilding(ScriptableBuilding scriptableBuilding) {

            bool canAffordWood = PlayerResources.Instance.GetResourceAmount(ResourceType.Wood) >= scriptableBuilding.WoodCost;
            bool canAffordStone = PlayerResources.Instance.GetResourceAmount(ResourceType.Stone) >= scriptableBuilding.StoneCost;

            return canAffordWood && canAffordStone;
        }

        private void CostPlayer(ScriptableBuilding scriptableBuilding) {
            PlayerResources.Instance.RemoveResource(ResourceType.Wood, scriptableBuilding.WoodCost);
            PlayerResources.Instance.RemoveResource(ResourceType.Stone, scriptableBuilding.StoneCost);
        }
    }
}