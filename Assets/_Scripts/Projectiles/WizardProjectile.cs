using IslandDefender.Management;
using UnityEngine;

namespace IslandDefender {
    public class WizardProjectile : MonoBehaviour, IDirectionProjectile {

        [SerializeField] private float speed;

        [SerializeField] private LayerMask allyLayerMask;

        private float direction;
        private float damage;

        public void Shoot(int direction, float damage) {
            this.direction = direction;
            this.damage = damage;
        }

        private void Update() {
            transform.position += new Vector3(direction * speed * Time.deltaTime, 0);
        }

        // destroy when hit ground, damage and destroy when hit enemy
        private void OnTriggerEnter2D(Collider2D collision) {

            if (allyLayerMask.ContainsLayer(collision.gameObject.layer)) {
                if (collision.TryGetComponent(out ProjectileImmunity projectileImmunity)) {
                    return;
                }

                if (collision.TryGetComponent(out IDamagable damagable)) {
                    damagable.KnockbackDamage(damage, transform.position);
                }
                else {
                    Debug.LogWarning("Ally Layer Does Not Have IDamagable!");
                }

                ObjectPoolManager.ReturnObjectToPool(gameObject);
            }
        }
    }
}