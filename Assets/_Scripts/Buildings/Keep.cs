using UnityEngine;
using UnityEngine.SceneManagement;

namespace IslandDefender.Environment.Building {
    public class Keep : Health {
        [SerializeField] private float maxHealth;

        protected override void ResetValues() {
            base.ResetValues();
            health = maxHealth;
        }

        protected override void Die() {
            SceneManager.LoadScene("Game");
        }
    }
}
