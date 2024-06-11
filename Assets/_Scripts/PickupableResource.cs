using IslandDefender.Management;
using IslandDefender.Utilities;
using UnityEngine;

namespace IslandDefender.Environment {
    public class PickupableResource : MonoBehaviour, IPickupable {

        [SerializeField] private ResourceType resourceType;
        [SerializeField] private RandomInt resourceDropAmount;

        public void Pickup() {
            PlayerResources.Instance.AddResource(ResourceType.Wood, resourceDropAmount.Randomize());
            ObjectPoolManager.ReturnObjectToPool(gameObject);
        }
    }
}