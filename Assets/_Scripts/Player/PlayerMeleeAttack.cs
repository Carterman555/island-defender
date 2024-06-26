using IslandDefender.Audio;
using System;
using TarodevController;
using UnityEngine;

namespace IslandDefender.Units.Player {

    [RequireComponent(typeof(UnitBase))]
    public class PlayerMeleeAttack : MonoBehaviour {

        public static event Action OnAttack;

        [SerializeField] private LayerMask damagableLayerMask;

        [Header("Stats")]
        [SerializeField] private Vector2 size;
        [SerializeField] private float offset;

        private float attackTimer = float.MaxValue;

        [SerializeField] private Animator anim;
        [SerializeField] private PlayerAnimator playerAnimator;
        private PlayerController playerController;

        private Stats playerStats;

        private void Awake() {
            playerController = GetComponent<PlayerController>();
            playerStats = GetComponent<UnitBase>().Stats;
        }

        private void OnEnable() {
            playerAnimator.OnAnimationTriggered += TryAttack;
        }
        private void OnDisable() {
            playerAnimator.OnAnimationTriggered -= TryAttack;
        }

        private void Update() {
            attackTimer += Time.deltaTime;

            bool canAttack = attackTimer > playerStats.AttackCooldown && !Helpers.IsMouseOverUI();
            if (canAttack && Input.GetMouseButtonDown(0)) {
                anim.SetTrigger("meleeAttack");
                AudioManager.Instance.PlaySound(AudioManager.Instance.SoundClips.MeleeAttack, 0.2f, 0.25f);
                attackTimer = 0;
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
                hit.transform.GetComponent<IDamagable>().KnockbackDamage(playerStats.Damage, transform.position);
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
