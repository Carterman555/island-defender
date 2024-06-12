using UnityEngine;

namespace IslandDefender {
    public interface ITriggerCheckable {
        void SetMidrangeObject(GameObject midrangeObject);
        void SetCloseObject(GameObject closeObject);
    }
}