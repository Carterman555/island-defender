using IslandDefender.Units;
using UnityEngine;

namespace IslandDefender {

	[CreateAssetMenu(fileName = "NewEnemy", menuName = "Units/Enemy")]
	public class ScriptableEnemy : ScriptableUnit {

        [SerializeField] private EnemyType enemyType;
        public EnemyType EnemyType => enemyType;

        [SerializeField] private int startingWave;
        public int StartingWave => startingWave;

        [SerializeField] private int strength;
        public int Strength => strength;

        [SerializeField] private int spawnWeight;
        public int SpawnWeight => spawnWeight;
    }

    public enum EnemyType {
        Frog,
        //Snake,
        //Snail,
    }
}