namespace IslandDefender {
    public class EnemyAttackState : EnemyState {
        public EnemyAttackState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine) {
        }

        public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType) {
            base.AnimationTriggerEvent(triggerType);

            _enemy.EnemyAttackBaseInstance.DoAnimationTriggerEventLogic(triggerType);
        }

        public override void EnterState() {
            base.EnterState();

            _enemy.EnemyAttackBaseInstance.DoEnterLogic();
        }

        public override void ExitState() {
            base.ExitState();

            _enemy.EnemyAttackBaseInstance.DoExitLogic();
        }

        public override void FrameUpdate() {
            base.FrameUpdate();

            _enemy.EnemyAttackBaseInstance.DoFrameUpdateLogic();
        }

        public override void PhysicsUpdate() {
            base.PhysicsUpdate();

            _enemy.EnemyAttackBaseInstance.DoPhysicsUpdateLogic();
        }
    }
}