using UnityEngine;

namespace IslandDefender {

    [RequireComponent(typeof(UnitBase))]
    public class UnitHealth : Health {

        private bool invincible;

        #region Set Methods

        [SerializeField] private SpriteRenderer spriteRenderer;// replace with animation

        public void SetInvincible(bool invincible) {
            this.invincible = invincible;
            spriteRenderer.color = invincible ? Color.gray : Color.white; // replace with animation
        }

        #endregion

        protected override void ResetValues() {
            base.ResetValues();
            health = GetComponent<UnitBase>().Stats.Health;
        }

        public override void KnockbackDamage(float damage, Vector3 attackerPosition) {

            if (invincible) {
                return;
            }

            base.KnockbackDamage(damage, attackerPosition);
        }
    }
}
