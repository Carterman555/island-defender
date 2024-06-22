using DG.Tweening;
using IslandDefender.Utilities;
using UnityEngine;

namespace IslandDefender.Environment {
    public class DamagableResource : Health {

        [SerializeField] private ResourceType resourceType;
        [SerializeField] private RandomInt resourceDropAmount;

        public override void Damage(float damage) {
            base.Damage(damage);

            if (!IsDead()) {
                float duration = 0.25f;
                float strength = 10f;
                int vibrato = 5;
                float randomness = 90f;
                transform.DOShakeRotation(duration, strength, vibrato, randomness);
            }
        }

        public override void Die() {
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


