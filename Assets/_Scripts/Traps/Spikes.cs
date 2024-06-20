using IslandDefender.Management;
using UnityEngine;

namespace IslandDefender {
	public class Spikes : Health {

		[SerializeField] private TouchDamage touchDamage;

        protected override void OnEnable() {
            base.OnEnable();
            touchDamage.OnDamage += WearDown;
        }
        protected override void OnDisable() {
            base.OnDisable();
            touchDamage.OnDamage -= WearDown;
        }

        private void WearDown() {
            Damage(1);
        }
    }
}