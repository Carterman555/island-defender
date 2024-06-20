using System;
using UnityEngine;

namespace IslandDefender {
    public class EnemyAnimator : MonoBehaviour, IAnimationTrigger {
        public event Action<AnimationTriggerType> OnAnimationTriggered;

        [SerializeField] private Enemy enemy;

        // played by animation
        private void AnimationTriggerEvent(AnimationTriggerType triggerType) {
            OnAnimationTriggered?.Invoke(triggerType);

            enemy.AnimationTriggerEvent(triggerType);
        }
    }
}