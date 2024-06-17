using UnityEngine;

namespace IslandDefender {
	public class Garden : Health {

		[SerializeField] private float maxHealth;

        private int wavesAlive;

        protected override void OnEnable() {
            base.OnEnable();
            ResourceSpawnManager.Instance.AddGarden();

            EnemyWaveManager.OnNextWave += IncreaseWaves;
        }

        protected override void OnDisable() {
            base.OnDisable();
            EnemyWaveManager.OnNextWave -= IncreaseWaves;
        }

        protected override void Die() {
            base.Die();
            ResourceSpawnManager.Instance.RemoveGarden();
        }

        protected override void ResetValues() {
            base.ResetValues();
            health = maxHealth;
            wavesAlive = 0;
        }

        private void IncreaseWaves(int n) {
            if (!enabled) {
                return;
            }

            wavesAlive++;

            // can only last 3 waves
            if (wavesAlive >= 3) {
                Die();
            }
        }
    }
}