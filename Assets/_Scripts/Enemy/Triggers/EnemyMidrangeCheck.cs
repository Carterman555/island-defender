using System.Collections.Generic;
using UnityEngine;

namespace IslandDefender {
    public class EnemyMidrangeCheck : MonoBehaviour {
        private Enemy _enemy;

        private List<GameObject> _objectsInRange = new();

        private void Awake() {
            _enemy = GetComponentInParent<Enemy>();
        }

        private void OnTriggerEnter2D(Collider2D collision) {
            if (collision.gameObject.layer == GameLayers.PlayerLayer) {
                _objectsInRange.Add(collision.gameObject);
                _enemy.SetMidrangeObject(_objectsInRange[0]);
            }
        }

        private void OnTriggerExit2D(Collider2D collision) {

            if (collision.gameObject.layer == GameLayers.PlayerLayer) {
                _objectsInRange.RemoveWithCheck(collision.gameObject);

                if (_objectsInRange.Count > 0)
                    _enemy.SetMidrangeObject(_objectsInRange[0]);
                else
                    _enemy.SetMidrangeObject(null);
            }
        }
    }
}