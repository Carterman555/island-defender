using UnityEngine;

namespace IslandDefender {
    public class Enemy : UnitBase, ITriggerCheckable {

        public Rigidbody2D RB { get; set; }
        [field: SerializeField] public Animator Anim { get; private set; }
        public UnitHealth Health { get; private set; }
        public Knockback Knockback { get; private set; }

        [SerializeField] private bool jumpEnemy;

        public bool IsFacingRight { get; set; } = false;

        private GameObject midrangeObject { get; set; }
        private GameObject closeObject { get; set; }

        public EnemyStateMachine StateMachine { get; set; }

        #region ScriptableObject Variables

        [SerializeField] private EnemySOBase scriptableFarState;
        [SerializeField] private EnemySOBase scriptableMidrangeState;
        [SerializeField] private EnemySOBase scriptableCloseState;

        public EnemySOBase FarState { get; set; }
        public EnemySOBase MidrangeState { get; set; }
        public EnemySOBase CloseState { get; set; }

        #endregion

        #region Get Methods

        public GameObject GetMidrangeObject() {
            return midrangeObject;
        }

        public GameObject GetCloseObject() {
            return closeObject;
        }

        #endregion

        private bool move = true;
        public void SetMove(bool move) {
            this.move = move;
        }

        private void Awake() {
            FarState = Instantiate(scriptableFarState);
            MidrangeState = Instantiate(scriptableMidrangeState);
            CloseState = Instantiate(scriptableCloseState);

            StateMachine = new EnemyStateMachine();

            RB = GetComponent<Rigidbody2D>();
            Health = GetComponent<UnitHealth>();
            Knockback = GetComponent<Knockback>();

            InitializeInstances();
        }

        private void OnEnable() {
            ResetValues();

            StateMachine.Initialize(FarState);

            SetMidrangeObject(null);
            SetCloseObject(null);
        }

        protected virtual void InitializeInstances() {
            FarState.Initialize(gameObject, this);
            MidrangeState.Initialize(gameObject, this);
            CloseState.Initialize(gameObject, this);
        }

        private void Update() {
            StateMachine.CurrentEnemyState.FrameUpdate();
        }

        private void FixedUpdate() {
            StateMachine.CurrentEnemyState.PhysicsUpdate();
        }

        #region Movement

        float _speedVariance;

        private void ResetValues() {
            // make enemies have slighty different speeds (to not clump)
            _speedVariance = Random.Range(-0.2f, 0.2f);

            // reset the facing direction
            IsFacingRight = false;
        }

        public float GetMoveSpeed() {
            return Stats.MoveSpeed + _speedVariance;
        }

        public void SetEnemyXVel(float xVelocity) {

            if (!move) return;
            if (Stats.KnockBackable > 0 && Knockback.BeingKnockedBack()) return;

            RB.velocity = new Vector3(xVelocity, RB.velocity.y);
            CheckForLeftOrRightFacing(xVelocity);

            if (!jumpEnemy) {
                Anim.SetBool("isMoving", xVelocity != 0);
            }
        }

        public void CheckForLeftOrRightFacing(float xVelocity) {
            if (IsFacingRight && xVelocity < 0f) {
                Vector3 rotator = new Vector3(transform.rotation.x, 180f, transform.rotation.z);
                transform.rotation = Quaternion.Euler(rotator);
                IsFacingRight = !IsFacingRight;
            }
            else if (!IsFacingRight && xVelocity > 0f) {
                Vector3 rotator = new Vector3(transform.rotation.x, 0f, transform.rotation.z);
                transform.rotation = Quaternion.Euler(rotator);
                IsFacingRight = !IsFacingRight;
            }
        }

        #endregion

        #region Trigger Functions

        public void SetMidrangeObject(GameObject midrangeObject) {
            this.midrangeObject = midrangeObject;
            UpdateState();
        }

        public void SetCloseObject(GameObject closeObject) {
            this.closeObject = closeObject;
            UpdateState();
        }

        private void UpdateState() {
            if (closeObject != null) {
                StateMachine.ChangeState(CloseState);
            }
            else if (midrangeObject != null) {
                StateMachine.ChangeState(MidrangeState);
            }
            else {
                StateMachine.ChangeState(FarState);
            }
        }

        #endregion

        #region Animation Triggers

        // played by animation
        public void AnimationTriggerEvent(AnimationTriggerType triggerType) {
            StateMachine.CurrentEnemyState.DoAnimationTriggerEventLogic(triggerType);
        }

        #endregion
    }
}