using IslandDefender.Environment;
using IslandDefender.Management;
using System;
using System.Collections.Generic;
using TarodevController;
using UnityEngine;

namespace IslandDefender {
	public class PlayerBuild : StaticInstance<PlayerBuild> {

        public static event Action<BuildingType> OnBuild;

        private ScriptableBuilding buildingPlacing;
        private SpriteRenderer activePlaceVisual;
        private Color originalPlaceVisualColor;

        private PlayerController playerController;

        public bool BuildingVisualActive => activePlaceVisual != null;

        private List<int> takenXPositions = new List<int>();

        [SerializeField] private float buildDuration;
        private float buildTimer;

        private bool building;

        protected override void Awake() {
            base.Awake();
            playerController = GetComponent<PlayerController>();
        }

        private void OnEnable() {
            IDamagable.OnAnyDespawn += TryUpdateTakenList;
        }
        private void OnDisable() {
            IDamagable.OnAnyDespawn -= TryUpdateTakenList;
        }

        private void TryUpdateTakenList(GameObject objectDestroyed) {
            if (objectDestroyed.layer == GameLayers.BuildingLayer) {

                int buildingXPos = Mathf.RoundToInt(objectDestroyed.transform.position.x);
                if (takenXPositions.Contains(buildingXPos)) {
                    takenXPositions.Remove(buildingXPos);
                }
                else {
                    Debug.LogWarning("Building X Pos Not Found in List!");
                }
            }
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Q)) {
                if (BuildingVisualActive) {
                    HidePlaceVisual();
                }
            }

            if (BuildingVisualActive && CanAffordBuilding(buildingPlacing.BuildingType)) {

                UpdateVisualPosition();

                if (Input.GetMouseButtonDown(1)) {
                    StartBuilding();
                }
            }

            if (building) {
                buildTimer += Time.deltaTime;
                if (buildTimer > buildDuration) {
                    buildTimer = 0;
                    building = false;

                    CreateBuilding();

                    playerController.EnableMovement();
                }
            }
            else {
                buildTimer = 0;
            }
        }

        public void ShowPlaceVisual(BuildingType buildingType) {

            // hide previous visual if placing
            if (BuildingVisualActive) {
                HidePlaceVisual();
            }

            buildingPlacing = ResourceSystem.Instance.GetBuilding(buildingType);

            activePlaceVisual = ObjectPoolManager.SpawnObject(buildingPlacing.PlaceVisualPrefab,
                Vector3.zero, // position will be updated
                Quaternion.identity,
                Containers.Instance.Buildings).GetComponent<SpriteRenderer>();
            originalPlaceVisualColor = activePlaceVisual.color;
        }

        private void HidePlaceVisual() {
            ObjectPoolManager.ReturnObjectToPool(activePlaceVisual.gameObject);
            activePlaceVisual = null;
        }

        private void UpdateVisualPosition() {

            float buildingXOffset = 4f;
            float directionalXOffset = playerController.IsFacingRight ? buildingXOffset : -buildingXOffset;
            float xPos = directionalXOffset + transform.position.x;

            activePlaceVisual.color = originalPlaceVisualColor;

            if (!IsAvailableGridPos(xPos, out int gridXPos)) {
                float hueShiftIntensity = 0.5f;
                activePlaceVisual.ChangeHue(Color.red, hueShiftIntensity);
            }

            float buildingYPos = 0.75f;
            activePlaceVisual.transform.position = new Vector3(gridXPos, buildingYPos);
        }

        private bool IsAvailableGridPos(float xPos, out int gridXPos) {
            int spacing = 4;
            gridXPos = Mathf.RoundToInt(xPos / spacing) * spacing;

            return !takenXPositions.Contains(gridXPos);
        }

        private void StartBuilding() {
            building = true;
            playerController.DisableMovement();
        }

        private void CreateBuilding() {

            if (!IsAvailableGridPos(activePlaceVisual.transform.position.x, out int gridXPos)) {
                return;
            }

            ObjectPoolManager.SpawnObject(buildingPlacing.Prefab,
                activePlaceVisual.transform.position,
                Quaternion.identity,
                Containers.Instance.Buildings);

            int buildingXPos = Mathf.RoundToInt(activePlaceVisual.transform.position.x);
            takenXPositions.Add(buildingXPos);

            HidePlaceVisual();

            CostPlayer(buildingPlacing);

            OnBuild?.Invoke(buildingPlacing.BuildingType);
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