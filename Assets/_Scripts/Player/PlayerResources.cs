using IslandDefender.Environment;
using System;
using System.Collections.Generic;

namespace IslandDefender {
    public class PlayerResources : StaticInstance<PlayerResources> {

        public static event Action<ResourceType, int> OnResourceAdded;
        public static event Action<ResourceType, int> OnResourceRemoved;

        private Dictionary<ResourceType, int> resourceAmounts;

        protected override void Awake() {
            base.Awake();

            // player starts with 0 of each resource
            resourceAmounts = new Dictionary<ResourceType, int>();
            foreach (ResourceType type in Enum.GetValues(typeof(ResourceType))) {
                resourceAmounts.Add(type, 0);
            }
        }

        private void Start() {
            AddResource(ResourceType.Wood, 99);
            //AddResource(ResourceType.Fiber, 99);
            //AddResource(ResourceType.Stone, 99);
            AddResource(ResourceType.Gold, 99);
        }

        public void AddResource(ResourceType resourceType, int amount) {
            resourceAmounts[resourceType] += amount;
            OnResourceAdded?.Invoke(resourceType, resourceAmounts[resourceType]);
        }

        public void RemoveResource(ResourceType resourceType, int amount) {
            resourceAmounts[resourceType] -= amount;
            OnResourceRemoved?.Invoke(resourceType, resourceAmounts[resourceType]);
        }

        public int GetResourceAmount(ResourceType resourceType) {
            return resourceAmounts[resourceType];
        }
    }
}
