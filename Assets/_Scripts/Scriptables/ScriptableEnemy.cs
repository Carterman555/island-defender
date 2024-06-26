using IslandDefender.Units;
using UnityEngine;

namespace IslandDefender {

	[CreateAssetMenu(fileName = "NewEnemy", menuName = "Units/Enemy")]
	public class ScriptableEnemy : ScriptableUnit {

        [SerializeField] private EnemyType enemyType;
        public EnemyType EnemyType => enemyType;

        [SerializeField] private bool spawnInWave = true;
        public bool SpawnInWave => spawnInWave;

        [SerializeField] private int startingWave;
        public int StartingWave => startingWave;

        [SerializeField] private float strength;
        public float Strength => strength;
    }

    [System.Serializable]
    public enum EnemyType {
        Snake = 0,
        BabyFrog = 1,
        Frog = 2,
        Ent = 3,
        FlyTrap = 4,
        BigPlant = 5
    }
}