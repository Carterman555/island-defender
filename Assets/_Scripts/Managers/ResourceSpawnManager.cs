using IslandDefender.Environment;
using IslandDefender.Management;
using IslandDefender.Utilities;
using System.Collections;
using UnityEngine;

namespace IslandDefender {
    public class ResourceSpawnManager : StaticInstance<ResourceSpawnManager> {

        [SerializeField] private float minX = -100f;
        [SerializeField] private float maxX = 100f;

        [SerializeField] private GameObject treePrefab;
        [SerializeField] private GameObject plantPrefab;
        [SerializeField] private GameObject rockPrefab;

        [SerializeField] private RandomFloat treeSpawnInterval;
        [SerializeField] private RandomFloat plantSpawnInterval;
        [SerializeField] private RandomFloat rockSpawnInterval;

        [SerializeField] private int startingTreeAmount;
        [SerializeField] private int startingPlantAmount;
        [SerializeField] private int startingRockAmount;

        private int gardenAmount;

        public void AddGarden() {
            gardenAmount++;
        }

        public void RemoveGarden() {
            gardenAmount--;
        }

        private float treeY = 1.75f;
        private float plantY = -1.5f;
        private float rockY = -1.45f;

        private void Start() {
            StartCoroutine(SpawnResource(treePrefab, treeSpawnInterval, treeY));
            StartCoroutine(SpawnResource(plantPrefab, plantSpawnInterval, plantY));
            StartCoroutine(SpawnResource(rockPrefab, rockSpawnInterval, rockY));

            SpawnStartingResources();
        }

        private void SpawnStartingResources() {
            SpawnOneType(treePrefab, startingTreeAmount, treeY);
            SpawnOneType(plantPrefab, startingPlantAmount, plantY);
            SpawnOneType(rockPrefab, startingRockAmount, rockY);
        }

        private IEnumerator SpawnResource(GameObject prefab, RandomFloat interval, float yPos) {
            while (true) {
                interval.Randomize();
                float newInterval = ApplyGardenBoost(interval.Value, gardenAmount);
                yield return new WaitForSeconds(ApplyGardenBoost(newInterval, gardenAmount));

                SpawnOneType(prefab, 1, yPos);
            }
        }

        private float ApplyGardenBoost(float interval, int gardenAmount) {
            // the closer to 1, the less gardens have effect
            float decayFactor = 0.9f; 

            // Calculate the new interval using exponential decay
            float newInterval = interval * Mathf.Pow(decayFactor, gardenAmount);

            // Ensure the interval doesn't go below a certain minimum threshold
            float minimumInterval = 8f; // This is the minimum time interval in seconds
            if (newInterval < minimumInterval) {
                newInterval = minimumInterval;
            }

            return newInterval;
        }

        private void SpawnOneType(GameObject prefab, int amount, float yPos) {
            for (int i = 0; i < amount; i++) {
                Vector2 pos = new Vector2(Random.Range(minX, maxX), yPos);
                ObjectPoolManager.SpawnObject(prefab, pos, Quaternion.identity, Containers.Instance.Resources);
            }
        }

	}
}