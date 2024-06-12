using IslandDefender.Management;
using UnityEngine;

namespace IslandDefender {
    public class PoisonArea : MonoBehaviour {

        [SerializeField] private float duration;
        private float timer;

        private void OnEnable() {
            ResetValues();
        }

        private void ResetValues() {
            timer = 0;
        }

        private void Update() {
            timer += Time.deltaTime;
            if (timer > duration) {
                ObjectPoolManager.ReturnObjectToPool(gameObject);
            }
        }
    }
}
