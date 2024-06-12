using IslandDefender.Environment;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace IslandDefender {
    public class PlayerResources : StaticInstance<PlayerResources> {

        public static event Action<ResourceType, int> OnResourceChanged;

        private Dictionary<ResourceType, int> resourceAmounts;

        protected override void Awake() {
            base.Awake();

            // player starts with 0 of each resource
            resourceAmounts = new Dictionary<ResourceType, int>();
            foreach (ResourceType type in Enum.GetValues(typeof(ResourceType))) {
                resourceAmounts.Add(type, 0);
            }
        }

        public void AddResource(ResourceType resourceType, int amount) {
            resourceAmounts[resourceType] += amount;
            OnResourceChanged?.Invoke(resourceType, resourceAmounts[resourceType]);
        }

        public void RemoveResource(ResourceType resourceType, int amount) {
            resourceAmounts[resourceType] -= amount;
            OnResourceChanged?.Invoke(resourceType, resourceAmounts[resourceType]);
        }

        public int GetResourceAmount(ResourceType resourceType) {
            return resourceAmounts[resourceType];
        }
    }
}
