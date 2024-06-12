using UnityEngine;

namespace IslandDefender {
    [CreateAssetMenu(fileName = "Direct Chase", menuName = "Enemy Logic/Direct Chase")]
    public class EnemyDirectChase : EnemySOBase {
        public override void DoAnimationTriggerEventLogic(AnimationTriggerType triggerType) {
            base.DoAnimationTriggerEventLogic(triggerType);
        }

        public override void DoEnterLogic() {
            base.DoEnterLogic();
        }

        public override void DoExitLogic() {
            base.DoExitLogic();
        }

        public override void FrameUpdate() {
            base.FrameUpdate();

            if (enemy.GetMidrangeObject() == null) {
                return;
            }

            // -1 or 1 - left or right
            int direction = _transform.position.x < enemy.GetMidrangeObject().transform.position.x ? 1 : -1;

            enemy.SetEnemyXVel(enemy.GetMoveSpeed() * direction);
        }

        public override void PhysicsUpdate() {
            base.PhysicsUpdate();
        }

        public override void Initialize(GameObject gameObject, Enemy enemy) {
            base.Initialize(gameObject, enemy);
        }

        public override void ResetValues() {
            base.ResetValues();
        }
    }
}