using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

namespace IslandDefender {

	/// <summary>
	/// Spawns in enemies in waves. The waves grow in difficulty and the amount and type of enemies are randomly spawned
	/// in based on the difficultly.
	/// </summary>
	public class EnemyWaveManager : MonoBehaviour {

        public static event Action<int> OnNewWave;

        [SerializeField] private Transform[] spawnPoints;

        private List<EnemyType> availableEnemies = new List<EnemyType>();

        private float currentDifficulty;
		private int currentWave = 1;

        private float timeBetweenWaves = 10;

        public Vector3 GetRandomSpawnPos() {
            return spawnPoints.RandomItem().position;
        }

        private void Awake() {
			float startingDifficulty = 5;
            currentDifficulty = startingDifficulty;
        }

        private void Start() {
            UpdateAvailableEnemies();

            new WaveSpawner(this, availableEnemies, currentDifficulty);
        }

        public IEnumerator FinishedSpawningWave() {
            yield return new WaitForSeconds(timeBetweenWaves);
            NextWave();
        }

        private void NextWave() {
			currentWave++;

			float difficultyIncrease = 3;
			currentDifficulty += difficultyIncrease;

            UpdateAvailableEnemies();

            new WaveSpawner(this, availableEnemies, currentDifficulty);

            OnNewWave?.Invoke(currentWave);
        }

        // find the enemies that can be spawned depending on wave (harder enemies can only be spawned on later waves)
        private void UpdateAvailableEnemies() {
            availableEnemies.Clear();
            foreach (EnemyType enemyType in Enum.GetValues(typeof(EnemyType))) {
                if (currentWave >= ResourceSystem.Instance.GetEnemy(enemyType).StartingWave) {
                    availableEnemies.Add(enemyType);
                }
            }
        }
    }
}