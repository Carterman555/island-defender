using IslandDefender.Environment;
using System;
using UnityEngine;

namespace IslandDefender {
	public class PlayerPickup : MonoBehaviour {

		public static event Action OnPickup;

        [SerializeField] private PlayerAnimator playerAnimator;
        [SerializeField] private Animator anim;

        [SerializeField] private TriggerContactTracker resourceContactTracker;

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


        private void Update() {
            if (resourceContactTracker.GetContacts().Count > 0 && Input.GetKeyDown(KeyCode.E)) {
                anim.SetTrigger("gather");
            }
        }
    }
}