using IslandDefender.Management;
using System;
using System.Collections;
using UnityEngine;

namespace IslandDefender {
    public class Health : MonoBehaviour, IDamagable {
        public event Action<Vector3> OnKnockbackDamaged;
        public event Action OnDamaged;

        protected float health;
        private bool dead;

        [SerializeField] private Animator anim;

        private IAnimationTrigger animationTrigger;

        // prototyping
        protected SpriteRenderer spriteRenderer;
        private float _maxHealth;
        private void Start() {
            _maxHealth = health;
        }
        // prototyping /\

        protected virtual void Awake() {

            spriteRenderer = GetComponentInChildren<SpriteRenderer>(); // prototyping

            if (anim != null) {
                animationTrigger = anim.GetComponent<IAnimationTrigger>();
            }
        }

        protected virtual void OnEnable() {
            if (anim != null) {
                animationTrigger.OnAnimationTriggered += OnAnimationTriggered;
            }

            ResetValues();
        }
        protected virtual void OnDisable() {
            if (anim != null) {
                animationTrigger.OnAnimationTriggered -= OnAnimationTriggered;
            }
        }

        protected virtual void ResetValues() {
            dead = false;
            spriteRenderer.Fade(1); // prototyping
        }

        public virtual void KnockbackDamage(float damage, Vector3 attackerPosition) {

            if (dead) {
                return;
            }

            Damage(damage);

            OnKnockbackDamaged?.Invoke(attackerPosition);
        }


        public void Damage(float damage) {

            if (dead) {
                return;
            }

            health -= damage;

            spriteRenderer.Fade(Mathf.InverseLerp(0, _maxHealth, health));

            OnDamaged?.Invoke();

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
