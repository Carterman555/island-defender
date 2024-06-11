using UnityEngine;

namespace IslandDefender {
    [CreateAssetMenu(fileName = "Attack-Stop Movement", menuName = "Enemy Logic/Attack Logic/Stop Movement")]
    public class EnemyAttackSOBase : EnemySOBase {
        public override void Initialize(GameObject gameObject, Enemy enemy) {
            _gameObject = gameObject;
            _transform = _gameObject.transform;
            base.enemy = enemy;
        }

        public override void DoEnterLogic() { }

        public override void DoExitLogic() { base.DoExitLogic(); }

        public override void DoFrameUpdateLogic() {
            // if the enemy isn't detecting any units within range to attack, switch to chase state
            if (enemy.ObjectWithinStrikingDistance == null) {
                enemy.StateMachine.ChangeState(enemy.ChaseState);
            }
        }

        public override void DoPhysicsUpdateLogic() { }

        public override void DoAnimationTriggerEventLogic(AnimationTriggerType triggerType) { }

        public override void ResetValues() { }
    }
}