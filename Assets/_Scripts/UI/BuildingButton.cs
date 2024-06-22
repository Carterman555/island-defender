using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace IslandDefender.UI {
    public class BuildingButton : GameButton, IPointerEnterHandler, IPointerExitHandler {

        [SerializeField] private BuildingType buildingType;
        private ScriptableBuilding scriptableBuilding;

        [SerializeField] private Image image;

        public int GetLevelToUnlock() {
            return ResourceSystem.Instance.GetBuilding(buildingType).KeepLevelToUnlock;
        }

        private void Start() {
            scriptableBuilding = ResourceSystem.Instance.GetBuilding(buildingType);
        }

        public void OnPointerEnter(PointerEventData eventData) {
            RectTransform rectTransform = GetComponent<RectTransform>();
            Vector2 offset = new Vector2(0, 2.5f);

            CostText.Instance.Show((Vector2)rectTransform.position + offset,
                scriptableBuilding.WoodCost,
                scriptableBuilding.FiberCost,
                scriptableBuilding.StoneCost);
        }

        public void OnPointerExit(PointerEventData eventData) {
            CostText.Instance.Hide();
        }

        protected override void OnClicked() {
            base.OnClicked();

            if (PlayerBuild.Instance.CanAffordBuilding(buildingType)) {
                PlayerBuild.Instance.ShowPlaceVisual(buildingType);
            }
        }

        private void Update() {
            if (PlayerBuild.Instance.CanAffordBuilding(buildingType)) {
                image.color = Color.white;
            }
            else {
                image.color = Color.gray;
            }
        }
    }
}