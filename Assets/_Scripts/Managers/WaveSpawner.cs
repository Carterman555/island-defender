using IslandDefender.Environment;
using IslandDefender.Management;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IslandDefender {
    public class WaveSpawner {

        private EnemyWaveManager enemyWaveManager;
        private Dictionary<EnemyType, int> enemyAmounts;
        private float spawnInterval;
        private Vector3 spawnPos;

        private int totalEnemiesInWave;

        public int EnemiesLeft => enemyAmounts.Values.Sum();

        private bool completed;

        public bool Completed() {
            return completed;
        }

        public WaveSpawner(EnemyWaveManager enemyWaveManager, Dictionary<EnemyType, int> enemyAmounts, float spawnInterval, Vector3 spawnPos, bool delaySpawning) {
            this.enemyWaveManager = enemyWaveManager;
            this.enemyAmounts = enemyAmounts;
            this.spawnInterval = spawnInterval;
            this.spawnPos = spawnPos;

            totalEnemiesInWave = EnemiesLeft;

            completed = false;

            enemyWaveManager.StartCoroutine(SpawnWave(delaySpawning));
        }

        private IEnumerator SpawnWave(bool delaySpawning) {

            if (delaySpawning) {
                yield return new WaitForSeconds(spawnInterval * 0.5f);
            }

            while (EnemiesLeftToSpawn()) {

                EnemyType enemyToSpawn = ChoseRandomEnemy();

                float strength = ResourceSystem.Instance.GetEnemy(enemyToSpawn).Strength;
                if (strength < 8) {
                    enemyWaveManager.StartCoroutine(SpawnEnemyGroup(enemyToSpawn, enemyAmounts[enemyToSpawn]));
                }
                else {
                    enemyAmounts[enemyToSpawn]--;
                    SpawnEnemy(enemyToSpawn);
                }

                float interval = GetSpawnInterval();
                yield return new WaitForSeconds(interval);
            }

            completed = true;
        }

        private IEnumerator SpawnEnemyGroup(EnemyType enemyType, int maxAmount) {

            int amount = GetGroupSize();
            amount = Mathf.Min(amount, maxAmount);

            for (int i = 0; i < amount; i++) {

                if (!EnemiesLeftToSpawn()) {
                    break;
                }

                enemyAmounts[enemyType]--;
                SpawnEnemy(enemyType);

                float interval = 0.2f;
                yield return new WaitForSeconds(interval);
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

            float posVariance = 20f;
            Vector2 newSpawnPos = new Vector2(spawnPos.x + UnityEngine.Random.Range(-posVariance, posVariance), spawnPos.y);
            ObjectPoolManager.SpawnObject(prefab, newSpawnPos, Quaternion.identity, Containers.Instance.WaveEnemies);
        }

        private float GetSpawnInterval() {

            //... value between 0 and 1 depending on how much of the wave is completed
            float waveProgress = GetWaveProgress();

            //... the later in the wave, the higher the waveProgressDifficulty and the more enemies spawn
            float waveProgressDifficultyMult = GetMultDifficultyFromProgress(waveProgress);

            float avgInterval = spawnInterval / waveProgressDifficultyMult; // techinically not exactly avg, but close
            float intervalVariance = 0.15f;
            float interval = UnityEngine.Random.Range(avgInterval * (1 - intervalVariance), avgInterval * (1 + intervalVariance));
            return interval;
        }

        private bool EnemiesLeftToSpawn() {
            int amountOfEnemies = EnemiesLeft;
            return amountOfEnemies > 0;
        }

        private float GetMultDifficultyFromProgress(float progress) {
            return (progress * 0.3f) + 0.7f;
        }

        // value between 0 and 1 depending on how much of the wave is completed
        public float GetWaveProgress() {
            return Mathf.InverseLerp(totalEnemiesInWave, 0, EnemiesLeft);
        }

        private int GetGroupSize() {
            int avgGroupSize = Mathf.RoundToInt(1 + (EnemyWaveManager.Instance.GetCurrentWave() * 0.5f));
            int randomSize = UnityEngine.Random.Range(avgGroupSize - 1, avgGroupSize + 2);
            return randomSize;
        }
    }
}
