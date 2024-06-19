using IslandDefender.Management;
using IslandDefender.UI;
using System.Collections;
using UnityEngine;

namespace IslandDefender {
	public class PlayButton : GameButton {

        protected override void OnClicked() {
            base.OnClicked();
            StartCoroutine(PlayGame());
        }

        private IEnumerator PlayGame() {

            FadePanel.Instance.FadeOut();

            float fadeDuration = 1.5f;
            yield return new WaitForSeconds(fadeDuration);

            GameManager.Instance.StartGame();
        }
    }
}