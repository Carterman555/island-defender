using IslandDefender.Utilities;
using UnityEngine;

namespace IslandDefender.Environment {
    public class DamagableResource : Health {

        [SerializeField] private ResourceType resourceType;
        [SerializeField] private RandomInt resourceDropAmount;

        [SerializeField] private float maxHealth = 5;

        protected override void ResetValues() {
            base.ResetValues();
            health = maxHealth;
        }

        protected override void Die() {
            PlayerResources.Instance.AddResource(resourceType, resourceDropAmount.Randomize());
            base.Die();
        }
    }

    public enum ResourceType {
        Wood,
        Fiber,
        Stone,
        Gold,
    }
}


