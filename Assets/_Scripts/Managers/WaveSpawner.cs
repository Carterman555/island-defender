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
        private List<EnemyType> availableEnemies;
        private float waveDifficulty;

        public WaveSpawner(EnemyWaveManager enemyWaveManager, List<EnemyType> availableEnemies, float difficulty) {
            this.enemyWaveManager = enemyWaveManager;
            this.availableEnemies = availableEnemies;
            this.waveDifficulty = difficulty;

            enemyWaveManager.StartCoroutine(SpawnWave());
        }

        private IEnumerator SpawnWave() {

            float difficultyValueRemaining = waveDifficulty;

            while (difficultyValueRemaining > 0) {

                EnemyType enemyToSpawn = ChoseRandomEnemy();
                SpawnEnemy(enemyToSpawn);

                int strengthOfSpawned = ResourceSystem.Instance.GetEnemy(enemyToSpawn).Strength;
                difficultyValueRemaining -= strengthOfSpawned;

                float interval = GetSpawnInterval(difficultyValueRemaining);

                yield return new WaitForSeconds(interval);
            }

            enemyWaveManager.StartCoroutine(enemyWaveManager.FinishedSpawningWave());
        }

        // each enemy has a weight, the higher the weight the most likely an enemy is to get picked.
        // if deciding between two where one has weight of 80 and the other of 20, the one with 80 has
        // a 80% chance of getting picked
        private EnemyType ChoseRandomEnemy() {

            float totalWeight = availableEnemies.Sum(enemyType => ResourceSystem.Instance.GetEnemy(enemyType).SpawnWeight);

            float randomValue = UnityEngine.Random.Range(0, totalWeight);
            float cumulativeWeight = 0;

            foreach (EnemyType enemyType in availableEnemies) {
                float weight = ResourceSystem.Instance.GetEnemy(enemyType).SpawnWeight;
                cumulativeWeight += weight;

                if (randomValue <= cumulativeWeight) {
                    return enemyType;
                }
            }

            // Should never reach here if weights are properly assigned
            throw new InvalidOperationException("Failed to pick an enemy based on weights.");
        }

        // probably need resource system
        private void SpawnEnemy(EnemyType enemyType) {
            GameObject prefab = ResourceSystem.Instance.GetEnemy(enemyType).Prefab;
            ObjectPoolManager.SpawnObject(prefab, enemyWaveManager.GetRandomSpawnPos(), Quaternion.identity, Containers.Instance.Enemies);
        }

        private float GetSpawnInterval(float difficultyValueRemaining) {

            //... value between 0 and 1 depending on how much of the wave is completed
            float waveProgress = GetWaveProgress(difficultyValueRemaining);

            //... the later in the wave, the higher the waveProgressDifficulty and the more enemies spawn
            float waveProgressDifficultyMult = GetMultDifficultyFromProgress(waveProgress);

            float avgInterval = GetAvgInterval(waveDifficulty * waveProgressDifficultyMult); // techinically not exactly avg, but close
            float intervalVariance = 0.3f;
            float interval = UnityEngine.Random.Range(avgInterval * (1 - intervalVariance), avgInterval * (1 + intervalVariance));
            return interval;
        }

        private float GetMultDifficultyFromProgress(float progress) {
            return (progress * 0.5f) + 0.5f;
        }

        // the higher the difficulty, the lower the interval - Desmos: y=\frac{100}{x+10}
        private float GetAvgInterval(float difficulty) {
            return 100 / (difficulty + 10);
        }

        // value between 0 and 1 depending on how much of the wave is completed
        public float GetWaveProgress(float difficultyValueRemaining) {
            return Mathf.InverseLerp(waveDifficulty, 0, difficultyValueRemaining); ;
        }
    }
}
