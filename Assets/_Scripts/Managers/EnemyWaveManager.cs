using UnityEngine;

namespace IslandDefender {

	/// <summary>
	/// Spawns in enemies in waves. The waves grow in difficulty and the amount and type of enemies are randomly spawned
	/// in based on the difficultly.
	/// </summary>
	public class EnemyWaveManager : MonoBehaviour {

		private float currentDifficulty;
		private int currentWave = 1;

        private void Awake() {
			float startingDifficulty = 10;
            currentDifficulty = startingDifficulty;
        }

        private void NextWave() {
			currentWave++;

			float difficultyIncrease = 5;
			currentDifficulty += difficultyIncrease;
        }
    }
}