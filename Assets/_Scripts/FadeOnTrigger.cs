using DG.Tweening;
using System;
using UnityEngine;

namespace IslandDefender {
	public class FadeOnTrigger : MonoBehaviour {

		[SerializeField] private TriggerContactTracker contactTracker;

		private SpriteRenderer spriteRenderer;

        private void Awake() {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void OnEnable() {
            contactTracker.OnEnterContact += Fade;
            contactTracker.OnLeaveContact += Unfade;
        }
        private void OnDisable() {
            contactTracker.OnEnterContact -= Fade;
            contactTracker.OnLeaveContact -= Unfade;
        }

        private void Fade(GameObject @object) {
            float fadeValue = 0.5f;
            float duration = 0.3f;
            spriteRenderer.DOFade(fadeValue, duration);
        }

        private void Unfade(GameObject @object) {
            float duration = 0.3f;
            spriteRenderer.DOFade(1, duration);
        }
    }
}