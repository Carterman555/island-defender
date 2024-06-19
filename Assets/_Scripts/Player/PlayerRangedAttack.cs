using IslandDefender.Environment;
using IslandDefender.Management;
using TarodevController;
using UnityEngine;

namespace IslandDefender.Units.Player {
	public class PlayerRangedAttack : MonoBehaviour {

		[SerializeField] private Arrow arrowPrefab;
        [SerializeField] private Vector2 arrowOffset;

        private float attackTimer = float.MaxValue;

        [SerializeField] private Animator anim;
        [SerializeField] private PlayerAnimator playerAnimator;
        private PlayerController playerController;

        [SerializeField] private float cooldown;
        [SerializeField] private float damage;

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

            bool canAttack = attackTimer > cooldown && !PlayerBuild.Instance.BuildingVisualActive;
            if (canAttack && Input.GetMouseButtonDown(1)) {
                attackTimer = 0;
                anim.SetTrigger("rangedAttack");
            }
        }

        private void TryAttack(AnimationTriggerType animationTriggerType) {
            if (animationTriggerType == AnimationTriggerType.RangedAttack) {
                Attack();
            }
        }

        private void Attack() {

            int direction = playerController.IsFacingRight ? 1 : -1;
            Vector2 directionalOffset = new Vector2(arrowOffset.x * direction, arrowOffset.y);

            Arrow arrow = ObjectPoolManager.SpawnObject(arrowPrefab,
                transform.position + (Vector3)directionalOffset,
                Quaternion.identity,
                Containers.Instance.Projectiles);

            arrow.Shoot(direction, damage);
        }
    }
}