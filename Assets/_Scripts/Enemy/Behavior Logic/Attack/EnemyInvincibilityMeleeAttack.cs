using UnityEngine;

namespace IslandDefender {
    [CreateAssetMenu(fileName = "InvincibilityMeleeAttack", menuName = "Enemy Logic/Invincibility Melee Attack")]
    public class EnemyInvincibilityMeleeAttack : EnemyAttackMelee {

        public override void DoEnterLogic() {
            base.DoEnterLogic();

            enemy.Health.SetInvincible(true);
            enemy.SetEnemyXVel(0);
        }

        public override void DoExitLogic() {
            base.DoExitLogic();

            enemy.Health.SetInvincible(false);
        }
    }
}