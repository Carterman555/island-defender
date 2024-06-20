using IslandDefender.Environment;
using IslandDefender.Management;
using IslandDefender.Utilities;
using System;
using UnityEngine;

namespace IslandDefender {
	public class RoamingEnemyManager : StaticInstance<RoamingEnemyManager> {

		private bool dayTime = true;

        private void OnEnable() {
            DayNightCycle.OnDayTime += TurnDayTime;
            DayNightCycle.OnNightTime += TurnNightTime;

            flyTrapSpawnInterval.Randomize();
        }
        private void OnDisable() {
            DayNightCycle.OnDayTime -= TurnDayTime;
            DayNightCycle.OnNightTime -= TurnNightTime;
        }

        private void TurnDayTime() {
            dayTime = true;
        }
        private void TurnNightTime() {
            dayTime = false;
        }

        [SerializeField] private Transform roamBorderPoint1, roamBorderPoint2, roamBorderPoint3, roamBorderPoint4;

        [SerializeField] private RandomFloat flyTrapSpawnInterval;

        [SerializeField] private GameObject flyTrapPrefab;

        private float flyTrapTimer;

        private void Update() {

            flyTrapTimer += Time.deltaTime;

            if (flyTrapTimer > flyTrapSpawnInterval.Value) {
                flyTrapTimer = 0;
                flyTrapSpawnInterval.Randomize();

                float flyTrapY = -2.5f;
                SpawnEnemy(flyTrapY, true);
            }
        }

        public void SpawnFlyTrapInWave() {
            float flyTrapY = -2.5f;
            SpawnEnemy(flyTrapY, false);
        }

        private void SpawnEnemy(float yPos, bool awayFromKeep = true) {

            float spawnXPos;

            // between point 1 and 2 or  between 3 and 4
            if (awayFromKeep) {
                if (UnityEngine.Random.value > 0.5f) {
                    spawnXPos = UnityEngine.Random.Range(roamBorderPoint1.position.x, roamBorderPoint2.position.x);
                }
                else {
                    spawnXPos = UnityEngine.Random.Range(roamBorderPoint3.position.x, roamBorderPoint4.position.x);
                }
            }
            // between 2 and 3 (near keep)
            else {
                spawnXPos = UnityEngine.Random.Range(roamBorderPoint2.position.x, roamBorderPoint3.position.x);
            }

            GameObject enemy = ObjectPoolManager.SpawnObject(flyTrapPrefab, new Vector3(spawnXPos, yPos), Quaternion.identity, Containers.Instance.Enemies);
        }
    }
}