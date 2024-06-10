using IslandDefender.Units;
using UnityEngine;

namespace IslandDefender {
    public class UnitBase : MonoBehaviour {

        [SerializeField] private ScriptableUnit scriptableUnit;

        public virtual Stats Stats => scriptableUnit.BaseStats;
        public Faction Faction => scriptableUnit.Faction;
    }
}
