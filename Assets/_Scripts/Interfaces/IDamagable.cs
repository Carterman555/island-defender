using System;
using UnityEngine;

namespace IslandDefender {

	public interface IDamagable {

		public event Action<Vector3> OnDamaged;

		void Damage(float damage, Vector3 attackerPosition);
	}
}