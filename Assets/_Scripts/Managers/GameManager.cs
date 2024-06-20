using IslandDefender.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IslandDefender.Management {
	public class GameManager : Singleton<GameManager> {

		[SerializeField] private bool playIntroAndTutorial = false;

        public bool PlayIntroAndTutorial() {
            return playIntroAndTutorial;
        }

        public void SetPlayIntroAndTutorial(bool playIntroAndTutorial) {
            this.playIntroAndTutorial = playIntroAndTutorial;
        }

        public void StartGame() {
            SceneManager.LoadScene("Game");
            Time.timeScale = 1f;
        }

        public void GameOver() {
            PopupCanvas.Instance.ActivateUIObject("GameOverPanel");
            Time.timeScale = 0f;
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                GameOver();
            }
        }
    }
}