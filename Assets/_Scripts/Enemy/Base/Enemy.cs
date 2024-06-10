using UnityEngine;

namespace IslandDefender {
    public class Enemy : UnitBase, ITriggerCheckable {

        public Rigidbody2D RB { get; set; }
        public bool IsFacingRight { get; set; } = false;
        public GameObject ObjectAggroed { get; set; }
        public GameObject ObjectWithinStrikingDistance { get; set; }

        private bool isKnockbackBeingApplied = false;

        [SerializeField] private int startingWave;

        #region State Machine Variables

        public EnemyStateMachine StateMachine { get; set; }
        public EnemyIdleState IdleState { get; set; }
        public EnemyChaseState ChaseState { get; set; }
        public EnemyAttackState AttackState { get; set; }

        #endregion

        #region ScriptableObject Variables

        [SerializeField] private EnemySOBase _enemyIdleBase;
        [SerializeField] private EnemySOBase _enemyChaseBase;
        [SerializeField] private EnemySOBase _enemyAttackBase;

        public EnemySOBase EnemyIdleBaseInstance { get; set; }
        public EnemySOBase EnemyChaseBaseInstance { get; set; }
        public EnemySOBase EnemyAttackBaseInstance { get; set; }

        #endregion

        [field: SerializeField] public Animator Anim { get; private set; }



        private void Awake() {
            EnemyIdleBaseInstance = Instantiate(_enemyIdleBase);
            EnemyChaseBaseInstance = Instantiate(_enemyChaseBase);
            EnemyAttackBaseInstance = Instantiate(_enemyAttackBase);

            StateMachine = new EnemyStateMachine();

            IdleState = new EnemyIdleState(this, StateMachine);
            ChaseState = new EnemyChaseState(this, StateMachine);
            AttackState = new EnemyAttackState(this, StateMachine);

            RB = GetComponent<Rigidbody2D>();

            //_knockBack = new EnemyKnockBackLogic(this, GetComponent<Health>(), RB);

            InitializeInstances();
        }

        private void OnEnable() {
            ResetValues();
            //_knockBack.ResetValues();

            StateMachine.Initialize(IdleState);

            // spawn animation
            Anim.SetTrigger("spawn");
            Anim.SetBool("isGrounded", true);
        }

        protected virtual void InitializeInstances() {
            EnemyIdleBaseInstance.Initialize(gameObject, this);
            EnemyChaseBaseInstance.Initialize(gameObject, this);
            EnemyAttackBaseInstance.Initialize(gameObject, this);
        }

        private void Update() {
            //if (IsDead()) return;

            //IsKnockbackBeingApplied = _knockBack.IsApplyingKnockback();

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

            SetAggroedObject(null);
            SetStrikingDistanceObject(null);
        }

        public float GetMoveSpeed() {
            return Stats.MoveSpeed + _speedVariance;
        }

        public void SetEnemyXVel(float xVelocity) {
            if (isKnockbackBeingApplied && Stats.KnockBackable > 0) return;

            RB.velocity = new Vector3(xVelocity, RB.velocity.y);
            CheckForLeftOrRightFacing(xVelocity);

            Anim.SetBool("isMoving", xVelocity != 0);
        }

        public void CheckForLeftOrRightFacing(float xVelocity) {
            if (IsFacingRight && xVelocity < 0f) {
                Vector3 rotator = new Vector3(transform.rotation.x, 0f, transform.rotation.z);
                transform.rotation = Quaternion.Euler(rotator);
                IsFacingRight = !IsFacingRight;
            }
            else if (!IsFacingRight && xVelocity > 0f) {
                Vector3 rotator = new Vector3(transform.rotation.x, 180f, transform.rotation.z);
                transform.rotation = Quaternion.Euler(rotator);
                IsFacingRight = !IsFacingRight;
            }
        }

        #endregion

        #region Trigger Functions

        public void SetAggroedObject(GameObject objectAggroed) {
            ObjectAggroed = objectAggroed;
        }

        public void SetStrikingDistanceObject(GameObject objectWithinStrikingDistance) {
            ObjectWithinStrikingDistance = objectWithinStrikingDistance;
        }

        #endregion

        #region Animation Triggers

        // played by animation
        private void AnimationTriggerEvent(AnimationTriggerType triggerType) {
            StateMachine.CurrentEnemyState.AnimationTriggerEvent(triggerType);
        }

        public enum AnimationTriggerType {
            EnemyAttack
        }

        #endregion
    }

    public enum EnemyType {
        Frog,
    }
}