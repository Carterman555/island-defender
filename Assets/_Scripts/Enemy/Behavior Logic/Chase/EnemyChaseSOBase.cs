using UnityEngine;

namespace IslandDefender {
    public class EnemyChaseSOBase : EnemySOBase {
        public override void Initialize(GameObject gameObject, Enemy enemy) {
            _gameObject = gameObject;
            _transform = _gameObject.transform;
            base.enemy = enemy;
        }

        public override void DoEnterLogic() { }

        public override void DoExitLogic() { base.DoExitLogic(); }

        public override void DoFrameUpdateLogic() {
            // if the enemy isn't detecting any units, switch to idle state
            if (enemy.ObjectAggroed == null) {
                enemy.StateMachine.ChangeState(enemy.IdleState);
            }

            // if the enemy detects units within striking distance, switch to attack state
            if (enemy.ObjectWithinStrikingDistance != null) {
                enemy.StateMachine.ChangeState(enemy.AttackState);
            }
        }

        public override void DoPhysicsUpdateLogic() { }

        public override void DoAnimationTriggerEventLogic(AnimationTriggerType triggerType) { }

        public override void ResetValues() { }
    }
}