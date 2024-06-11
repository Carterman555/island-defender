using UnityEngine;

namespace IslandDefender.Environment {
	public class Containers : StaticInstance<Containers> {

		[field: SerializeField] public Transform Enemies { get; private set; }
		[field: SerializeField] public Transform Resources { get; private set; }
		[field: SerializeField] public Transform Projectiles { get; private set; }
	}
}