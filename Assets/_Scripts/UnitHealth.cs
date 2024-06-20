using UnityEngine;

namespace IslandDefender {

    [RequireComponent(typeof(UnitBase))]
    public class UnitHealth : Health {

        private bool invincible;

        #region Set Methods

        public void SetInvincible(bool invincible) {
            this.invincible = invincible;
            //spriteRenderer.color = invincible ? Color.gray : Color.white; // replace with animation
        }

        #endregion

        protected override void Awake() {
            base.Awake();
            SetMaxHealth(GetComponent<UnitBase>().Stats.Health);
        }

        public override void KnockbackDamage(float damage, Vector3 attackerPosition) {

            if (invincible) {
                return;
            }

            base.KnockbackDamage(damage, attackerPosition);
        }
    }
}
