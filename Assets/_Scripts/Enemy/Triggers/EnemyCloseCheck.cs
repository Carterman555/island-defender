using System.Collections.Generic;
using UnityEngine;

namespace IslandDefender {
    public class EnemyCloseCheck : MonoBehaviour {

        private Enemy enemy;
        private List<GameObject> objectsInRange = new();

        private void Awake() {
            enemy = GetComponentInParent<Enemy>();
        }

        [SerializeField] private LayerMask layerFilter;

        private void OnTriggerEnter2D(Collider2D collision) {
            if (layerFilter.ContainsLayer(collision.gameObject.layer)) {
                objectsInRange.Add(collision.gameObject);
                enemy.SetCloseObject(objectsInRange[0]);
            }
        }

        private void OnTriggerExit2D(Collider2D collision) {
            if (layerFilter.ContainsLayer(collision.gameObject.layer)) {
                objectsInRange.RemoveWithCheck(collision.gameObject);

                if (objectsInRange.Count > 0)
                    enemy.SetCloseObject(objectsInRange[0]);
                else
                    enemy.SetCloseObject(null);
            }
        }
    }
}