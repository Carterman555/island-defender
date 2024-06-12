using UnityEngine;

namespace IslandDefender {
	public class Wall : Health {
        [SerializeField] private float maxHealth;

        protected override void ResetValues() {
            base.ResetValues();
            health = maxHealth;
        }
    }
}