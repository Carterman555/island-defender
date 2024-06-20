using DG.Tweening;
using UnityEngine;
using IslandDefender.Environment.Building;
using System.Collections;

namespace IslandDefender.UI {
	public class BuildingButtonContainer : MonoBehaviour {


        private BuildingButton[] buildingButtons;

        private bool showing;

        private void Awake() {
            buildingButtons = GetComponentsInChildren<BuildingButton>(true);

            Keep.OnUpgrade += TryUnlock;
        }

        private IEnumerator Start() {
            yield return null; // wait for Building Buttons to initialize
            DeactivateAllButtons();
            TryUnlock(1);
        }

        private void DeactivateAllButtons() {
            foreach (var button in buildingButtons) {
                button.gameObject.SetActive(false);
            }
        }

        private void TryUnlock(int keepLevel) {
            buildingButtons = GetComponentsInChildren<BuildingButton>(true); // they become null for some reason, so need to reassign
            foreach (var button in buildingButtons) {
                if (keepLevel >= button.GetLevelToUnlock()) {
                    button.gameObject.SetActive(true);
                }
            }
        }

        private void Update() {

            // to face correct direction when player changes facd
            bool facingRight = PlayerBuild.Instance.transform.eulerAngles.y == 0;
            float direction = facingRight ? 1 : -1;

            if (showing) {
                transform.localScale = new Vector3(direction, 1);
            }

            // show when press q
            if (Input.GetKeyDown(KeyCode.Q)) {
                float duration = 0.3f;
                Vector2 scale = new Vector2(direction, 1);
                transform.DOScale(scale, duration).SetEase(Ease.OutSine).OnComplete(() => {
                    showing = true;
                });
            }

            // hide when release press q
            if (showing && !Input.GetKey(KeyCode.Q)) {
                float duration = 0.3f;
                transform.DOScale(0, duration).SetEase(Ease.OutSine).OnComplete(() => {
                    showing = true;
                });
            }
        }

    }
}