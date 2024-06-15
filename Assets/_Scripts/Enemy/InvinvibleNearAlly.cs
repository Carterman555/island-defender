using UnityEngine;

namespace IslandDefender {

	public class InvinvibleNearAlly : MonoBehaviour {

		[SerializeField] private TriggerContactTracker allyContactTracker;

        private Enemy enemy;
        private UnitHealth health;

        private void Awake() {
            enemy = GetComponent<Enemy>();
            health = GetComponent<UnitHealth>();
        }

        private void Update() {
            bool alliesNearby = allyContactTracker.GetContacts().Count > 0;
            health.SetInvincible(alliesNearby);
            enemy.SetMove(!alliesNearby);
        }
    }
}