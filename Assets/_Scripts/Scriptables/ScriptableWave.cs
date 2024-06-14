using System.Collections.Generic;
using UnityEngine;

namespace IslandDefender {

	[CreateAssetMenu(fileName = "Wave", menuName = "Wave")]
	public class ScriptableWave : ScriptableObject {

		[SerializeField] private float minWave;
		public float MinWave => minWave;

        [Header("Weights")]
		[SerializeField] private float snakeWeight;
		[SerializeField] private float babyFrogWeight;
		[SerializeField] private float snailWeight;
		[SerializeField] private float frogWeight;

		public Dictionary<EnemyType, float> GetEnemyWeights() {
            Dictionary<EnemyType, float> enemyWeights = new() {
                { EnemyType.Snake, snakeWeight },
                { EnemyType.BabyFrog, babyFrogWeight },
                { EnemyType.Snail, snailWeight },
                { EnemyType.Frog, frogWeight },
            };

            return enemyWeights;
        }
    }
}