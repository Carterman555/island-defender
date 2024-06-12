using UnityEngine;

namespace IslandDefender {
    [CreateAssetMenu(fileName = "Chase Player", menuName = "Enemy Logic/Chase Player")]
    public class EnemyChasePlayer : EnemySOBase // inherits from idle base because _enemy.UnitAggroed is null and the logic works better (not clean code though)
    {
        private Transform player;

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

            // -1 or 1 - left or right
            int direction = _transform.position.x < player.position.x ? 1 : -1;
            enemy.SetEnemyXVel(enemy.GetMoveSpeed() * direction);
        }

        public override void PhysicsUpdate() {
            base.PhysicsUpdate();
        }

        public override void Initialize(GameObject gameObject, Enemy enemy) {
            base.Initialize(gameObject, enemy);

            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        public override void ResetValues() {
            base.ResetValues();
        }
    }
}