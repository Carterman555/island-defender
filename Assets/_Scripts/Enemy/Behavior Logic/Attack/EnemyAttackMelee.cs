using System.Data;
using UnityEngine;

namespace IslandDefender {
    [CreateAssetMenu(fileName = "MeleeAttack", menuName = "Enemy Logic/Melee Attack")]
    public class EnemyAttackMelee : EnemySOBase {
        private float timer;

        public override void DoAnimationTriggerEventLogic(AnimationTriggerType triggerType) {
            base.DoAnimationTriggerEventLogic(triggerType);

            if (triggerType.Equals(AnimationTriggerType.MeleeAttack)) {
                Attack();
            }
        }

        protected virtual void Attack() {
            if (enemy.GetCloseObject() == null) {
                return;
            }

            // set enemy facing correct direction
            Vector2 targetPos = enemy.GetCloseObject().transform.position;
            int direction = _transform.position.x < targetPos.x ? 1 : -1;
            enemy.CheckForLeftOrRightFacing(direction);

            // deal damage
            if (enemy.GetCloseObject().TryGetComponent(out IDamagable damagable)) {
                damagable.KnockbackDamage(enemy.Stats.Damage, _transform.position);
            }
            else {
                Debug.LogWarning("Could Not Get Damagable Component From " + enemy.GetCloseObject().name);
            }
        }

        public override void DoEnterLogic() {
            base.DoEnterLogic();
        }

        public override void DoExitLogic() {
            base.DoExitLogic();
        }

        public override void FrameUpdate() {
            base.FrameUpdate();

            if (enemy.GetCloseObject() == null) return;

            enemy.SetEnemyXVel(0);

            timer += Time.deltaTime;
            if (timer > enemy.Stats.AttackCooldown) {
                // play animation which will play Attack()
                enemy.Anim.SetTrigger("attack");

                timer = 0f;
            }
        }

        public override void PhysicsUpdate() {
            base.PhysicsUpdate();
        }

        public override void Initialize(GameObject gameObject, Enemy enemy) {
            base.Initialize(gameObject, enemy);
        }

        public override void ResetValues() {
            base.ResetValues();
        }
    }
}