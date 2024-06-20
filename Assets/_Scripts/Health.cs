using IslandDefender.Management;
using System;
using UnityEngine;

namespace IslandDefender {
    public class Health : MonoBehaviour, IDamagable {
        public event Action<Vector3> OnKnockbackDamaged;
        public event Action<float> OnDamaged;

        public static event Action<GameObject> OnAnyDeath;

        public event Action OnDeath;

        [SerializeField] private float maxHealth;
        protected float health;
        private bool dead;

        [SerializeField] private Animator anim;

        private IAnimationTrigger animationTrigger;

        protected void SetMaxHealth(float maxHealth) {
            this.maxHealth = maxHealth;
        }

        public float GetMaxHealth() {
            return maxHealth;
        }

        protected virtual void Awake() {
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
            health = maxHealth;
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

            //spriteRenderer.Fade(Mathf.InverseLerp(0, _maxHealth, health));

            OnDamaged?.Invoke(health);
            OnAnyDeath?.Invoke(gameObject);

            if (health <= 0) {
                DieAnimation();
                dead = true;
            }
            else if (anim != null) {
                print("hurt");
                anim.SetTrigger("hurt");
            }
        }

        private void DieAnimation() {

            OnDeath?.Invoke();

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

        public virtual void Die() {
            ObjectPoolManager.ReturnObjectToPool(gameObject);
        }

        
    }
}
