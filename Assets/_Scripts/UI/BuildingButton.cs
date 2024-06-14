using UnityEngine;
using UnityEngine.EventSystems;

namespace IslandDefender.UI {
    public class BuildingButton : GameButton, IPointerEnterHandler, IPointerExitHandler {

        [SerializeField] private BuildingType buildingType;

        public void OnPointerEnter(PointerEventData eventData) {
            RectTransform rectTransform = GetComponent<RectTransform>();
            Vector2 offset = new Vector2(0, 130f);

            ScriptableBuilding scriptableBuilding = ResourceSystem.Instance.GetBuilding(buildingType);

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
    }
}