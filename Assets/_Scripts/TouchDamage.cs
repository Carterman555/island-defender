using System.Collections;
using UnityEngine;

namespace IslandDefender {
    [RequireComponent(typeof(TriggerContactTracker))]
    public class TouchDamageContinuous : MonoBehaviour {

        [SerializeField] private int damage = 5;
        [SerializeField] private float damageCooldown = 1f; // Damage interval in seconds

        private TriggerContactTracker tracker;

        private void Awake() {
            tracker = GetComponent<TriggerContactTracker>();
        }

        private void OnEnable() {
            tracker.OnEnterContact += HandleEnterContact;
            tracker.OnLeaveContact += HandleLeaveContact;
        }
        private void OnDisable() {
            tracker.OnEnterContact -= HandleEnterContact;
            tracker.OnLeaveContact -= HandleLeaveContact;
        }

        private void HandleEnterContact(GameObject target) {
            StartCoroutine(DamageOverTime(target));
        }

        private void HandleLeaveContact(GameObject target) {
            StopCoroutine(DamageOverTime(target));

        }

        private IEnumerator DamageOverTime(GameObject target) {
            while (true) {
                if (target.TryGetComponent(out IDamagable damagable)) {
                    damagable.Damage(damage, transform.position);
                }
                yield return new WaitForSeconds(damageCooldown);
            }
        }
    }
}
