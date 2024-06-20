using UnityEngine;

namespace IslandDefender {

	public interface IDirectionProjectile {

		public void Shoot(int direction, float damage);

	}

    public interface IPositionProjectile {

        public void Shoot(Vector2 direction, float damage);

    }
}