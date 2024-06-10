using IslandDefender.Management;
using UnityEngine;

namespace IslandDefender {
    public class Health : MonoBehaviour, IDamagable {

        [SerializeField] private float maxHealth;
        private float health;

        private void Awake() {
            health = maxHealth;
        }

        public void Damage(float damage, Vector3 attackerPosition) {
            health -= damage;

            if (health <= 0) {
                Die();
            }
        }

        protected virtual void Die() {
            ObjectPoolManager.ReturnObjectToPool(gameObject);
        }
    }
}
