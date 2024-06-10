using IslandDefender.Environment;
using TMPro;
using UnityEngine;

namespace IslandDefender {
    public class ResourceText : MonoBehaviour {

        [SerializeField] private ResourceType resourceType;

        private TextMeshProUGUI text;

        private void Awake() {
            text = GetComponent<TextMeshProUGUI>();
        }

        private void OnEnable() {
            PlayerResources.OnResourceChanged += TryUpdateText;
        }
        private void OnDisable() {
            PlayerResources.OnResourceChanged -= TryUpdateText;
        }

        private void TryUpdateText(ResourceType resourceType, int amount) {
            if (resourceType == this.resourceType) {
                text.text = resourceType.ToString() + ": " + amount;
            }
        }
    }
}
