using UnityEngine;

namespace IslandDefender {
	public class InvinvibleNearAlly : MonoBehaviour {

		[SerializeField] private TriggerContactTracker allyContactTracker;

        private UnitHealth health;

        private void Awake() {
            health = GetComponent<UnitHealth>();
        }

        private void Update() {
            bool alliesNearby = allyContactTracker.GetContacts().Count > 0;
            health.SetInvincible(alliesNearby);
        }
    }
}