using System;
using TarodevController;
using UnityEngine;

namespace IslandDefender {
	public class PlayerAnimator : MonoBehaviour, IAnimationTrigger {

        public event Action<AnimationTriggerType> OnAnimationTriggered;

		[SerializeField] private PlayerController playerController;
        private Animator anim;

        private void Awake() {
            anim = GetComponent<Animator>();
        }

        private void Update() {
            HandleWalk();
        }

        private void HandleWalk() {
            anim.SetBool("walking", playerController.FrameInput.x != 0);
        }

        // played by animation
        private void AnimationTriggerEvent(AnimationTriggerType triggerType) {
            OnAnimationTriggered?.Invoke(triggerType);
        }
    }
}