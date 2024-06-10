using System.Collections.Generic;
using UnityEngine;

namespace IslandDefender {
    public class EnemyStrikingDistanceCheck : MonoBehaviour {
        private Enemy _enemy;

        private List<GameObject> _objectsInRange = new();

        private void Awake() {
            _enemy = GetComponentInParent<Enemy>();
        }

        private void OnTriggerEnter2D(Collider2D collision) {
            if (Helpers.IsCollisionWithOpposite(out IDamagable damagable, _enemy.Faction, collision.gameObject)) {
                _objectsInRange.Add(collision.gameObject);
                _enemy.SetStrikingDistanceObject(_objectsInRange[0]);
            }
        }

        private void OnTriggerExit2D(Collider2D collision) {
            if (Helpers.IsCollisionWithOpposite(out IDamagable damagable, _enemy.Faction, collision.gameObject)) {
                _objectsInRange.RemoveWithCheck(collision.gameObject);

                if (_objectsInRange.Count > 0)
                    _enemy.SetStrikingDistanceObject(_objectsInRange[0]);
                else
                    _enemy.SetStrikingDistanceObject(null);
            }
        }
    }
}