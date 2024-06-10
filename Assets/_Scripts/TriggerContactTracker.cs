using System;
using System.Collections.Generic;
using UnityEngine;

namespace IslandDefender {
    public class TriggerContactTracker : MonoBehaviour {

        public event Action<GameObject> OnEnterContact;
        public event Action<GameObject> OnLeaveContact;

        [SerializeField] private LayerMask layerFilter;

        private List<GameObject> contacts = new List<GameObject>();

        public List<GameObject> GetContacts() {
            return contacts;
        }

        private void OnTriggerEnter2D(Collider2D collision) {
            if (IsInLayerMask(collision.gameObject.layer, layerFilter)) {
                contacts.Add(collision.gameObject);
                OnEnterContact?.Invoke(collision.gameObject);
            }
        }

        private void OnTriggerExit2D(Collider2D collision) {
            if (IsInLayerMask(collision.gameObject.layer, layerFilter)) {
                contacts.Remove(collision.gameObject);
                OnLeaveContact?.Invoke(collision.gameObject);
            }
        }

        private bool IsInLayerMask(int layer, LayerMask mask) {
            return (mask.value & (1 << layer)) != 0;
        }

    }
}
