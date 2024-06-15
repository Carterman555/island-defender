using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IslandDefender.UI {
	public class StartWaveButton : GameButton {

        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI text;

        protected override void OnEnable() {
            base.OnEnable();
            EnemyWaveManager.OnNextWave += Show;
            EnemyWaveManager.OnStartWave += Hide;
        }
        protected override void OnDisable() {
            base.OnDisable();
            EnemyWaveManager.OnNextWave -= Show;
            EnemyWaveManager.OnStartWave -= Hide;
        }

        private void Show(int n) {
            image.enabled = true;
            text.enabled = true;
        }
        private void Hide(int n) {
            image.enabled = false;
            text.enabled = false;
        }

        protected override void OnClicked() {
            base.OnClicked();
            EnemyWaveManager.Instance.PlayCurrentWave();
        }
    }
}