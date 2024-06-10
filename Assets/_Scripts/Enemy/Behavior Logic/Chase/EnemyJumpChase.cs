using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace IslandDefender {
    [CreateAssetMenu(fileName = "Jump Chase", menuName = "Enemy Logic/Chase Logic/Jump Chase")]
    public class EnemyJumpChase : EnemyIdleSOBase {

        [SerializeField] private Vector2 jumpForce;
        [SerializeField] private float jumpCooldown;

        private float jumpTimer;

        private Transform player;

        public override void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType triggerType) {
            base.DoAnimationTriggerEventLogic(triggerType);
        }

        public override void DoEnterLogic() {
            base.DoEnterLogic();
        }

        public override void DoExitLogic() {
            base.DoExitLogic();
        }

        public override void DoFrameUpdateLogic() {
            base.DoFrameUpdateLogic();

            // -1 or 1 - left or right
            int direction = _transform.position.x < player.position.x ? 1 : -1;

            enemy.CheckForLeftOrRightFacing(enemy.GetMoveSpeed() * direction);

            jumpTimer += Time.deltaTime;
            Debug.Log("Jump counting");
            if (jumpTimer > jumpCooldown) {
                Debug.Log("Jump");
                jumpTimer = 0;
                enemy.RB.AddForce(new Vector2(jumpForce.x * direction, jumpForce.y), ForceMode2D.Impulse);
            }
        }

        public override void DoPhysicsUpdateLogic() {
            base.DoPhysicsUpdateLogic();
        }

        public override void Initialize(GameObject gameObject, Enemy enemy) {
            base.Initialize(gameObject, enemy);

            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        public override void ResetValues() {
            base.ResetValues();

            Debug.Log("Jump Reset");
            jumpTimer = 0;
        }
    }
}