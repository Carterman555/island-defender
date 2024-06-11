using System;
using UnityEngine;

namespace IslandDefender {

	public interface IAnimationTrigger {

        public event Action<AnimationTriggerType> OnAnimationTriggered;
    }
    public enum AnimationTriggerType {
        Die,
        MeleeAttack,
        RangedAttack,
        Pickup,
    }
}