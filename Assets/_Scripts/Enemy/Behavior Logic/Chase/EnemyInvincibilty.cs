using UnityEngine;

namespace IslandDefender {
    [CreateAssetMenu(fileName = "Invincibility", menuName = "Enemy Logic/Invincibility")]
    public class EnemyInvincibility : EnemySOBase {

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