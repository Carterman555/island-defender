using IslandDefender.Environment;
using IslandDefender.Units.Player;
using System;
using TarodevController;
using UnityEngine;

namespace IslandDefender {
	public class PlayerPickup : MonoBehaviour  {

		public static event Action OnPickup;

        private PlayerController playerController;

        [SerializeField] private PlayerAnimator playerAnimator;
        [SerializeField] private Animator anim;

        [SerializeField] private TriggerContactTracker resourceContactTracker;

        private void Awake() {
            playerController = GetComponent<PlayerController>();
        }

        private void OnEnable() {
            playerAnimator.OnAnimationTriggered += TryPickup;
        }
        private void OnDisable() {
            playerAnimator.OnAnimationTriggered -= TryPickup;
        }

        private void TryPickup(AnimationTriggerType animationTriggerType) {
            if (resourceContactTracker.GetContacts().Count > 0 && animationTriggerType == AnimationTriggerType.Pickup) {

                IPickupable pickupable = resourceContactTracker.GetContacts()[0].GetComponent<IPickupable>();
                pickupable.Pickup();
                OnPickup?.Invoke();
            }
        }

        // this acts as a safety so the player doesn't stay stuck forever if
        // somehow the resource cannot be picked up
        public void StopPickingUp() {
            playerController.EnableMovement();
        }

        private void Update() {
            if (resourceContactTracker.GetContacts().Count > 0 && Input.GetKeyDown(KeyCode.E)) {
                anim.SetTrigger("gather");

                playerController.DisableMovement();
            }
        }
    }
}