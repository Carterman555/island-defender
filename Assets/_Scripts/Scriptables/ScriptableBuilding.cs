using UnityEngine;

namespace IslandDefender {

	[CreateAssetMenu(fileName = "Building", menuName = "Building")]
	public class ScriptableBuilding : ScriptableObject {

		[SerializeField] private BuildingType buildingType;
		public BuildingType BuildingType => buildingType;

        [Header("Prefabs")]
        [SerializeField] private GameObject prefab;
		public GameObject Prefab => prefab;

		[SerializeField] private GameObject placeVisualPrefab;
        public GameObject PlaceVisualPrefab => placeVisualPrefab;

		[SerializeField] private Vector2 buildOffset;
		public Vector2 BuildOffset => buildOffset;

        [Header ("Cost")]
		[SerializeField] private int woodCost;
		public int WoodCost => woodCost;

        [SerializeField] private int fiberCost;
        public int FiberCost => fiberCost;

        [SerializeField] private int stoneCost;
        public int StoneCost => stoneCost;
    }

	public enum BuildingType {
		SmallWall = 0,
		Wall = 1,
		Spikes = 2,
		BearTrap = 3,
		BasicTower = 4,
		Garden = 5
	}
}