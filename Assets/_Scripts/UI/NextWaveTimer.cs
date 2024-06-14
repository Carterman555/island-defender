using TMPro;
using UnityEngine;

namespace IslandDefender {
    public class NextWaveTimer : MonoBehaviour {

        private TextMeshProUGUI text;

        private void Awake() {
            text = GetComponent<TextMeshProUGUI>();
        }

        private void Update() {
            if (EnemyWaveManager.Instance.IsDayTime(out float timeLeft)) {
                text.text = "Time Until Next Wave: " + ((int)timeLeft).ToString();
            }
            else {
                text.text = "";
            }
        }
    }
}