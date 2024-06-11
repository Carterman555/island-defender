using IslandDefender.Management;
using IslandDefender.Environment;
using UnityEngine;

namespace IslandDefender {
    public class PoisonBallProjectile : MonoBehaviour, IProjectile {

        [SerializeField] private TouchDamage poisonAreaPrefab;
        [SerializeField] private Vector2 shootForce;

        private float damage;

        public void Shoot(int direction, float damage) {
            this.damage = damage;

            // add force that lobs projectile
            Vector2 directionalForce = new Vector2(shootForce.x * direction, shootForce.y);
            GetComponent<Rigidbody2D>().AddForce(directionalForce);
        }

        private void OnTriggerEnter2D(Collider2D collision) {
            if (collision.gameObject.layer == GameLayers.GroundLayer) {
                SplashPoison();
                ObjectPoolManager.ReturnObjectToPool(gameObject);
            }
        }

        private void SplashPoison() {
            ObjectPoolManager.SpawnObject(poisonAreaPrefab, transform.position, Quaternion.identity, Containers.Instance.Projectiles);
        }
    }
}
