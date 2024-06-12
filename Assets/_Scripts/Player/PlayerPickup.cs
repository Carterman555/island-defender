using IslandDefender.Environment;
using System;
using UnityEngine;

namespace IslandDefender {
	public class PlayerPickup : MonoBehaviour {

		public static event Action OnPickup;

        private IPickupable pickupableTouching;

        [SerializeField] private PlayerAnimator playerAnimator;
        [SerializeField] private Animator anim;

        private void OnTriggerEnter2D(Collider2D collision) {
            if (collision.TryGetComponent(out IPickupable pickupable)) {
                pickupableTouching = pickupable;
            }
        }
        private void OnTriggerExit2D(Collider2D collision) {
            if (collision.TryGetComponent(out IPickupable pickupable)) {
                pickupableTouching = null;
            }
        }

        private void OnEnable() {
            playerAnimator.OnAnimationTriggered += TryPickup;
        }
        private void OnDisable() {
            playerAnimator.OnAnimationTriggered -= TryPickup;
        }

        private void TryPickup(AnimationTriggerType animationTriggerType) {
            if (pickupableTouching != null && animationTriggerType == AnimationTriggerType.Pickup) {

                pickupableTouching.Pickup();
                inPickupAnimation = false;

                OnPickup?.Invoke();
            }
        }

        private bool inPickupAnimation = false;

        private void Update() {
            if (!inPickupAnimation && pickupableTouching != null && Input.GetKeyDown(KeyCode.E)) {
                anim.SetTrigger("gather");
                inPickupAnimation = true;
            }
        }
    }
}