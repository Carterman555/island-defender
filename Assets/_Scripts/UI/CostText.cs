using TMPro;
using UnityEngine;

namespace IslandDefender {
	public class CostText : StaticInstance<CostText> {

		private TextMeshProUGUI text;

        protected override void Awake() {
            base.Awake();
            text = GetComponent<TextMeshProUGUI>();

            Hide();
        }

        public void Show(Vector2 pos, int woodCost, int fiberCost, int stoneCost) {
            RectTransform rectTransform = GetComponent<RectTransform>();
            rectTransform.position = pos;

            text.text = "Wood: " + woodCost + "\n" +
                "Fiber: " + fiberCost + "\n" +
                "Stone: " + stoneCost;
        }

        public void Hide() {
            text.text = "";
        }
    }
}