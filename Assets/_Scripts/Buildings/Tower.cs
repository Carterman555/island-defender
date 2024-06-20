using IslandDefender.Environment;
using IslandDefender.Management;
using System.Linq;
using UnityEngine;

namespace IslandDefender.Buildings {
	public class Tower : Health {

		[SerializeField] private TowerProjectile projectilePrefab;
		[SerializeField] private Transform projectileSpawnPoint;
        [SerializeField] private TriggerContactTracker enemiesInRangeTracker;

        [Header("Stats")]
		[SerializeField] private float maxHealth;
		[SerializeField] private float damage;
		[SerializeField] private float shootCooldown;
		private float shootTimer;

        protected override void ResetValues() {
            base.ResetValues();
            health = maxHealth;
        }

        private void Update() {
			shootTimer += Time.deltaTime;
			if (shootTimer > shootCooldown && EnemyInRange(out Vector3 closestEnemyPos)) {
				shootTimer = 0;
                Shoot(closestEnemyPos);
            }
        }

		private void Shoot(Vector3 closestEnemyPos) {
            TowerProjectile projectile = ObjectPoolManager.SpawnObject(projectilePrefab,
				projectileSpawnPoint.position,
				Quaternion.identity,
				Containers.Instance.Projectiles);

            projectile.Shoot(closestEnemyPos, damage);
		}

		private bool EnemyInRange(out Vector3 position) {

            GameObject closestEnemy = enemiesInRangeTracker.GetContacts()
                .OrderBy(go => Vector2.Distance(go.transform.position, transform.position))
				.FirstOrDefault();

			if (closestEnemy == null) {
				position = Vector3.zero;
				return false;
			}
			else {
				position = closestEnemy.transform.position;
				return true;
			}
        }
    }
}