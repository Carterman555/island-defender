namespace IslandDefender {
    public class EnemyChaseState : EnemyState {
        public EnemyChaseState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine) {
        }

        public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType) {
            base.AnimationTriggerEvent(triggerType);

            _enemy.EnemyChaseBaseInstance.DoAnimationTriggerEventLogic(triggerType);
        }

        public override void EnterState() {
            base.EnterState();

            _enemy.EnemyChaseBaseInstance.DoEnterLogic();
        }

        public override void ExitState() {
            base.ExitState();

            _enemy.EnemyChaseBaseInstance.DoExitLogic();
        }

        public override void FrameUpdate() {
            base.FrameUpdate();

            _enemy.EnemyChaseBaseInstance.DoFrameUpdateLogic();
        }

        public override void PhysicsUpdate() {
            base.PhysicsUpdate();

            _enemy.EnemyChaseBaseInstance.DoPhysicsUpdateLogic();
        }
    }
}