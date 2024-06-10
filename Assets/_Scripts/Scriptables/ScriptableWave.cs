using System.Collections.Generic;
using UnityEngine;

namespace IslandDefender {

	[CreateAssetMenu(fileName = "Wave", menuName = "Wave")]
	public class ScriptableWave : ScriptableObject {

		[SerializeField] private float difficulty;
		public float Difficulty => difficulty;

		[SerializeField] private Dictionary<EnemyType, bool> enemiesInWave = new Dictionary<EnemyType, bool>() {
			{ EnemyType.Frog, true },
		};
		public Dictionary<EnemyType, bool> EnemiesInWave => enemiesInWave;

    }
}