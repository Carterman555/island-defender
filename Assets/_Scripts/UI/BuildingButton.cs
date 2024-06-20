using IslandDefender.Environment.Building;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace IslandDefender.UI {
    public class BuildingButton : GameButton, IPointerEnterHandler, IPointerExitHandler {

        [SerializeField] private BuildingType buildingType;
        private ScriptableBuilding scriptableBuilding;

        private bool unlocked;

        private void Start() {
            scriptableBuilding = ResourceSystem.Instance.GetBuilding(buildingType);

            if (scriptableBuilding.KeepLevelToUnlock == 1) {
                Unlock();
            }
            else {
                Lock();
            }
        }

        protected override void OnEnable() {
            base.OnEnable();
            Keep.OnUpgrade += TryUnlock;
        }
        protected override void OnDisable() {
            base.OnDisable();
            Keep.OnUpgrade -= TryUnlock;
        }

        private void TryUnlock(int keepLevel) {
            if (!unlocked && keepLevel >= scriptableBuilding.KeepLevelToUnlock) {
                Unlock();
            }
        }

        private void Lock() {
            unlocked = false;
        }

        private void Unlock() {
            unlocked = true;
        }

        public void OnPointerEnter(PointerEventData eventData) {
            RectTransform rectTransform = GetComponent<RectTransform>();
            Vector2 offset = new Vector2(0, 70f);

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