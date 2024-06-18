using IslandDefender.Environment;
using IslandDefender.Management;
using IslandDefender.Utilities;
using UnityEngine;

namespace IslandDefender {
	public class FlyTrapSapling : Health {

        [SerializeField] private float maxHealth;

		[SerializeField] private RandomFloat growTime;
		private float growTimer;

        [SerializeField] private float ySpawnOffset;

        protected override void ResetValues() {
            base.ResetValues();
            health = maxHealth;

            growTime.Randomize();
            growTimer = 0;
        }

        private void Update() {
            growTimer += Time.deltaTime;
            if (growTimer > growTime.Value) {
                growTimer = 0;

                GameObject flyTrapPrefab = ResourceSystem.Instance.GetEnemy(EnemyType.FlyTrap).Prefab;
                Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y + ySpawnOffset);
                ObjectPoolManager.SpawnObject(flyTrapPrefab, spawnPos, Quaternion.identity, Containers.Instance.Enemies);

                Die();
            }
        }
    }
}