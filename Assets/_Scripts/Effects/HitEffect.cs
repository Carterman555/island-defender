using IslandDefender.Environment;
using IslandDefender.Management;
using IslandDefender.Systems;
using UnityEngine;

namespace IslandDefender {
	public class HitEffect : MonoBehaviour {

		public static HitEffect Create(Vector3 position) {
			return ObjectPoolManager.SpawnObject(AssetSystem.Instance.HitEffect, position, Quaternion.identity, Containers.Instance.Effects);
		}

		// played by anim
		private void Die() {
			ObjectPoolManager.ReturnObjectToPool(gameObject);
		}
	}
}