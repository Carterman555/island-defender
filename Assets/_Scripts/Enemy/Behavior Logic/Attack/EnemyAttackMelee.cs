using UnityEngine;

namespace IslandDefender {
    [CreateAssetMenu(fileName = "Attack-Melee", menuName = "Enemy Logic/Attack Logic/Melee Attack")]
    public class EnemyAttackMelee : EnemyAttackSOBase {
        private float _timer;

        public override void DoAnimationTriggerEventLogic(AnimationTriggerType triggerType) {
            base.DoAnimationTriggerEventLogic(triggerType);

            if (triggerType.Equals(AnimationTriggerType.MeleeAttack)) {
                Attack();
            }
        }

        protected virtual void Attack() {
            if (enemy.ObjectWithinStrikingDistance == null) {
                return;
            }

            // set enemy facing correct direction
            Vector2 targetPos = enemy.ObjectWithinStrikingDistance.transform.position;
            int direction = _transform.position.x < targetPos.x ? 1 : -1;
            enemy.CheckForLeftOrRightFacing(direction);

            enemy.ObjectWithinStrikingDistance.GetComponent<IDamagable>().Damage(enemy.Stats.Damage, _transform.position);

            // deal damage
            /*if (_enemy.ObjectWithinStrikingDistance.TryGetComponent<IDamagable>(out var damagable)) {
                damagable.Damage(_enemy.Stats.AttackPowerInt, _transform.position);
            }*/
        }

        public override void DoEnterLogic() {
            base.DoEnterLogic();
        }

        public override void DoExitLogic() {
            base.DoExitLogic();
        }

        public override void DoFrameUpdateLogic() {
            base.DoFrameUpdateLogic();

            if (enemy.ObjectWithinStrikingDistance == null) return;

            enemy.SetEnemyXVel(0);

            _timer += Time.deltaTime;
            if (_timer > enemy.Stats.AttackCooldown) {
                // play animation which will play Attack()
                enemy.Anim.SetTrigger("attack");

                _timer = 0f;
            }

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