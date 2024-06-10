using RoboRiftRush;
using TarodevController;
using UnityEngine;

namespace IslandDefender {
    public class PlayerAttack : MonoBehaviour {

        [SerializeField] private LayerMask damagableLayerMask;

        [Header("Stats")]
        [SerializeField] private float damage;
        [SerializeField] private Vector2 size;
        [SerializeField] private float offset;
        [SerializeField] private float cooldown;

        private float attackTimer = float.MaxValue;

        private PlayerController playerController;

        private void Awake() {
            playerController = GetComponent<PlayerController>();
        }

        private void Update() {
            attackTimer += Time.deltaTime;
            if (attackTimer > cooldown && Input.GetMouseButtonDown(0)) {
                Attack();
            }
        }

        private void Attack() {

            // change the attack direction depending on the way the player is facing
            float directionalOffset = playerController.IsFacingRight ? offset : -offset;

            Vector3 center = new Vector3(transform.position.x + directionalOffset, transform.position.y);
            RaycastHit2D[] hits = Physics2D.BoxCastAll(center, size, 0, Vector2.right, 0, damagableLayerMask);

            foreach (RaycastHit2D hit in hits) {
                hit.transform.GetComponent<IDamagable>().Damage(damage);
            }

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
