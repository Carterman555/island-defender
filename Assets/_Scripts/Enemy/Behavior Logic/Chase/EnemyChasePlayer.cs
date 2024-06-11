using UnityEngine;

namespace IslandDefender {
    [CreateAssetMenu(fileName = "Chase-Chase Player", menuName = "Enemy Logic/Chase Logic/Chase Player")]
    public class EnemyChasePlayer : EnemyIdleSOBase // inherits from idle base because _enemy.UnitAggroed is null and the logic works better (not clean code though)
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

        public override void DoFrameUpdateLogic() {
            base.DoFrameUpdateLogic();

            // -1 or 1 - left or right
            int direction = _transform.position.x < player.position.x ? 1 : -1;
            enemy.SetEnemyXVel(enemy.GetMoveSpeed() * direction);
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
        }
    }
}