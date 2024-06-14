using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace IslandDefender {
	public class EnemyPreview : MonoBehaviour {

		[SerializeField] private EnemyType enemyType;
        [SerializeField] private TextMeshProUGUI text;

		public void UpdateText(Dictionary<EnemyType, int> enemyAmounts) {
			text.text = enemyAmounts[enemyType].ToString();
		}
	}
}