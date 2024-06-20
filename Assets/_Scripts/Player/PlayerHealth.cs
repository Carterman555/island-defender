using IslandDefender.Management;
using UnityEngine;

namespace IslandDefender.Units.Player {
	public class PlayerHealth : Health {

        protected override void Awake() {
            base.Awake();
            SetMaxHealth(GetComponent<UnitBase>().Stats.Health);
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
            health = GetMaxHealth();
        }

        public override void Die() {
            GameManager.Instance.GameOver();
        }
    }
}