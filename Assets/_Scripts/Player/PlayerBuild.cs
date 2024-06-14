using IslandDefender.Environment;
using IslandDefender.Management;
using TarodevController;
using UnityEngine;

namespace IslandDefender {
	public class PlayerBuild : StaticInstance<PlayerBuild> {

        private ScriptableBuilding buildingPlacing;
        private GameObject activePlaceVisual;

        private PlayerController playerController;

        public bool IsPlacingBuilding => activePlaceVisual != null;

        protected override void Awake() {
            base.Awake();
            playerController = GetComponent<PlayerController>();
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Q)) {
                if (IsPlacingBuilding) {
                    HidePlaceVisual();
                }
            }

            if (IsPlacingBuilding && CanAffordBuilding(buildingPlacing.BuildingType)) {

                UpdateVisualPosition();

                if (Input.GetMouseButtonDown(1)) {
                    CreateBuilding();
                }
            }
        }

        public void ShowPlaceVisual(BuildingType buildingType) {

            // hide previous visual if placing
            if (IsPlacingBuilding) {
                HidePlaceVisual();
            }

            buildingPlacing = ResourceSystem.Instance.GetBuilding(buildingType);

            activePlaceVisual = ObjectPoolManager.SpawnObject(buildingPlacing.PlaceVisualPrefab,
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
            Vector2 directionalOffset = new Vector2(buildingPlacing.BuildOffset.x * direction, buildingPlacing.BuildOffset.y);

            activePlaceVisual.transform.position = transform.position + (Vector3)directionalOffset;
        }

        private void CreateBuilding() {
            ObjectPoolManager.SpawnObject(buildingPlacing.Prefab,
                activePlaceVisual.transform.position,
                Quaternion.identity,
                Containers.Instance.Buildings);

            HidePlaceVisual();

            CostPlayer(buildingPlacing);
        }

        public bool CanAffordBuilding(BuildingType buildingType) {

            ScriptableBuilding scriptableBuilding = ResourceSystem.Instance.GetBuilding(buildingType);

            bool canAffordWood = PlayerResources.Instance.GetResourceAmount(ResourceType.Wood) >= scriptableBuilding.WoodCost;
            bool canAffordFiber = PlayerResources.Instance.GetResourceAmount(ResourceType.Fiber) >= scriptableBuilding.FiberCost;
            bool canAffordStone = PlayerResources.Instance.GetResourceAmount(ResourceType.Stone) >= scriptableBuilding.StoneCost;

            return canAffordWood && canAffordFiber && canAffordStone;
        }

        private void CostPlayer(ScriptableBuilding scriptableBuilding) {
            PlayerResources.Instance.RemoveResource(ResourceType.Wood, scriptableBuilding.WoodCost);
            PlayerResources.Instance.RemoveResource(ResourceType.Fiber, scriptableBuilding.StoneCost);
            PlayerResources.Instance.RemoveResource(ResourceType.Stone, scriptableBuilding.StoneCost);
        }
    }
}