using IslandDefender.Units;
using UnityEngine;

namespace IslandDefender {

	[CreateAssetMenu(fileName = "NewAlly", menuName = "Units/Ally")]
	public class ScriptableAlly : ScriptableUnit {

        [SerializeField] private AllyType allyType;
        public AllyType AllyType => allyType;
    }

    public enum AllyType {
        Player,
    }
}