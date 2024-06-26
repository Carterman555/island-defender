using DG.Tweening;
using UnityEngine;
using IslandDefender.Environment.Building;
using System.Collections;
using System;

namespace IslandDefender.UI {
	public class BuildingButtonContainer : MonoBehaviour {

        public static event Action OnNewBuildingUnlocked;

        [SerializeField] private BuildingButton[] buildingButtons = new BuildingButton[6];

        private bool showing;

        private void Awake() {
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
            foreach (var button in buildingButtons) {
                if (keepLevel >= button.GetLevelToUnlock()) {

                    if (!button.gameObject.activeSelf && keepLevel > 1) {
                        OnNewBuildingUnlocked?.Invoke();
                    }

                    button.gameObject.SetActive(true);
                }
            }
        }

        private void Update() {

            Vector3 offset = new Vector3(0, 3.75f);
            transform.position = PlayerBuild.Instance.transform.position + offset;

            // show when press q
            if (Input.GetKeyDown(KeyCode.Q)) {
                float duration = 0.3f;
                transform.DOScale(1, duration).SetEase(Ease.OutSine).OnComplete(() => {
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