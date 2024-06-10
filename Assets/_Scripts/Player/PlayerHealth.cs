using UnityEngine;
using UnityEngine.SceneManagement;

namespace IslandDefender {
	public class PlayerHealth : Health {

        protected override void Die() {
            SceneManager.LoadScene("Game");
        }
    }
}