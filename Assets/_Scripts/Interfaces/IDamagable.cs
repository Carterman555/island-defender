using System;
using UnityEngine;

namespace IslandDefender {

	public interface IDamagable {

		public event Action<Vector3> OnKnockbackDamaged;
		public event Action<float> OnDamaged;

		public static event Action<GameObject> OnAnyDespawn;
        public event Action OnDeath;

        void Damage(float damage);
		void KnockbackDamage(float damage, Vector3 attackerPosition);
	}
}