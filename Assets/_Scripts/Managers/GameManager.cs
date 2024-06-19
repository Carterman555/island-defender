using UnityEngine;
using UnityEngine.SceneManagement;

namespace IslandDefender.Management {
	public class GameManager : Singleton<GameManager> {

		private bool playIntroAndTutorial = true;

        public bool PlayIntroAndTutorial() {
            return playIntroAndTutorial;
        }

        public void SetPlayIntroAndTutorial(bool playIntroAndTutorial) {
            this.playIntroAndTutorial = playIntroAndTutorial;
        }

        private void Start() {
            FadePanel.Instance.FadeIn();
        }

        public void StartGame() {
            SceneManager.LoadScene("Game");
        }
    }
}