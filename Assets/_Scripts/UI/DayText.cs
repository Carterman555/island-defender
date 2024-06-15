using TMPro;
using UnityEngine;

namespace IslandDefender {
	public class DayText : MonoBehaviour {

		private TextMeshProUGUI text;

        private void Awake() {
            text = GetComponent<TextMeshProUGUI>();
        }

        private void OnEnable() {
            EnemyWaveManager.OnNextWave += UpdateText;
        }
        private void OnDisable() {
            EnemyWaveManager.OnNextWave -= UpdateText;
        }

        private void UpdateText(int waveNum) {
            text.text = "Day: " + waveNum;
        }
    }
}