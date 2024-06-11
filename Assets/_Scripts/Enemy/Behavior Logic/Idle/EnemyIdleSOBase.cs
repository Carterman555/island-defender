using UnityEngine;

namespace IslandDefender {
    public class EnemyIdleSOBase : EnemySOBase {
        public override void Initialize(GameObject gameObject, Enemy enemy) {
            _gameObject = gameObject;
            _transform = _gameObject.transform;
            base.enemy = enemy;
        }

        public override void DoEnterLogic() { base.DoEnterLogic(); }

        public override void DoExitLogic() { }

        public override void DoFrameUpdateLogic() {
            if (enemy.ObjectAggroed != null) {
                enemy.StateMachine.ChangeState(enemy.ChaseState);
            }
            if (enemy.ObjectWithinStrikingDistance != null) {
                enemy.StateMachine.ChangeState(enemy.AttackState);
            }
        }

        public override void DoPhysicsUpdateLogic() { }

        public override void DoAnimationTriggerEventLogic(AnimationTriggerType triggerType) { }

        public override void ResetValues() { }
    }
}