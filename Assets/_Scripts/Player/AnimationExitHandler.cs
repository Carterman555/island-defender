using UnityEngine;

namespace IslandDefender {
    public class AnimationExitHandler : StateMachineBehaviour {

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            if (stateInfo.IsName("GatherResource")) {
                var playerPickup = animator.GetComponentInParent<PlayerPickup>();
                if (playerPickup != null) {
                    playerPickup.StopPickingUp();
                }
            }
        }
    }
}
