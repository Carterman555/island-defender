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
        }
    }
}