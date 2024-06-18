using IslandDefender.Environment;
using IslandDefender.Management;
using System.Collections.Generic;
using TarodevController;
using UnityEngine;

namespace IslandDefender {
	public class PlayerBuild : StaticInstance<PlayerBuild> {

        private ScriptableBuilding buildingPlacing;
        private SpriteRenderer activePlaceVisual;

        private PlayerController playerController;

        public bool IsPlacingBuilding => activePlaceVisual != null;

        private List<int> takenXPositions = new List<int>();

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
                Containers.Instance.Buildings).GetComponent<SpriteRenderer>();
        }

        private void HidePlaceVisual() {
            ObjectPoolManager.ReturnObjectToPool(activePlaceVisual.gameObject);
            activePlaceVisual = null;
        }

        private void UpdateVisualPosition() {
            int direction = playerController.IsFacingRight ? 1 : -1;


            float directionalXOffset = buildingPlacing.BuildOffset.x * direction;
            float xPos = directionalXOffset + transform.position.x;

            if (IsAvailableGridPos(xPos, out int gridXPos)) {
                activePlaceVisual.color = Color.white;
            }
            else {
                activePlaceVisual.color = Color.red;
            }

            activePlaceVisual.transform.position = new Vector3(gridXPos, transform.position.y + buildingPlacing.BuildOffset.y);
        }


        private bool IsAvailableGridPos(float xPos, out int gridXPos) {

            print("in: " + xPos);

            int spacing = 3;
            gridXPos = Mathf.RoundToInt(xPos / spacing) * spacing;

            print("out: " + gridXPos);

            return !takenXPositions.Contains(gridXPos);
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
            bool canAffordGold = PlayerResources.Instance.GetResourceAmount(ResourceType.Gold) >= scriptableBuilding.GoldCost;

            return canAffordWood && canAffordFiber && canAffordStone && canAffordGold;
        }

        private void CostPlayer(ScriptableBuilding scriptableBuilding) {
            PlayerResources.Instance.RemoveResource(ResourceType.Wood, scriptableBuilding.WoodCost);
            PlayerResources.Instance.RemoveResource(ResourceType.Fiber, scriptableBuilding.FiberCost);
            PlayerResources.Instance.RemoveResource(ResourceType.Stone, scriptableBuilding.StoneCost);
            PlayerResources.Instance.RemoveResource(ResourceType.Gold, scriptableBuilding.StoneCost);
        }
    }
}