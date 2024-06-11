using System;
using TarodevController;
using UnityEngine;

namespace IslandDefender {
    public class PlayerMeleeAttack : MonoBehaviour {

        public static event Action OnAttack;

        [SerializeField] private LayerMask damagableLayerMask;

        [Header("Stats")]
        [SerializeField] private float damage;
        [SerializeField] private Vector2 size;
        [SerializeField] private float offset;
        [SerializeField] private float cooldown;

        private float attackTimer = float.MaxValue;

        [SerializeField] private Animator anim;
        [SerializeField] private PlayerAnimator playerAnimator;
        private PlayerController playerController;

        private void Awake() {
            playerController = GetComponent<PlayerController>();
        }

        private void OnEnable() {
            playerAnimator.OnAnimationTriggered += TryAttack;
        }
        private void OnDisable() {
            playerAnimator.OnAnimationTriggered -= TryAttack;
        }

        private void Update() {
            attackTimer += Time.deltaTime;
            if (attackTimer > cooldown && Input.GetMouseButtonDown(0)) {
                anim.SetTrigger("meleeAttack");
            }
        }

        private void TryAttack(AnimationTriggerType animationTriggerType) {
            if (animationTriggerType == AnimationTriggerType.MeleeAttack) {
                Attack();
            }
        }

        private void Attack() {

            // change the attack direction depending on the way the player is facing
            float directionalOffset = playerController.IsFacingRight ? offset : -offset;

            Vector3 center = new Vector3(transform.position.x + directionalOffset, transform.position.y);
            RaycastHit2D[] hits = Physics2D.BoxCastAll(center, size, 0, Vector2.right, 0, damagableLayerMask);

            foreach (RaycastHit2D hit in hits) {
                hit.transform.GetComponent<IDamagable>().Damage(damage, transform.position);
            }

            OnAttack?.Invoke();
        }

        private float boxWidth => size.x;
        private float boxHeight => size.y;

        void OnDrawGizmos() {
            if (Application.isPlaying) {
                // change the attack direction depending on the way the player is facing
                float directionalOffset = playerController.IsFacingRight ? offset : -offset;

                Vector3 center = new Vector3(transform.position.x + directionalOffset, transform.position.y);
                Vector3 size = new Vector3(boxWidth, boxHeight, 1);

                Gizmos.color = Color.red;
                Gizmos.DrawWireCube(center, size);
            }
        }
    }
}
