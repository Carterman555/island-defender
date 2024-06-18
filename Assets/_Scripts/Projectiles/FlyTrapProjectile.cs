using IslandDefender.Management;
using UnityEngine;

namespace IslandDefender {
    public class FlyTrapProjectile : MonoBehaviour, IProjectile {

        [SerializeField] private float speed;
        [SerializeField] private float maxDistance;
        private float distance;

        private int direction;
        private float damage;

        public void Shoot(int direction, float damage) {
            this.direction = direction;
            this.damage = damage;
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

        private void Update() {

            float xDelta = direction * speed * Time.deltaTime;
            transform.position += new Vector3(xDelta, 0);

            distance += xDelta;

            if (distance > maxDistance) {
                ObjectPoolManager.ReturnObjectToPool(gameObject);
            }
        }
    }
}