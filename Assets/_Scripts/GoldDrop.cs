using DG.Tweening;
using IslandDefender.Management;
using UnityEngine;

namespace IslandDefender {
    public class GoldDrop : MonoBehaviour {

        [SerializeField] private float bobDuration;
        [SerializeField] private float bobMag;

        private void Start() {
            transform.DOMoveY(transform.position.y + bobMag, bobDuration).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        }

        private void OnTriggerEnter2D(Collider2D collision) {
            if (collision.gameObject.layer == GameLayers.PlayerLayer) {
                PlayerResources.Instance.AddResource(Environment.ResourceType.Gold, 1);
                ObjectPoolManager.ReturnObjectToPool(gameObject);
                AudioManager.Instance.PlaySound(AudioManager.Instance.SoundClips.GrabGold, 0.3f, 0.25f);
            }
        }
    }
}
