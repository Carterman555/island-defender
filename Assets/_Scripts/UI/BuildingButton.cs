using UnityEngine;

namespace IslandDefender.UI {
	public class BuildingButton : GameButton {

        [SerializeField] private BuildingType buildingType;

        protected override void OnClicked() {
            base.OnClicked();

            if (PlayerBuild.Instance.CanAffordBuilding(buildingType)) {
                PlayerBuild.Instance.ShowPlaceVisual(buildingType);
            }
        }

    }
}