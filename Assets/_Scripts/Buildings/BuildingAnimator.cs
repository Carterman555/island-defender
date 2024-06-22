using System;
using UnityEngine;

namespace IslandDefender {
	public class BuildingAnimator : MonoBehaviour, IAnimationTrigger {
        public event Action<AnimationTriggerType> OnAnimationTriggered;

        // played by animation
        private void AnimationTriggerEvent(AnimationTriggerType triggerType) {
            OnAnimationTriggered?.Invoke(triggerType);
        }
    }
}