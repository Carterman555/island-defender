using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace IslandDefender {
    public class ResolutionText : MonoBehaviour {

        private TextMeshProUGUI text;

        private void Awake() {
            text = GetComponent<TextMeshProUGUI>();
        }

        private void Update() {
            text.text = Screen.width + ", " + Screen.height;
        }
    }
}
