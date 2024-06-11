using UnityEngine;

namespace IslandDefender {

	public interface IProjectile {

		public void Shoot(int direction, float damage);

	}
}