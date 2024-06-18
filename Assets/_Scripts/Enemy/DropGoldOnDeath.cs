using IslandDefender.Environment;
using IslandDefender.Management;
using IslandDefender.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IslandDefender {

    [RequireComponent(typeof(IDamagable))]
    public class DropGoldOnDeath : MonoBehaviour {

        [SerializeField] private RandomInt dropAmount;
        [SerializeField] private GoldDrop goldPrefab;

        private IDamagable damagable;

        private void Awake() {
            damagable = GetComponent<IDamagable>();
        }

        private void OnEnable() {
            damagable.OnDeath += DropGold;
        }
        private void OnDisable() {
            damagable.OnDeath -= DropGold;
        }

        private void DropGold() {
            dropAmount.Randomize();
            for (int i = 0; i < dropAmount.Value; i++) {
                ObjectPoolManager.SpawnObject(goldPrefab, transform.position, Quaternion.identity, Containers.Instance.Resources);
            }
        }
    }
}
