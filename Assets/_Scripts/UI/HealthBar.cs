using System;
using UnityEngine;
using UnityEngine.UI;

namespace IslandDefender {
	public class HealthBar : MonoBehaviour {

        [SerializeField] private Image backgroundImage;
        [SerializeField] private Image fillImage;

		private Health healthComponent;

        private bool FacingRight => healthComponent.transform.eulerAngles.y == 0;

        private void Awake() {
            healthComponent = GetComponentInParent<Health>();

            backgroundImage.enabled = false;
            fillImage.enabled = false;
        }

        private void OnEnable() {
            healthComponent.OnDamaged += UpdateHealth;
        }
        private void OnDisable() {
            healthComponent.OnDamaged -= UpdateHealth;
        }

        private void UpdateHealth(float health) {
            backgroundImage.enabled = true;
            fillImage.enabled = true;

            fillImage.fillAmount = health / healthComponent.GetMaxHealth();
        }

        private void Update() {

            // to face correct direction when enemy changes facd
            float direction = FacingRight ? 1 : -1;
            transform.localScale = new Vector3(direction, 1);
        }
    }
}