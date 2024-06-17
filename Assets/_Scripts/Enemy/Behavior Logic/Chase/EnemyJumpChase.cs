using IslandDefender.Environment.Building;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace IslandDefender {
    [CreateAssetMenu(fileName = "Jump Chase", menuName = "Enemy Logic/JumpChaseKeep")]
    public class EnemyJumpChase : EnemySOBase {

        [SerializeField] private Vector2 jumpForce;
        [SerializeField] private float jumpCooldown;

        private float jumpTimer;

        private Transform keep;

        public override void DoAnimationTriggerEventLogic(AnimationTriggerType triggerType) {
            base.DoAnimationTriggerEventLogic(triggerType);
        }

        public override void DoEnterLogic() {
            base.DoEnterLogic();
        }

        public override void DoExitLogic() {
            base.DoExitLogic();
        }

        public override void FrameUpdate() {
            base.FrameUpdate();

            // -1 or 1 - left or right
            int direction = _transform.position.x < keep.position.x ? 1 : -1;

            enemy.CheckForLeftOrRightFacing(enemy.GetMoveSpeed() * direction);

            jumpTimer += Time.deltaTime;
            if (jumpTimer > jumpCooldown) {
                jumpTimer = 0;
                enemy.RB.velocity = Vector2.zero;
                enemy.RB.AddForce(new Vector2(jumpForce.x * direction, jumpForce.y), ForceMode2D.Impulse);
            }

            StopOnGrounded();
            JumpOnSpikeContact();
        }

        [SerializeField] private LayerMask groundLayer;
        private bool grounded;

        private void StopOnGrounded() {

            float checkDistance = 0.1f;
            CapsuleCollider2D col = enemy.GetComponent<CapsuleCollider2D>();
            bool groundHit = Physics2D.CapsuleCast(col.bounds.center, col.size, col.direction, 0, Vector2.down, checkDistance, groundLayer);

            if (groundHit && !grounded) {
                grounded = true;
                enemy.SetEnemyXVel(0);
            }
            else if (!groundHit) {
                grounded = false;
            }
        }

        private bool touchingTrap;

        private void JumpOnSpikeContact() {

            float checkDistance = 0.1f;
            CapsuleCollider2D col = enemy.GetComponent<CapsuleCollider2D>();
            RaycastHit2D trapHit = Physics2D.CapsuleCast(col.bounds.center, col.size * 1.3f, col.direction, 0, Vector2.down, checkDistance);

            if (!touchingTrap && trapHit.transform.TryGetComponent(out Spikes spikes)) {
                jumpTimer = float.MaxValue;
                touchingTrap = true;
            }
            else if (!trapHit.transform.TryGetComponent(out Spikes _spikes)) {
                touchingTrap = false;
            }
        }

        public override void PhysicsUpdate() {
            base.PhysicsUpdate();
        }

        public override void Initialize(GameObject gameObject, Enemy enemy) {
            base.Initialize(gameObject, enemy);

            keep = FindObjectOfType<Keep>().transform;
        }

        public override void ResetValues() {
            base.ResetValues();

            jumpTimer = 0;
        }
    }
}