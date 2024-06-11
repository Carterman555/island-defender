using UnityEngine;

namespace IslandDefender {
    [CreateAssetMenu(fileName = "Idle-Patrol", menuName = "Enemy Logic/Idle Logic/Patrol")]
    public class EnemyIdlePatrol : EnemyIdleSOBase {
        private protected float _leftPoint, _rightPoint;
        private int _direction = -1; // -1: left, 1: right

        [SerializeField] private float _patrolSlowMult;

        [SerializeField] private float _stopTime;
        private float _patrolStopTimer;

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

            _patrolStopTimer += Time.deltaTime;
            if (_patrolStopTimer < _stopTime) {
                enemy.SetEnemyXVel(0);
                return;
            }

            enemy.SetEnemyXVel(enemy.GetMoveSpeed() * _patrolSlowMult * _direction);

            // if going left and goes past the left point
            if (_direction == -1 && _transform.position.x < _leftPoint) {
                // stop for stop time
                _patrolStopTimer = 0;

                // switch direction to right
                _direction = 1;
            }

            // if going right and goes past the right point
            else if (_direction == 1 && _transform.position.x > _rightPoint) {
                // stop for stop time
                _patrolStopTimer = 0;

                // switch direction to left
                _direction = -1;
            }
        }

        public override void DoPhysicsUpdateLogic() {
            base.DoPhysicsUpdateLogic();
        }

        public override void Initialize(GameObject gameObject, Enemy enemy) {
            base.Initialize(gameObject, enemy);

            //if (_enemy is IEnemyBoundedMovement _boundedMovement) {
            //    _leftPoint = _boundedMovement.GetLeftPoint();
            //    _rightPoint = _boundedMovement.GetRightPoint();
            //}
            //else
            //    Debug.LogWarning("Patrol Logic Attatched Without IBoundedMovement On " + enemy.name);
        }

        public override void ResetValues() {
            base.ResetValues();
        }
    }
}