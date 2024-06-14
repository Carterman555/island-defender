using IslandDefender.Management;
using UnityEngine;

namespace IslandDefender {
    public class TowerProjectile : MonoBehaviour, IProjectile {

        [SerializeField] private float speed;

        private float damage;
        private Vector2 directionToEnemy;

        public void Shoot(int direction, float damage) {
            this.damage = damage;
        }

        public void Shoot(Vector3 targetPos, float damage) {

            int xDirection = transform.position.x > targetPos.x ? -1 : 1;
            Shoot(xDirection, damage);

            directionToEnemy = (targetPos - transform.position).normalized;
            transform.right = directionToEnemy;
        }

        private void Update() {
            transform.position += (Vector3)directionToEnemy * speed * Time.deltaTime;
        }

        // destroy when hit ground, damage and destroy when hit enemy
        private void OnTriggerEnter2D(Collider2D collision) {
            if (collision.gameObject.layer == GameLayers.GroundLayer) {
                ObjectPoolManager.ReturnObjectToPool(gameObject);
            }
            else if (collision.gameObject.layer == GameLayers.EnemyLayer) {

                if (collision.TryGetComponent(out IDamagable damagable)) {
                    damagable.KnockbackDamage(damage, transform.position);
                }
                else {
                    Debug.LogWarning("Enemy Layer Does Not Have IDamagable");
                }

                ObjectPoolManager.ReturnObjectToPool(gameObject);
            }
        }
    }
}
