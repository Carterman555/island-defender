using System.Collections;
using UnityEngine;

namespace IslandDefender {

	/// <summary>
	/// Spawns in enemies in waves. The waves grow in difficulty and the amount and type of enemies are randomly spawned
	/// in based on the difficultly.
	/// </summary>
	public class EnemyWaveManager : MonoBehaviour {

		[SerializeField] private AnimationCurve spawnIntervalCurve;

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

		private IEnumerator SpawnWave() {

			float difficultyValueRemaining = currentDifficulty;

			while (difficultyValueRemaining > 0) {

				EnemyType enemyToSpawn = ChoseRandomEnemy(currentDifficulty, difficultyValueRemaining);
				SpawnEnemy(enemyToSpawn);
				
				int strengthOfSpawned = -1;
                difficultyValueRemaining -= strengthOfSpawned;

                float interval = GetSpawnInterval(currentDifficulty);
                yield return new WaitForSeconds(interval);
			}
		}

		private float GetSpawnInterval(float difficulty) {
            float avgInterval = spawnIntervalCurve.Evaluate(difficulty); // techinically not exactly avg, but close
            float intervalVariance = 0.3f;
            float interval = Random.Range(avgInterval * (1 - intervalVariance), avgInterval * (1 + intervalVariance));
			return interval;
        }

        // chose random enemy weighted based on level difficulty and time in wave (later in wave gets harder)
        private EnemyType ChoseRandomEnemy(float waveDifficulty, float difficultyValueRemaining) {
			return EnemyType.Frog;
		}

		// probably need resource system
		private void SpawnEnemy(EnemyType enemy) {

		}
    }
}