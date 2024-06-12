using UnityEngine;

namespace IslandDefender {

	[CreateAssetMenu(fileName = "Building", menuName = "Building")]
	public class ScriptableBuilding : ScriptableObject {

		[SerializeField] private GameObject prefab;
		public GameObject Prefab => prefab;

		[SerializeField] private GameObject placeVisualPrefab;
        public GameObject PlaceVisualPrefab => placeVisualPrefab;

        [Header ("Cost")]
		[SerializeField] private int woodCost;
		public int WoodCost => woodCost;

		[SerializeField] private int stoneCost;
        public int StoneCost => stoneCost;

    }
}