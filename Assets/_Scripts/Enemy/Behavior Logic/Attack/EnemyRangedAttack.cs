using IslandDefender.Environment;
using IslandDefender.Management;
using UnityEngine;

namespace IslandDefender {
    [CreateAssetMenu(fileName = "Attack-Ranged", menuName = "Enemy Logic/Attack Logic/Ranged Attack")]
    public class EnemyRangedAttack : EnemyAttackSOBase {

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
            if (enemy.ObjectWithinStrikingDistance == null) {
                return;
            }

            // set enemy facing correct direction
            Vector2 targetPos = enemy.ObjectWithinStrikingDistance.transform.position;
            int direction = _transform.position.x < targetPos.x ? 1 : -1;
            enemy.CheckForLeftOrRightFacing(direction);

            GameObject poisonBall = ObjectPoolManager.SpawnObject(projectilePrefab,
                enemy.transform.position + (Vector3)projectileOffset,
                Quaternion.identity,
                Containers.Instance.Projectiles);
            
            if (poisonBall.TryGetComponent(out IProjectile projectile)) {
                projectile.Shoot(direction, enemy.Stats.Damage);
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

        public override void DoFrameUpdateLogic() {
            base.DoFrameUpdateLogic();

            if (enemy.ObjectWithinStrikingDistance == null) return;

            timer += Time.deltaTime;
            if (timer > enemy.Stats.AttackCooldown) {
                // play animation which will play Attack()
                //enemy.Anim.SetTrigger("attack");

                Attack();

                timer = 0f;
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