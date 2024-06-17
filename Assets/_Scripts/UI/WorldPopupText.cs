using DG.Tweening;
using TMPro;
using UnityEngine;

namespace IslandDefender {
    public class WorldPopupText : StaticInstance<WorldPopupText> {

        private TextMeshProUGUI text;

        protected override void Awake() {
            base.Awake();
            text = GetComponent<TextMeshProUGUI>();
        }

        public void ShowText(Vector2 pos, string popupText) {

            transform.localScale = Vector2.zero;
            float duration = 0.3f;
            transform.DOScale(1, duration);

            transform.position = pos;
            text.text = popupText;
        }

        public void HideText() {
            text.text = "";
        }
    }
}
