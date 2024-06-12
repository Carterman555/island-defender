using UnityEngine;

namespace IslandDefender {
    public class EnemyStateMachine {
        public EnemySOBase CurrentEnemyState { get; set; }

        public void Initialize(EnemySOBase startingState) {
            CurrentEnemyState = startingState;
            CurrentEnemyState.DoEnterLogic();
        }

        public void ChangeState(EnemySOBase newState) {
            CurrentEnemyState.DoExitLogic();
            CurrentEnemyState = newState;
            CurrentEnemyState.DoEnterLogic();
        }
    }
}