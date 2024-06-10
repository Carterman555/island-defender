using UnityEngine;

namespace IslandDefender {
    public interface ITriggerCheckable {
        GameObject ObjectAggroed { get; set; }
        GameObject ObjectWithinStrikingDistance { get; set; }

        void SetAggroedObject(GameObject objectAggroed);
        void SetStrikingDistanceObject(GameObject objectWithinStrikingDistance);
    }
}