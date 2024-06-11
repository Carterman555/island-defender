namespace IslandDefender {
    public class EnemyIdleState : EnemyState {
        public EnemyIdleState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine) {
        }

        public override void AnimationTriggerEvent(AnimationTriggerType triggerType) {
            base.AnimationTriggerEvent(triggerType);

            _enemy.EnemyIdleBaseInstance.DoAnimationTriggerEventLogic(triggerType);
        }

        public override void EnterState() {
            base.EnterState();

            _enemy.EnemyIdleBaseInstance.DoEnterLogic();
        }

        public override void ExitState() {
            base.ExitState();

            _enemy.EnemyIdleBaseInstance.DoExitLogic();
        }

        public override void FrameUpdate() {
            base.FrameUpdate();

            _enemy.EnemyIdleBaseInstance.DoFrameUpdateLogic();
        }

        public override void PhysicsUpdate() {
            base.PhysicsUpdate();

            _enemy.EnemyIdleBaseInstance.DoPhysicsUpdateLogic();
        }
    }
}