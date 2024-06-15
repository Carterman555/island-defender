using IslandDefender.Environment;
using IslandDefender.Management;
using UnityEngine;

namespace IslandDefender {
	public class ResourceSpawnManager : MonoBehaviour {

		[SerializeField] private GameObject treePrefab;
		[SerializeField] private GameObject plantPrefab;
		[SerializeField] private GameObject rockPrefab;

		[SerializeField] private int treeAmount;
		[SerializeField] private int plantAmount;
		[SerializeField] private int rockAmount;

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
			float treeY = 2.77f;
			SpawnOneType(treePrefab, treeAmount, treeY);

            float plantY = -1.5f;
            SpawnOneType(plantPrefab, plantAmount, plantY);

            float rockY = -1.45f;
            SpawnOneType(rockPrefab, rockAmount, rockY);
        }

		private void SpawnOneType(GameObject prefab, int amount, float yPos) {
            float minX = -30f;
            float maxX = 30f;
            for (int i = 0; i < amount; i++) {
                Vector2 pos = new Vector2(Random.Range(minX, maxX), yPos);
                ObjectPoolManager.SpawnObject(prefab, pos, Quaternion.identity, Containers.Instance.Resources);
            }
        }

	}
}