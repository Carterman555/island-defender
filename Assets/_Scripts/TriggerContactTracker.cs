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

        private void Update() {
            RemoveDisabled();
        }

        private void RemoveDisabled() {
            for (int i = contacts.Count - 1; i >= 0; i--) {
                if (!contacts[i].activeSelf) {
                    OnLeaveContact?.Invoke(contacts[i]);
                    contacts.RemoveAt(i);
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision) {
            if (layerFilter.ContainsLayer(collision.gameObject.layer)) {
                contacts.Add(collision.gameObject);
                OnEnterContact?.Invoke(collision.gameObject);
            }
        }

        private void OnTriggerExit2D(Collider2D collision) {
            if (layerFilter.ContainsLayer(collision.gameObject.layer)) {
                contacts.Remove(collision.gameObject);
                OnLeaveContact?.Invoke(collision.gameObject);
            }
        }
    }
}
