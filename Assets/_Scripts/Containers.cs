using UnityEngine;

namespace IslandDefender.Environment {
	public class Containers : StaticInstance<Containers> {

		[field: SerializeField] public Transform WaveEnemies { get; private set; }
		[field: SerializeField] public Transform EnvironmentEnemies { get; private set; }
		[field: SerializeField] public Transform Resources { get; private set; }
		[field: SerializeField] public Transform Projectiles { get; private set; }
		[field: SerializeField] public Transform Buildings { get; private set; }
		[field: SerializeField] public Transform Effects { get; private set; }
	}
}