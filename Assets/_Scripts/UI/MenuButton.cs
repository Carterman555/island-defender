using IslandDefender.Management;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IslandDefender.UI {
	public class MenuButton : GameButton {

        protected override void OnClicked() {
            base.OnClicked();
            StartCoroutine(PlayGame());
        }

        private IEnumerator PlayGame() {

            FadePanel.Instance.FadeOut();

            float fadeDuration = 1.5f;
            yield return new WaitForSecondsRealtime(fadeDuration);

            SceneManager.LoadScene("Menu");
        }
    }
}