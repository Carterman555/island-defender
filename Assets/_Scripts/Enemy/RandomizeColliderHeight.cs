using UnityEngine;

namespace IslandDefender {

	[RequireComponent (typeof(CapsuleCollider2D))]
	public class RandomizeColliderHeight : MonoBehaviour {

		private CapsuleCollider2D col;
        private float originalHeight;

        private void Awake() {
            col = GetComponent<CapsuleCollider2D>();
            originalHeight = col.size.y;
        }

        private void OnEnable() {
            float variance = 1f;
            col.size = new Vector2(col.size.x, originalHeight + Random.Range(0, variance));
        }
    }
}