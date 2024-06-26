using IslandDefender.Environment;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IslandDefender {

	/// <summary>
	/// Spawns in enemies in waves. The waves grow in difficulty and the amount and type of enemies are randomly spawned
	/// in based on the difficultly.
	/// </summary>
	public class EnemyWaveManager : StaticInstance<EnemyWaveManager> {

        public static event Action<int> OnStartWave;
        public static event Action<int> OnNextWave;

        [SerializeField] private Transform leftSpawnPoint;
        [SerializeField] private Transform rightSpawnPoint;

        private WaveSpawner leftWaveSpawner;
        private WaveSpawner rightWaveSpawner;

        [SerializeField] private float[] spawnIntervals;

        [Range(1f, 2f)]
        [SerializeField] private float difficultyCurve = 1.6f;
		private int currentWave = 1;

        public int GetCurrentWave() {
            return currentWave;
        }

        private void OnEnable() {
            DayNightCycle.OnNightTime += PlayCurrentWave;
        }
        private void OnDisable() {
            DayNightCycle.OnNightTime -= PlayCurrentWave;
        }

        private void Start() {
            SetupCurrentWave();
        }

        private void Update() {

            if (leftWaveSpawner == null || rightWaveSpawner == null) {
                return;
            }

            if (leftWaveSpawner.Completed() && rightWaveSpawner.Completed() && !AnyEnemiesAlive()) {
                ContinueNextWave();
                SetupCurrentWave();

                DayNightCycle.Instance.EndNightTime();
            }
        }

        Dictionary<EnemyType, int> leftSideEnemyAmounts;
        Dictionary<EnemyType, int> rightSideEnemyAmounts;

        private void ContinueNextWave() {
            currentWave++;

            leftWaveSpawner = null;
            rightWaveSpawner = null;

            OnNextWave?.Invoke(currentWave);
        }

        private float GetCurrentDifficulty() {
            float difficulty = Mathf.Pow(currentWave, difficultyCurve) + 4;
            return difficulty;
        }

        private void SetupCurrentWave() {

            // setup left side
            leftSideEnemyAmounts = CalculateEnemyAmounts(ChooseEnemyWeights());

            // setup right side
            rightSideEnemyAmounts = CalculateEnemyAmounts(ChooseEnemyWeights());
        }

        public void PlayCurrentWave() {
            float currentInterval = spawnIntervals[currentWave - 1];
            leftWaveSpawner = new WaveSpawner(this, leftSideEnemyAmounts, currentInterval, leftSpawnPoint.position, false);
            rightWaveSpawner = new WaveSpawner(this, rightSideEnemyAmounts, currentInterval, rightSpawnPoint.position, true);

            OnStartWave?.Invoke(currentWave);
        }

        // give each available enemy a weight for the wave, so waves are unique
        private Dictionary<EnemyType, float> ChooseEnemyWeights() {

            Dictionary<EnemyType, float> enemyWeights = new();
            foreach (EnemyType enemyType in Enum.GetValues(typeof(EnemyType))) {

                ScriptableEnemy scriptableEnemy = ResourceSystem.Instance.GetEnemy(enemyType);

                if (!scriptableEnemy.SpawnInWave) {
                    continue;
                }

                int startingWave = scriptableEnemy.StartingWave;
                if (currentWave >= startingWave) {

                    int randomWeight = 1;
                    if (UnityEngine.Random.value < 0.25f) { // 25% chance
                        randomWeight = 4;
                    }
                    enemyWeights.Add(enemyType, randomWeight);
                }
            }
            return enemyWeights;
        }

        private Dictionary<EnemyType, int> CalculateEnemyAmounts(Dictionary<EnemyType, float> enemyWeights) {

            Dictionary<EnemyType, int> enemyAmounts = new();
            foreach (EnemyType enemyType in Enum.GetValues(typeof(EnemyType))) {
                enemyAmounts.Add(enemyType, 0);
            }

            float difficultyValueRemaining = GetCurrentDifficulty();

            while (difficultyValueRemaining > 0) {
                EnemyType enemyToSpawn = ChoseRandomEnemy(enemyWeights);
                enemyAmounts[enemyToSpawn]++;

                float strengthOfSpawned = ResourceSystem.Instance.GetEnemy(enemyToSpawn).Strength;
                difficultyValueRemaining -= strengthOfSpawned;
            }

            return enemyAmounts;
        }

        // each enemy has a weight, the higher the weight the most likely an enemy is to get picked.
        // if deciding between two where one has weight of 80 and the other of 20, the one with 80 has
        // a 80% chance of getting picked
        private EnemyType ChoseRandomEnemy(Dictionary<EnemyType, float> enemyWeights) {

            float totalWeight = enemyWeights.Values.Sum();

            float randomValue = UnityEngine.Random.Range(0, totalWeight);
            float cumulativeWeight = 0;

            foreach (EnemyType enemyType in enemyWeights.Keys) {
                float weight = enemyWeights[enemyType];
                cumulativeWeight += weight;

                if (randomValue <= cumulativeWeight) {
                    return enemyType;
                }
            }

            // Should never reach here if weights are properly assigned
            throw new InvalidOperationException("Failed to pick an enemy based on weights.");
        }

        private bool AnyEnemiesAlive() {
            foreach(Transform enemy in Containers.Instance.WaveEnemies) {
                if (enemy.gameObject.activeSelf) {
                    return true;
                }
            }
            return false;
        }
    }
}