using System.Collections.Generic;
using UnityEngine;

namespace IslandDefender {
    public class EnemyAggroCheck : MonoBehaviour {
        private Enemy _enemy;

        private List<GameObject> _objectsInRange = new();

        private void Awake() {
            _enemy = GetComponentInParent<Enemy>();
        }

        private void OnTriggerEnter2D(Collider2D collision) {
            if (collision.gameObject.layer == GameLayers.PlayerLayer) {
                _objectsInRange.Add(collision.gameObject);
                _enemy.SetAggroedObject(_objectsInRange[0]);
            }
        }

        private void OnTriggerExit2D(Collider2D collision) {

            if (collision.gameObject.layer == GameLayers.PlayerLayer) {
                _objectsInRange.RemoveWithCheck(collision.gameObject);

                if (_objectsInRange.Count > 0)
                    _enemy.SetAggroedObject(_objectsInRange[0]);
                else
                    _enemy.SetAggroedObject(null);
            }
        }
    }
}