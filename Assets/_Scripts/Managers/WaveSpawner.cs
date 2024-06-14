using IslandDefender.Environment;
using IslandDefender.Management;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace IslandDefender {
    public class WaveSpawner {

        private EnemyWaveManager enemyWaveManager;
        private Dictionary<EnemyType, int> enemyAmounts;
        private float waveDifficulty;
        private Vector3 spawnPos;

        private int totalEnemiesInWave;

        public int EnemiesLeft => enemyAmounts.Values.Sum();

        private bool completed;

        public bool Completed() {
            return completed;
        }

        public WaveSpawner(EnemyWaveManager enemyWaveManager, Dictionary<EnemyType, int> enemyAmounts, float difficulty, Vector3 spawnPos) {
            this.enemyWaveManager = enemyWaveManager;
            this.enemyAmounts = enemyAmounts;
            waveDifficulty = difficulty;
            this.spawnPos = spawnPos;

            totalEnemiesInWave = EnemiesLeft;

            completed = false;

            enemyWaveManager.StartCoroutine(SpawnWave());

            //testChooser();
        }

        private IEnumerator SpawnWave() {

            while (EnemiesLeftToSpawn()) {

                EnemyType enemyToSpawn = ChoseRandomEnemy();

                enemyAmounts[enemyToSpawn]--;
                SpawnEnemy(enemyToSpawn);

                float interval = GetSpawnInterval();

                yield return new WaitForSeconds(interval);
            }

            completed = true;
        }

        public void testChooser() {

            Dictionary<EnemyType, int> enemyAmount = new();

            foreach (EnemyType enemyType in Enum.GetValues(typeof(EnemyType))) {
                enemyAmount.Add(enemyType, 0);
            }

            for (int i = 0; i < 1000; i++) {
                enemyAmount[ChoseRandomEnemy()]++;
            }

            foreach (EnemyType enemyType in Enum.GetValues(typeof(EnemyType))) {
                Debug.Log(enemyType + ": " + enemyAmount[enemyType]);
            }
        }

        // each enemy has a weight, the higher the weight the most likely an enemy is to get picked.
        // if deciding between two where one has weight of 80 and the other of 20, the one with 80 has
        // a 80% chance of getting picked
        private EnemyType ChoseRandomEnemy() {

            float totalWeight = EnemiesLeft;

            float randomValue = UnityEngine.Random.Range(0, totalWeight);
            float cumulativeWeight = 0;

            foreach (EnemyType enemyType in Enum.GetValues(typeof(EnemyType))) {
                float weight = enemyAmounts[enemyType];
                cumulativeWeight += weight;

                if (randomValue <= cumulativeWeight) {
                    return enemyType;
                }
            }

            // Should never reach here if weights are properly assigned
            throw new InvalidOperationException("Failed to pick an enemy based on weights.");
        }

        private void SpawnEnemy(EnemyType enemyType) {
            GameObject prefab = ResourceSystem.Instance.GetEnemy(enemyType).Prefab;
            ObjectPoolManager.SpawnObject(prefab, spawnPos, Quaternion.identity, Containers.Instance.Enemies);
        }

        private float GetSpawnInterval() {

            //... value between 0 and 1 depending on how much of the wave is completed
            float waveProgress = GetWaveProgress();

            //... the later in the wave, the higher the waveProgressDifficulty and the more enemies spawn
            float waveProgressDifficultyMult = GetMultDifficultyFromProgress(waveProgress);

            float avgInterval = GetAvgInterval(waveDifficulty * waveProgressDifficultyMult); // techinically not exactly avg, but close
            //float intervalVariance = 0.15f;
            float intervalVariance = 0f;
            float interval = UnityEngine.Random.Range(avgInterval * (1 - intervalVariance), avgInterval * (1 + intervalVariance));
            return interval;
        }

        private bool EnemiesLeftToSpawn() {
            int amountOfEnemies = EnemiesLeft;
            return amountOfEnemies > 0;
        }

        private float GetMultDifficultyFromProgress(float progress) {
            return (progress * 0.5f) + 0.5f;
        }

        // the higher the difficulty, the lower the interval - Desmos: y=\frac{100}{x+10}
        private float GetAvgInterval(float difficulty) {
            return 200 / (difficulty + 10);
        }

        // value between 0 and 1 depending on how much of the wave is completed
        public float GetWaveProgress() {
            return Mathf.InverseLerp(0, totalEnemiesInWave, EnemiesLeft);
        }
    }
}
