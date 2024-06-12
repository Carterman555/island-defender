using IslandDefender.Environment.Building;
using UnityEngine;

namespace IslandDefender {
    [CreateAssetMenu(fileName = "Chase Keep", menuName = "Enemy Logic/Chase Keep")]
    public class EnemyChaseKeep : EnemySOBase {
        private Transform keep;

        public override void FrameUpdate() {
            base.FrameUpdate();

            // -1 or 1 - left or right
            int direction = _transform.position.x < keep.position.x ? 1 : -1;
            enemy.SetEnemyXVel(enemy.GetMoveSpeed() * direction);
        }

        public override void Initialize(GameObject gameObject, Enemy enemy) {
            base.Initialize(gameObject, enemy);

            keep = FindObjectOfType<Keep>().transform;
        }
    }
}