using IslandDefender.Environment;
using IslandDefender.Management;
using IslandDefender.Utilities;
using UnityEngine;

namespace IslandDefender {
    public class ResourceSpawnManager : StaticInstance<ResourceSpawnManager> {

        [SerializeField] private float minX = -100f;
        [SerializeField] private float maxX = 100f;

        [SerializeField] private GameObject treePrefab;
        [SerializeField] private GameObject plantPrefab;
        [SerializeField] private GameObject rockPrefab;

        [SerializeField] private RandomInt treeAmount;
        [SerializeField] private RandomInt plantAmount;
        [SerializeField] private RandomInt rockAmount;

        private int gardenAmount;

        public void AddGarden() {
            gardenAmount++;
        }

        public void RemoveGarden() {
            gardenAmount--;
        }

        private void Start() {
            SpawnResources(-1);
        }

        private void OnEnable() {
            EnemyWaveManager.OnNextWave += SpawnResources;
        }
        private void OnDisable() {
            EnemyWaveManager.OnNextWave -= SpawnResources;
        }

        private void SpawnResources(int n) {

            print("Garden amount: " + gardenAmount);

            float treeY = 2.77f;
            SpawnOneType(treePrefab, ApplyGardenBoost(treeAmount.Randomize()), treeY);

            float plantY = -1.5f;
            SpawnOneType(plantPrefab, ApplyGardenBoost(plantAmount.Randomize()), plantY);

            float rockY = -1.45f;
            SpawnOneType(rockPrefab, ApplyGardenBoost(rockAmount.Randomize()), rockY);
        }

        //private int ApplyGardenBoost(int amount) {
        //    float gardenSpawnBoost = 0.3f;
        //    return Mathf.RoundToInt(amount * (1 + (gardenAmount * gardenSpawnBoost)));
        //}

        private int ApplyGardenBoost(int amount) {

            if (Random.value > 0.5f) {
                return amount + gardenAmount;
            }
            else {
                return amount;
            }
        }

        private void SpawnOneType(GameObject prefab, int amount, float yPos) {
            for (int i = 0; i < amount; i++) {
                Vector2 pos = new Vector2(Random.Range(minX, maxX), yPos);
                ObjectPoolManager.SpawnObject(prefab, pos, Quaternion.identity, Containers.Instance.Resources);
            }
        }

	}
}