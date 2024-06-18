using System;
using UnityEngine;

namespace IslandDefender {

	public interface IDamagable {

		public event Action<Vector3> OnKnockbackDamaged;
		public event Action OnDamaged;

        public event Action OnDeath;

        void Damage(float damage);
		void KnockbackDamage(float damage, Vector3 attackerPosition);
	}
}