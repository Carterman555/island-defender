using IslandDefender.Units.Player;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace IslandDefender {
	public class HealthBar : MonoBehaviour {

        [SerializeField] private Image backgroundImage;
        [SerializeField] private Image fillImage;

		private Health healthComponent;

        private void Awake() {
            healthComponent = GetComponentInParent<Health>();

            Hide();
        }

        private void OnEnable() {
            healthComponent.OnDamaged += UpdateHealth;
            healthComponent.OnDeath += Hide;

            if (healthComponent is PlayerHealth playerHealth) {
                playerHealth.OnHeal += UpdateHealth;
            }
        }
        private void OnDisable() {
            healthComponent.OnDamaged -= UpdateHealth;
            healthComponent.OnDeath -= Hide;

            if (healthComponent is PlayerHealth playerHealth) {
                playerHealth.OnHeal -= UpdateHealth;
            }
        }

        private void Hide() {
            backgroundImage.enabled = false;
            fillImage.enabled = false;
        }

        private void UpdateHealth(float health) {
            backgroundImage.enabled = true;
            fillImage.enabled = true;

            fillImage.fillAmount = health / healthComponent.GetMaxHealth();
        }

        private void Update() {

            // to face correct direction when unit changes facing
            bool facingRight = healthComponent.transform.eulerAngles.y == 0;
            float direction = facingRight ? 1 : -1;
            transform.localScale = new Vector3(direction, 1);
        }
    }
}