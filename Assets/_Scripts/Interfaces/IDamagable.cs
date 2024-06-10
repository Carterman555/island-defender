using UnityEngine;

namespace IslandDefender {

	public interface IDamagable {
		
		void Damage(float damage, Vector3 attackerPosition);
	}
}