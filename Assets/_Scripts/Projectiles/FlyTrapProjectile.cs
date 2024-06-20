using IslandDefender.Management;
using UnityEngine;

namespace IslandDefender {
    public class FlyTrapProjectile : MonoBehaviour, IPositionProjectile {

        [SerializeField] private float gravity = 9.8f;

        private float damage;

        private Rigidbody2D rb;

        private void Awake() {
            rb = GetComponent<Rigidbody2D>();
        }

        public void Shoot(Vector2 targetPos, float damage) {
            this.damage = damage;

            float targetDistance = Mathf.Abs(transform.position.x - targetPos.x);

            // Calculate the required initial velocity
            float initialVelocity = Mathf.Sqrt(targetDistance * gravity);

            // Calculate the velocity components
            float velocityX = initialVelocity * Mathf.Cos(45 * Mathf.Deg2Rad);
            float velocityY = initialVelocity * Mathf.Sin(45 * Mathf.Deg2Rad);

            // Apply the force to the projectile
            Vector2 force = new Vector2(velocityX, velocityY) * rb.mass;
            rb.AddForce(force, ForceMode2D.Impulse);
        }

        private void OnTriggerEnter2D(Collider2D collision) {
            if (collision.gameObject.layer == GameLayers.PlayerLayer ||
                collision.gameObject.layer == GameLayers.AllyLayer ||
                collision.gameObject.layer == GameLayers.BuildingLayer) {

                if (collision.TryGetComponent(out IDamagable damagable)) {
                    damagable.KnockbackDamage(damage, transform.position);
                }
                else {
                    Debug.LogWarning("Could not find damagable component!");
                }

                ObjectPoolManager.ReturnObjectToPool(gameObject);
            }
        }
    }
}