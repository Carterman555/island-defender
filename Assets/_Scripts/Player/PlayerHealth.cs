using IslandDefender.Management;
using UnityEngine;

namespace IslandDefender.Units.Player {
	public class PlayerHealth : Health {

        private float maxHealth;

        protected override void Awake() {
            base.Awake();
            maxHealth = GetComponent<UnitBase>().Stats.Health;
        }

        protected override void OnEnable() {
            base.OnEnable();
            EnemyWaveManager.OnNextWave += Heal;
        }
        protected override void OnDisable() {
            base.OnDisable();
            EnemyWaveManager.OnNextWave -= Heal;
        }

        private void Heal(int n) {
            health = maxHealth;
        }

        protected override void ResetValues() {
            base.ResetValues();
            health = maxHealth;
        }

        protected override void Die() {
            GameManager.Instance.GameOver();
        }
    }
}