using UnityEngine;

namespace IslandDefender {

    [RequireComponent(typeof(IDamagable), typeof(UnitBase), typeof(Rigidbody2D))]
    public class Knockback : MonoBehaviour {

        private IDamagable damagable;
        private Rigidbody2D rb;
        private float knockbackable;

        private void Awake() {
            damagable = GetComponent<IDamagable>();
            rb = GetComponent<Rigidbody2D>();
            knockbackable = GetComponent<UnitBase>().Stats.KnockBackable;
        }

        private void OnEnable() {
            damagable.OnDamaged += ApplyKnockback;
        }
        private void OnDisable() {
            damagable.OnDamaged -= ApplyKnockback;
        }

        private void ApplyKnockback(Vector3 attackerPos) {

            int direction = attackerPos.x > transform.position.x ? -1 : 1;
            Vector3 upSideDirection = new Vector3(direction, 1f, 0);

            print("knock back");

            rb.AddForce(knockbackable * upSideDirection, ForceMode2D.Impulse);
        }
    }
}
