using IslandDefender.Management;
using UnityEngine;

namespace IslandDefender.Units.Player {
	public class PlayerHealth : Health {

        [SerializeField] private float invincibleDuration;
        private float invincibleTimer;

        private bool invincible;

        protected override void Awake() {
            base.Awake();
            SetMaxHealth(GetComponent<UnitBase>().Stats.Health);
        }

        protected override void OnEnable() {
            base.OnEnable();
            EnemyWaveManager.OnNextWave += Heal;
        }
        protected override void OnDisable() {
            base.OnDisable();
            EnemyWaveManager.OnNextWave -= Heal;
        }

        public override void KnockbackDamage(float damage, Vector3 attackerPosition) {

            if (invincible) {
                return;
            }

            base.KnockbackDamage(damage, attackerPosition);

            invincible = true;
            invincibleTimer = 0;
        }

        private void Update() {
            if (invincible) {
                invincibleTimer += Time.deltaTime;
                if (invincibleTimer > invincibleDuration) {
                    invincible = false;
                    invincibleTimer = 0;
                }
            }
        }

        private void Heal(int n) {
            health = GetMaxHealth();
        }

        public override void Die() {
            GameManager.Instance.GameOver();
        }
    }
}