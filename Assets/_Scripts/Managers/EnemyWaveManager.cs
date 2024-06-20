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

        [SerializeField] private ScriptableWave[] allWaves;
        private ScriptableWave[] availableWaves;

        [SerializeField] private GameObject previewContainer;
        [SerializeField] private EnemyPreview[] leftSideEnemyPreviews;
        [SerializeField] private EnemyPreview[] rightSideEnemyPreviews;

        private WaveSpawner leftWaveSpawner;
        private WaveSpawner rightWaveSpawner;

        [SerializeField] private float[] spawnIntervals;
        private float currentDifficulty;
		private int currentWave = 1;

        public int GetCurrentWave() {
            return currentWave;
        }

        protected override void Awake() {
            base.Awake();
			float startingDifficulty = 10;
            currentDifficulty = startingDifficulty;
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
                print("Wave Completed");
            }
        }

        Dictionary<EnemyType, int> leftSideEnemyAmounts;
        Dictionary<EnemyType, int> rightSideEnemyAmounts;

        private void ContinueNextWave() {
            currentWave++;

            float difficultyIncrease = 5;
            currentDifficulty += difficultyIncrease;

            leftWaveSpawner = null;
            rightWaveSpawner = null;

            OnNextWave?.Invoke(currentWave);
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

        public void PlayCurrentWave() {
            HidePreviews();

            float currentInterval = spawnIntervals[currentWave - 1];
            leftWaveSpawner = new WaveSpawner(this, leftSideEnemyAmounts, currentInterval, leftSpawnPoint.position, false);
            rightWaveSpawner = new WaveSpawner(this, rightSideEnemyAmounts, currentInterval, rightSpawnPoint.position, true);

            OnStartWave?.Invoke(currentWave);
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

        private bool AnyEnemiesAlive() {
            foreach(Transform enemy in Containers.Instance.Enemies) {
                if (enemy.gameObject.activeSelf) {
                    return true;
                }
            }
            return false;
        }
    }
}