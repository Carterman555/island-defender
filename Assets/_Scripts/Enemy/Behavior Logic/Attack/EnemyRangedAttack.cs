using IslandDefender.Environment;
using IslandDefender.Management;
using UnityEngine;

namespace IslandDefender {
    [CreateAssetMenu(fileName = "RangedAttack", menuName = "Enemy Logic/Ranged Attack")]
    public class EnemyRangedAttack : EnemySOBase {

        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private Vector2 projectileOffset;

        private float timer;

        public override void DoAnimationTriggerEventLogic(AnimationTriggerType triggerType) {
            base.DoAnimationTriggerEventLogic(triggerType);

            if (triggerType.Equals(AnimationTriggerType.RangedAttack)) {
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

            GameObject poisonBall = ObjectPoolManager.SpawnObject(projectilePrefab,
                enemy.transform.position + (Vector3)projectileOffset,
                Quaternion.identity,
                Containers.Instance.Projectiles);

            if (poisonBall.TryGetComponent(out IDirectionProjectile dDrojectile)) {
                dDrojectile.Shoot(direction, enemy.Stats.Damage);
            }
            else if (poisonBall.TryGetComponent(out IPositionProjectile pProjectile)) {
                pProjectile.Shoot(targetPos, enemy.Stats.Damage);
            }
            else {
                Debug.LogError("Projectile Prefab Does Not Have IProjectile Component!");
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