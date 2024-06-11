using IslandDefender.Management;
using UnityEngine;

namespace IslandDefender {
    public class Health : MonoBehaviour, IDamagable {

        [SerializeField] private Animator anim;
        [SerializeField] private float maxHealth;
        private float health;

        private IAnimationTrigger animationTrigger;

        private bool dead;

        private void Awake() {
            health = maxHealth;
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
