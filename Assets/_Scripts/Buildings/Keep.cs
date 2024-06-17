using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IslandDefender.Environment.Building {
    public class Keep : Health {

        [SerializeField] private TriggerContactTracker playerContactTracker;

        [SerializeField] private float maxHealth;
        private int level = 1;

        private bool playerTouching;

        protected override void ResetValues() {
            base.ResetValues();
            health = maxHealth;
        }

        protected override void OnEnable() {
            base.OnEnable();
            playerContactTracker.OnEnterContact += PlayerEnter;
            playerContactTracker.OnLeaveContact += PlayerExit;
        }

        protected override void OnDisable() {
            base.OnDisable();
            playerContactTracker.OnEnterContact -= PlayerEnter;
            playerContactTracker.OnLeaveContact -= PlayerExit;

        }

        private void PlayerEnter(GameObject player) {
            playerTouching = true;

            Vector2 offset = new Vector2(0, 3f);
            string text = "Press R to Upgrade Keep";
            WorldPopupText.Instance.ShowText((Vector2)transform.position + offset, text);
        }

        private void PlayerExit(GameObject player) {
            playerTouching = false;
            WorldPopupText.Instance.HideText();
        }

        private void Update() {
            if (playerTouching && Input.GetKeyDown(KeyCode.R)) {
                LevelUp();
            }
        }

        private void LevelUp() {
            level++;
        }

        protected override void Die() {
            SceneManager.LoadScene("Game");
        }
    }
}
