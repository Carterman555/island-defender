using System.Collections.Generic;
using UnityEngine;

namespace IslandDefender {
    public class EnemyStrikingDistanceCheck : MonoBehaviour {
        private Enemy enemy;

        private List<GameObject> objectsInRange = new();

        private void Awake() {
            enemy = GetComponentInParent<Enemy>();
        }

        private void OnTriggerEnter2D(Collider2D collision) {
            if (collision.gameObject.layer == GameLayers.PlayerLayer) {
                objectsInRange.Add(collision.gameObject);
                enemy.SetStrikingDistanceObject(objectsInRange[0]);
            }
        }

        private void OnTriggerExit2D(Collider2D collision) {
            if (collision.gameObject.layer == GameLayers.PlayerLayer) {
                objectsInRange.RemoveWithCheck(collision.gameObject);

                if (objectsInRange.Count > 0)
                    enemy.SetStrikingDistanceObject(objectsInRange[0]);
                else
                    enemy.SetStrikingDistanceObject(null);
            }
        }
    }
}