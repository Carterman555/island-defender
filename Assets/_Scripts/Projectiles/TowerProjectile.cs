using IslandDefender.Management;
using UnityEngine;

namespace IslandDefender {
    public class TowerProjectile : MonoBehaviour, IPositionProjectile {

        [SerializeField] private float speed;

        private float damage;
        private Vector2 directionToEnemy;

        public void Shoot(Vector2 targetPos, float damage) {
            this.damage = damage;
            directionToEnemy = (targetPos - (Vector2)transform.position).normalized;
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

                if (collision.TryGetComponent(out ProjectileImmunity projectileImmunity)) {
                    return;
                }

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
