using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IslandDefender {

	/// <summary>
	/// Spawns in enemies in waves. The waves grow in difficulty and the amount and type of enemies are randomly spawned
	/// in based on the difficultly.
	/// </summary>
	public class EnemyWaveManager : StaticInstance<EnemyWaveManager> {

        public static event Action<int> OnNewWave;

        [SerializeField] private Transform leftSpawnPoint;
        [SerializeField] private Transform rightSpawnPoint;

        [SerializeField] private ScriptableWave[] allWaves;
        private ScriptableWave[] availableWaves;

        [SerializeField] private GameObject previewContainer;
        [SerializeField] private EnemyPreview[] leftSideEnemyPreviews;
        [SerializeField] private EnemyPreview[] rightSideEnemyPreviews;

        private WaveSpawner leftWaveSpawner;
        private WaveSpawner rightWaveSpawner;

        private float currentDifficulty;
		private int currentWave = 1;

        [SerializeField] private float timeBetweenWaves;
        private float dayTimeTimer;

        public bool IsDayTime(out float timeLeft) {
            timeLeft = dayTimeTimer;
            return dayTimeTimer > 0;
        }

        protected override void Awake() {
            base.Awake();
			float startingDifficulty = 5;
            currentDifficulty = startingDifficulty;
        }

        private void Start() {
            dayTimeTimer = timeBetweenWaves;
            SetupCurrentWave();
        }

        private void Update() {
            dayTimeTimer -= Time.deltaTime;
            if (dayTimeTimer < 0) {
                dayTimeTimer = timeBetweenWaves;
                PlayCurrentWave();
            }

            if (leftWaveSpawner == null || rightWaveSpawner == null) {
                return;
            }

            if (leftWaveSpawner.Completed() && rightWaveSpawner.Completed()) {
                ContinueNextWave();
                SetupCurrentWave();
            }
        }

        Dictionary<EnemyType, int> leftSideEnemyAmounts;
        Dictionary<EnemyType, int> rightSideEnemyAmounts;

        private void ContinueNextWave() {
            currentWave++;

            float difficultyIncrease = 3;
            currentDifficulty += difficultyIncrease;

            leftWaveSpawner = null;
            rightWaveSpawner = null;
        }

        private void SetupCurrentWave() {
            
            UpdateAvailableWaves();

            ShowPreviews();

            // setup left side
            ScriptableWave leftChosenWave = availableWaves.RandomItem();
            leftSideEnemyAmounts = CalculateEnemyAmounts(leftChosenWave);
            UpdatePreviews(leftSideEnemyPreviews, leftSideEnemyAmounts);

            // setup right side
            ScriptableWave rightChosenWave = availableWaves.RandomItem();
            rightSideEnemyAmounts = CalculateEnemyAmounts(rightChosenWave);
            UpdatePreviews(rightSideEnemyPreviews, rightSideEnemyAmounts);
        }

        private void PlayCurrentWave() {
            HidePreviews();

            leftWaveSpawner = new WaveSpawner(this, leftSideEnemyAmounts, currentDifficulty, leftSpawnPoint.position);
            rightWaveSpawner = new WaveSpawner(this, rightSideEnemyAmounts, currentDifficulty, rightSpawnPoint.position);

            OnNewWave?.Invoke(currentWave);
        }

        // find the wave that can be spawned depending on day (harder enemies can only be spawned on later waves)
        private void UpdateAvailableWaves() {
            availableWaves = allWaves.Where(wave => currentWave >= wave.MinWave).ToArray();
        }

        private void UpdatePreviews(EnemyPreview[] enemyPreviews, Dictionary<EnemyType, int> enemyAmounts) {
            foreach (EnemyPreview preview in enemyPreviews) {
                preview.UpdateText(enemyAmounts);
            }
        }

        private void HidePreviews() {
            previewContainer.SetActive(false);
        }

        private void ShowPreviews() {
            previewContainer.SetActive(true);
        }

        private Dictionary<EnemyType, int> CalculateEnemyAmounts(ScriptableWave scriptableWave) {

            Dictionary<EnemyType, int> enemyAmounts = new();
            foreach (EnemyType enemyType in Enum.GetValues(typeof(EnemyType))) {
                enemyAmounts.Add(enemyType, 0);
            }

            float difficultyValueRemaining = currentDifficulty;

            while (difficultyValueRemaining > 0) {
                EnemyType enemyToSpawn = ChoseRandomEnemy(scriptableWave);
                enemyAmounts[enemyToSpawn]++;

                float strengthOfSpawned = ResourceSystem.Instance.GetEnemy(enemyToSpawn).Strength;
                difficultyValueRemaining -= strengthOfSpawned;
            }

            return enemyAmounts;
        }

        // each enemy has a weight, the higher the weight the most likely an enemy is to get picked.
        // if deciding between two where one has weight of 80 and the other of 20, the one with 80 has
        // a 80% chance of getting picked
        private EnemyType ChoseRandomEnemy(ScriptableWave scriptableWave) {

            float totalWeight = scriptableWave.GetEnemyWeights().Values.Sum();

            float randomValue = UnityEngine.Random.Range(0, totalWeight);
            float cumulativeWeight = 0;

            foreach (EnemyType enemyType in Enum.GetValues(typeof(EnemyType))) {
                float weight = scriptableWave.GetEnemyWeights()[enemyType];
                cumulativeWeight += weight;

                if (randomValue <= cumulativeWeight) {
                    return enemyType;
                }
            }

            // Should never reach here if weights are properly assigned
            throw new InvalidOperationException("Failed to pick an enemy based on weights.");
        }
    }
}