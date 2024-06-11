using IslandDefender;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace IslandDefender.Management {

    [RequireComponent(typeof(TriggerContactTracker))]
    public class TouchDamage : MonoBehaviour {
        [SerializeField] private int damage = 5;
        [SerializeField] private float damageCooldown = 1f; // Damage interval in seconds

        private TriggerContactTracker tracker;
        private Dictionary<GameObject, Coroutine> activeCoroutines = new Dictionary<GameObject, Coroutine>();

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
            if (!activeCoroutines.ContainsKey(target)) {
                Coroutine coroutine = StartCoroutine(DamageOverTime(target));
                activeCoroutines[target] = coroutine;
            }
        }

        private void HandleLeaveContact(GameObject target) {
            if (activeCoroutines.TryGetValue(target, out Coroutine coroutine)) {
                StopCoroutine(coroutine);
                activeCoroutines.Remove(target);
            }
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
