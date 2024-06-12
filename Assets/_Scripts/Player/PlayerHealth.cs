using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IslandDefender.Units.Player {
	public class PlayerHealth : UnitBase, IDamagable {

        public event Action<Vector3> OnDamaged;

        private float health;
        private bool dead;

        [SerializeField] private Animator anim;
        private IAnimationTrigger animationTrigger;

        private void Awake() {
            animationTrigger = anim.GetComponent<IAnimationTrigger>();

            health = Stats.Health;
        }

        private void OnEnable() {
            animationTrigger.OnAnimationTriggered += OnAnimationTriggered;
        }
        private void OnDisable() {
            animationTrigger.OnAnimationTriggered -= OnAnimationTriggered;
        }

        public void Damage(float damage, Vector3 attackerPosition) {

            if (dead) {
                return;
            }

            health -= damage;

            anim.SetTrigger("hurt");

            if (health <= 0) {
                DieAnimation();
                dead = true;
            }

            OnDamaged?.Invoke(attackerPosition);
        }

        private void DieAnimation() {
            anim.SetTrigger("die");
        }

        private void OnAnimationTriggered(AnimationTriggerType animationTriggerType) {
            if (animationTriggerType == AnimationTriggerType.Die) {
                Die();
            }
        }

        

        private void Die() {
            SceneManager.LoadScene("Game");
        }
    }
}