using IslandDefender;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace IslandDefender.Management {

    [RequireComponent(typeof(TriggerContactTracker))]
    public class TouchDamage : MonoBehaviour {

        public event Action OnDamage;

        [SerializeField] private float damage = 1;
        [SerializeField] private float damageCooldown = 1f; // Damage interval in seconds
        [SerializeField] private bool applyKnockback = true;

        private TriggerContactTracker tracker;
        private Dictionary<GameObject, Coroutine> activeCoroutines = new Dictionary<GameObject, Coroutine>();

        private void Awake() {
            tracker = GetComponent<TriggerContactTracker>();
        }

        private void OnEnable() {
            tracker.OnEnterContact += HandleEnterContact;
            tracker.OnLeaveContact += HandleLeaveContact;

            ResetValues();
        }

        private void OnDisable() {
            tracker.OnEnterContact -= HandleEnterContact;
            tracker.OnLeaveContact -= HandleLeaveContact;
        }

        private void ResetValues() {
            activeCoroutines.Clear();
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

                if (!target.activeSelf) {
                    if (activeCoroutines.TryGetValue(target, out Coroutine coroutine)) {
                        StopCoroutine(coroutine);
                        activeCoroutines.Remove(target);
                    }
                }

                if (target.TryGetComponent(out IDamagable damagable)) {
                    if (applyKnockback) {
                        damagable.KnockbackDamage(damage, transform.position);
                    }
                    else {
                        damagable.Damage(damage);
                    }

                    OnDamage?.Invoke();
                }
                yield return new WaitForSeconds(damageCooldown);
            }
        }
    }
}
