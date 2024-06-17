using IslandDefender.Management;
using UnityEngine;

namespace IslandDefender {

    [RequireComponent(typeof(Rigidbody2D))]
	public class Arrow : MonoBehaviour, IProjectile {


		private float damage;

        [SerializeField] private Vector2 shootForce;
        private Rigidbody2D rb;

        private void Awake() {
            rb = GetComponent<Rigidbody2D>();
        }

        public void Shoot(int direction, float damage) {
            this.damage = damage;

            rb.AddForce(new Vector2(shootForce.x * direction, shootForce.y));
        }

        private void OnTriggerEnter2D(Collider2D collision) {
            if (collision.gameObject.layer == GameLayers.EnemyLayer) {
                if (collision.TryGetComponent(out ProjectileImmunity projectileImmunity)) {
                    return;
                }

                collision.GetComponent<IDamagable>().KnockbackDamage(damage, transform.position);
                Die();
            }
        }

        private void Die() {
            ObjectPoolManager.ReturnObjectToPool(gameObject);
        }
    }
}