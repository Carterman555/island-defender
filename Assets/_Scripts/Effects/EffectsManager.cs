using DG.Tweening;
using IslandDefender.Environment;
using IslandDefender.Environment.Building;
using IslandDefender.Management;
using IslandDefender.UI;
using TMPro;
using UnityEngine;

namespace IslandDefender {
	public class EffectsManager : StaticInstance<EffectsManager> {

		[Header("Resource Effect")]
		[SerializeField] private SpriteRenderer effectResourcePrefab;

		[SerializeField] private Sprite woodSprite;
		[SerializeField] private Sprite fiberSprite;
		[SerializeField] private Sprite stoneSprite;
		[SerializeField] private Sprite coinSprite;

		[Header("Keep Effect")]
		[SerializeField] private TextMeshPro textEffect;


        private void OnEnable() {
			PlayerResources.OnResourceAdded += ResourceEffect;
			Keep.OnUpgrade += KeepUpgradeEffect;
            BuildingButtonContainer.OnNewBuildingUnlocked += NewBuildingEffect;
        }
        private void OnDisable() {
			PlayerResources.OnResourceAdded -= ResourceEffect;
            Keep.OnUpgrade -= KeepUpgradeEffect;
            BuildingButtonContainer.OnNewBuildingUnlocked -= NewBuildingEffect;
        }

        #region Resource Effect

        private void ResourceEffect(ResourceType type, int amount) {
			Vector3 offset = new Vector3(0, 2.5f);
			CreateResourceEffect(type, PlayerResources.Instance.transform.position + offset);
        }

        public void CreateResourceEffect(ResourceType resourceType, Vector2 position) {

			Sprite sprite = woodSprite;
			if (resourceType == ResourceType.Wood) sprite = woodSprite;
			if (resourceType == ResourceType.Fiber) sprite = fiberSprite;
			if (resourceType == ResourceType.Stone) sprite = stoneSprite;
			if (resourceType == ResourceType.Gold) sprite = coinSprite;

			SpriteRenderer newEffect = ObjectPoolManager.SpawnObject(effectResourcePrefab, position, Quaternion.identity, PlayerResources.Instance.transform);
            newEffect.sprite = sprite;
			newEffect.Fade(1);

            float moveDistance = 2f;
			float duration = 1f;
			newEffect.transform.DOBlendableLocalMoveBy(new Vector3(0, moveDistance), duration);

			newEffect.DOFade(0, duration).SetEase(Ease.InSine).OnComplete(() => {
				ObjectPoolManager.ReturnObjectToPool(newEffect.gameObject);
			});
        }

        #endregion

        private void KeepUpgradeEffect(int level) {
            Vector3 offset = new Vector3(0, 4f);
            Vector3 position = FindObjectOfType<Keep>().transform.position + offset;
            TextMeshPro newEffect = ObjectPoolManager.SpawnObject(textEffect, position, Quaternion.identity, Containers.Instance.Effects);

			newEffect.text = "Keep Leveled Up!";
			newEffect.fontSize = 12;
			newEffect.Fade(1);

            float moveDistance = 2f;
            float duration = 1f;
            newEffect.transform.DOBlendableLocalMoveBy(new Vector3(0, moveDistance), duration);

            newEffect.DOFade(0, duration).SetEase(Ease.InSine).OnComplete(() => {
                ObjectPoolManager.ReturnObjectToPool(newEffect.gameObject);
            });
        }

        private void NewBuildingEffect() {
            Vector3 offset = new Vector3(0, 3f);
            Vector3 position = FindObjectOfType<Keep>().transform.position + offset;
            TextMeshPro newEffect = ObjectPoolManager.SpawnObject(textEffect, position, Quaternion.identity, Containers.Instance.Effects);

            newEffect.text = "New Building Unlocked";
            newEffect.fontSize = 10;
            newEffect.Fade(1);

            float moveDistance = 2f;
            float duration = 1f;
            newEffect.transform.DOBlendableLocalMoveBy(new Vector3(0, moveDistance), duration);

            newEffect.DOFade(0, duration).SetEase(Ease.InSine).OnComplete(() => {
                ObjectPoolManager.ReturnObjectToPool(newEffect.gameObject);
            });
        }
    }
}