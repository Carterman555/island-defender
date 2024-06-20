using IslandDefender.Environment;
using IslandDefender.Management;
using IslandDefender.Utilities;
using UnityEngine;

namespace IslandDefender {
    public class SpawnFlyTrapSapling : MonoBehaviour {

        [SerializeField] private FlyTrapSapling flyTrapSaplingPrefab;

        [SerializeField] private RandomFloat spawnTime;
        private float spawnTimer;

        [SerializeField] private float obstacleDetectionDistance;
        [SerializeField] private RandomFloat spawnDistance;
        [SerializeField] private float ySpawnOffset;

        [SerializeField] private LayerMask obstacleLayerMask;

        private Enemy enemy;

        private void Awake() {
            enemy = GetComponent<Enemy>();
        }

        private void OnEnable() {
            spawnTime.Randomize();
            spawnTimer = 0;
        }

        private void Update() {
            spawnTimer += Time.deltaTime;
            if (spawnTimer > spawnTime.Value) {
                spawnTime.Randomize();
                spawnTimer = 0;

                if (IsSpawnClear(out int direction)) {
                    SpawnSapling(direction);
                }
            }
        }

        private void SpawnSapling(int direction) {
            Vector3 spawnPos = new Vector3(transform.position.x + (spawnDistance.Randomize() * direction), transform.position.y + ySpawnOffset);
            ObjectPoolManager.SpawnObject(flyTrapSaplingPrefab, spawnPos, Quaternion.identity, Containers.Instance.Enemies);
        }

        private bool IsSpawnClear(out int direction) {

            float height = 4;
            float offset = 2f;
            Vector2 leftOrigin = new Vector2(transform.position.x - offset, transform.position.y);
            Vector2 rightOrigin = new Vector2(transform.position.x + offset, transform.position.y);
            RaycastHit2D rightHit = Physics2D.BoxCast(leftOrigin, new Vector2(obstacleDetectionDistance, height), 0, Vector2.right, obstacleDetectionDistance, obstacleLayerMask);
            RaycastHit2D leftHit = Physics2D.BoxCast(rightOrigin, new Vector2(obstacleDetectionDistance, height), 0, Vector2.left, obstacleDetectionDistance, obstacleLayerMask);

            bool leftClear = leftHit.collider == null;
            bool rightClear = rightHit.collider == null;

            print("Tried spawn: " + leftClear + ", " + rightClear);

            if (!leftClear) {
                print(leftHit.collider.name);
            }

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

        private void OnDrawGizmos() {
            // Draw a box in the Scene view for visualization

            float height = 4;
            float offset = 2f;
            Vector3 leftOrigin = new Vector2(transform.position.x - offset, transform.position.y);
            Vector3 rightOrigin = new Vector2(transform.position.x + offset, transform.position.y);
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(leftOrigin + Vector3.left * obstacleDetectionDistance / 2, new Vector3(obstacleDetectionDistance, height, 0));
            Gizmos.DrawWireCube(rightOrigin + Vector3.right * obstacleDetectionDistance / 2, new Vector3(obstacleDetectionDistance, height, 0));
        }
    }
}