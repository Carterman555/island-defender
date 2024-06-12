using UnityEngine;

namespace IslandDefender {
    public class EnemySOBase : ScriptableObject {
        protected Enemy enemy;
        protected Transform _transform;
        protected GameObject _gameObject;

        public virtual void Initialize(GameObject gameObject, Enemy enemy) {
            _gameObject = gameObject;
            _transform = _gameObject.transform;
            this.enemy = enemy;
        }

        public virtual void DoEnterLogic() { }

        public virtual void DoExitLogic() { ResetValues(); }

        public virtual void FrameUpdate() { }

        public virtual void PhysicsUpdate() { }

        public virtual void DoAnimationTriggerEventLogic(AnimationTriggerType triggerType) { }

        public virtual void ResetValues() { }
    }
}