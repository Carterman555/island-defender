using UnityEngine;

namespace IslandDefender {
    public class ParallaxEffect : MonoBehaviour {
        Transform cam; // Camera reference (of its transform)
        Vector3 previousCamPos;

        public float distanceX; // Distance of the item (z-index based) 

        public float smoothingX = 1f; // Smoothing factor of parrallax effect

        void Awake() {
            cam = Camera.main.transform;
        }

        void Update() {

            if (distanceX != 0f) {
                float parallaxX = (previousCamPos.x - cam.position.x) * distanceX;
                Vector3 backgroundTargetPosX = new Vector3(transform.position.x + parallaxX, transform.position.y);
                transform.position = Vector3.Lerp(transform.position, backgroundTargetPosX, smoothingX * Time.deltaTime);
            }
            previousCamPos = cam.position;
        }
    }
}