using IslandDefender.Management;
using UnityEngine;

namespace IslandDefender {

    [RequireComponent(typeof(Rigidbody2D))]
	public class Arrow : MonoBehaviour, IDirectionProjectile {


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
            if (collision.gameObject.layer == GameLayers.GroundLayer) {
                Die();
            }
        }

        Vector3 previousPos;

        private void Update() {

            Vector3 delta = transform.position - previousPos;

            float rotateSpeed = 5f;
            transform.right = Vector3.MoveTowards(transform.right, delta, rotateSpeed * Time.deltaTime);


            previousPos = transform.position;
        }

        private void Die() {
            ObjectPoolManager.ReturnObjectToPool(gameObject);
        }
    }
}