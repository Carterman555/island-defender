using IslandDefender.Management;
using IslandDefender.Utilities;
using System;
using UnityEngine;

namespace IslandDefender.Environment {
    public class DamagableResource : MonoBehaviour, IDamagable {

        public event Action<Vector3> OnDamaged;

        [SerializeField] private ResourceType resourceType;
        [SerializeField] private RandomInt resourceDropAmount;

        private float health = 3;

        public void Damage(float damage, Vector3 attackerPosition) {
            health -= damage;

            if (health <= 0) {
                Die();
            }
        }

        private void Die() {
            PlayerResources.Instance.AddResource(resourceType, resourceDropAmount.Randomize());
            ObjectPoolManager.ReturnObjectToPool(gameObject);
        }
    }

    public enum ResourceType {
        Wood,
        Fiber,
        Stone,
    }
}


