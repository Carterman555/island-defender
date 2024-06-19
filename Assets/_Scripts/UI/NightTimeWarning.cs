using DG.Tweening;
using UnityEngine;

namespace IslandDefender {
	public class NightTimeWarning : MonoBehaviour {

        private void OnEnable() {
			DayNightCycle.OnNightTime += ShowText;
        }
		private void OnDisable() {
            DayNightCycle.OnNightTime -= ShowText;
        }

        private void ShowText() {
			transform.localScale = Vector3.zero;

			float changeScaleDuration = 0.5f;
            float scale = 1.1f;
            transform.DOScale(scale, changeScaleDuration).SetEase(Ease.OutSine).OnComplete(() => {

                float duration = 0.5f;
				int loops = 4;
				transform.DOScale(1, duration).SetEase(Ease.InOutSine).SetLoops(loops, LoopType.Yoyo).OnComplete(() => {
					transform.DOScale(0, changeScaleDuration).SetEase(Ease.InSine);
                });
            });
		}
	}
}