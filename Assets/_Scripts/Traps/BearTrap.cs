using IslandDefender.Management;
using UnityEngine;

namespace IslandDefender {
	public class BearTrap : MonoBehaviour {

        [SerializeField] private GameObject openVisual;
        [SerializeField] private GameObject closeVisual;

        [SerializeField] private float damage;
        [SerializeField] private float resetCooldown;
        private float resetTimer;

		private bool set = true;

        private void OnEnable() {
            resetTimer = 0;
            Open();
        }

        private void OnTriggerEnter2D(Collider2D collision) {
            if (set && collision.gameObject.layer == GameLayers.EnemyLayer) {
                Close();

                if (collision.TryGetComponent(out BearTrapBreaker bearTrapBreaker)) {
                    ObjectPoolManager.ReturnObjectToPool(gameObject);
                }

                collision.GetComponent<IDamagable>().KnockbackDamage(damage, transform.position);
            }
        }

        private void Update() {
            if (!set) {
                resetTimer += Time.deltaTime;
                if (resetTimer > resetCooldown) {
                    resetTimer = 0;
                    Open();
                }
            }
        }

        private void Close() {
            openVisual.SetActive(false);
            closeVisual.SetActive(true);
            set = false;
        }

        private void Open() {
            openVisual.SetActive(true);
            closeVisual.SetActive(false);
            set = true;
        }
    }
}