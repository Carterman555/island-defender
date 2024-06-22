using UnityEngine.UI;
using UnityEngine;

namespace IslandDefender {
	public class BuildingProgressBar : StaticInstance<BuildingProgressBar> {

		[SerializeField] private Image backgroundImage;
		[SerializeField] private Image fillImage;

        private void Start() {
            Hide();
        }

        public void Show() {
			backgroundImage.enabled = true;
			fillImage.enabled = true;
        }

		public void Hide() {
            backgroundImage.enabled = false;
            fillImage.enabled = false;
        }

        public void UpdateBar(float amount) {
			fillImage.fillAmount = amount;
		}

        private void Update() {
            // to face correct direction when unit changes facing
            bool facingRight = PlayerBuild.Instance.transform.eulerAngles.y == 0;
            float direction = facingRight ? 1 : -1;
            transform.localScale = new Vector3(direction, 1);
        }
    }
}