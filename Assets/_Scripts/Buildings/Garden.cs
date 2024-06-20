using UnityEngine;

namespace IslandDefender {
	public class Garden : Health {

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

        public override void Die() {
            base.Die();
            ResourceSpawnManager.Instance.RemoveGarden();
        }

        protected override void ResetValues() {
            base.ResetValues();
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