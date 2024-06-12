using IslandDefender.Management;
using System;
using UnityEngine;

namespace IslandDefender {

    [RequireComponent(typeof(UnitBase))]
    public class UnitHealth : MonoBehaviour, IDamagable {

        public event Action<Vector3> OnDamaged;

        private float health;
        private bool dead;

        [SerializeField] private Animator anim;

        private IAnimationTrigger animationTrigger;

        protected virtual void Awake() {
            health = GetComponent<UnitBase>().Stats.Health;

            if (anim != null) {
                animationTrigger = anim.GetComponent<IAnimationTrigger>();
            }
        }

        private void OnEnable() {
            if (anim != null) {
                animationTrigger.OnAnimationTriggered += OnAnimationTriggered;
            }
        }
        private void OnDisable() {
            if (anim != null) {
                animationTrigger.OnAnimationTriggered -= OnAnimationTriggered;
            }
        }

        public void Damage(float damage, Vector3 attackerPosition) {

            if (dead) {
                return;
            }

            health -= damage;

            if (anim != null) {
                anim.SetTrigger("hurt");
            }

            if (health <= 0) {
                DieAnimation();
                dead = true;
            }

            OnDamaged?.Invoke(attackerPosition);
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

        // gets played by die animation
        protected virtual void Die() {
            ObjectPoolManager.ReturnObjectToPool(gameObject);
        }
    }
}
