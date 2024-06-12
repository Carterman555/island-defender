using IslandDefender.Management;
using System;
using System.Collections;
using UnityEngine;

namespace IslandDefender {
    public class Health : MonoBehaviour, IDamagable {
        public event Action<Vector3> OnDamaged;

        protected float health;
        private bool dead;

        [SerializeField] private Animator anim;

        private IAnimationTrigger animationTrigger;

        protected virtual void Awake() {
            if (anim != null) {
                animationTrigger = anim.GetComponent<IAnimationTrigger>();
            }
        }

        private void OnEnable() {
            if (anim != null) {
                animationTrigger.OnAnimationTriggered += OnAnimationTriggered;
            }

            ResetValues();
        }
        private void OnDisable() {
            if (anim != null) {
                animationTrigger.OnAnimationTriggered -= OnAnimationTriggered;
            }
        }

        protected virtual void ResetValues() {
            dead = false;
        }

        public virtual void Damage(float damage, Vector3 attackerPosition) {

            if (dead) {
                return;
            }

            health -= damage;

            OnDamaged?.Invoke(attackerPosition);

            if (health <= 0) {
                DieAnimation();
                dead = true;
            }
            else if (anim != null) {
                anim.SetTrigger("hurt");
            }
        }

        private void DieAnimation() {
            if (anim != null) {
                anim.SetTrigger("die");
            }
            else {
                Die();
            }
        }

        private void OnAnimationTriggered(AnimationTriggerType animationTriggerType) {
            if (animationTriggerType == AnimationTriggerType.Die) {
                Die();
            }
        }

        protected virtual void Die() {
            ObjectPoolManager.ReturnObjectToPool(gameObject);
        }
    }
}
