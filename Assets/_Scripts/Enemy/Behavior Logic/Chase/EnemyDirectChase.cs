using UnityEngine;

namespace IslandDefender {
    [CreateAssetMenu(fileName = "Chase-Direct Chase", menuName = "Enemy Logic/Chase Logic/Direct Chase")]
    public class EnemyDirectChase : EnemyChaseSOBase {
        public override void DoAnimationTriggerEventLogic(AnimationTriggerType triggerType) {
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

            if (enemy.ObjectAggroed == null) {
                return;
            }

            // -1 or 1 - left or right
            int direction = _transform.position.x < enemy.ObjectAggroed.transform.position.x ? 1 : -1;

            enemy.SetEnemyXVel(enemy.GetMoveSpeed() * direction);
        }

        public override void DoPhysicsUpdateLogic() {
            base.DoPhysicsUpdateLogic();
        }

        public override void Initialize(GameObject gameObject, Enemy enemy) {
            base.Initialize(gameObject, enemy);
        }

        public override void ResetValues() {
            base.ResetValues();
        }
    }
}