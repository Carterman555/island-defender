using IslandDefender.Environment;
using IslandDefender.Management;
using IslandDefender.Utilities;
using UnityEngine;

namespace IslandDefender {
    public class SpawnFlyTrapSapling : MonoBehaviour {

        [SerializeField] private TriggerContactTracker leftObstacleContactTracker;
        [SerializeField] private TriggerContactTracker rightObstacleContactTracker;

        [SerializeField] private FlyTrapSapling flyTrapSaplingPrefab;

        [SerializeField] private RandomFloat spawnTime;
        private float spawnTimer;

        [SerializeField] private RandomFloat spawnDistance;
        [SerializeField] private float ySpawnOffset;

        private Enemy enemy;

        private TriggerContactTracker GetLeftContactTracker() {
            TriggerContactTracker leftTracker = !enemy.IsFacingRight ? rightObstacleContactTracker : leftObstacleContactTracker;
            return leftTracker;
        }

        private TriggerContactTracker GetRightContactTracker() {
            TriggerContactTracker rightTracker = enemy.IsFacingRight ? rightObstacleContactTracker : leftObstacleContactTracker;
            return rightTracker;
        }


        private void Awake() {
            enemy = GetComponent<Enemy>();
        }

        private void OnEnable() {
            spawnTime.Randomize();
            spawnTimer = 0;
        }

        private int left, right;

        private void Update() {
            spawnTimer += Time.deltaTime;
            if (spawnTimer > spawnTime.Value) {
                spawnTime.Randomize();
                spawnTimer = 0;

                if (IsSpawnClear(out int direction)) {
                    SpawnSapling(direction);
                }
            }

            left = GetLeftContactTracker().GetContacts().Count;
            right = GetRightContactTracker().GetContacts().Count;
        }

        private void SpawnSapling(int direction) {
            Vector3 spawnPos = new Vector3(transform.position.x + (spawnDistance.Randomize() * direction), transform.position.y + ySpawnOffset);
            ObjectPoolManager.SpawnObject(flyTrapSaplingPrefab, spawnPos, Quaternion.identity, Containers.Instance.Enemies);
        }

        private bool IsSpawnClear(out int direction) {
            bool leftClear = GetLeftContactTracker().GetContacts().Count <= 1;
            bool rightClear = GetRightContactTracker().GetContacts().Count <= 1;

            print("Tried spawn: " + leftClear + ", " + rightClear);

            if (leftClear && rightClear) {
                direction = Random.value > 0.5f ? 1 : -1;
            }
            else if (leftClear) {
                direction = -1;
            }
            else if (rightClear) {
                direction = 1;
            }
            else {
                direction = 0;
            }

            return leftClear || rightClear;
        }
    }
}