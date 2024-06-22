using IslandDefender.Environment.Building;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace IslandDefender {
	public class KeepWarningArrow : MonoBehaviour {

        [SerializeField] private Keep keep;

		[SerializeField] private Image leftArrow;
		[SerializeField] private Image rightArrow;

        [SerializeField] private float showDuration;
        private float showTimer;

        [SerializeField] private float distanceForKeepInView;

        private void OnEnable() {
            keep.OnDamaged += TryShowArrow;
        }
        private void OnDisable() {
            keep.OnDamaged -= TryShowArrow;
        }

        private void TryShowArrow(float obj) {

            float distance = Mathf.Abs(PlayerBuild.Instance.transform.position.x - keep.transform.position.x);

            if (distance > distanceForKeepInView) {

                bool playerLeftOfKeep = PlayerBuild.Instance.transform.position.x < keep.transform.position.x;
                if (playerLeftOfKeep) {
                    rightArrow.enabled = true;
                }
                else {
                    leftArrow.enabled = true;
                }
                showTimer = 0;
            }

            if (rightArrow.enabled || leftArrow.enabled) {
                showTimer += Time.deltaTime;

                if (showTimer > showDuration) {
                    rightArrow.enabled = false;
                    leftArrow.enabled = false;
                    showTimer = 0;
                }
            }
        }

        private void Update() {

            float distance = Mathf.Abs(PlayerBuild.Instance.transform.position.x - keep.transform.position.x);
            if (distance < distanceForKeepInView) {
                rightArrow.enabled = false;
                leftArrow.enabled = false;
            }
        }
    }
}