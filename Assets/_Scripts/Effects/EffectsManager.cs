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
		[SerializeField] private Animator effectResourcePrefab;

		[SerializeField] private AnimatorOverrideController woodOverride;
		[SerializeField] private AnimatorOverrideController fiberOverride;
		[SerializeField] private AnimatorOverrideController stoneOverride;
		[SerializeField] private AnimatorOverrideController coinOverride;

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

            AnimatorOverrideController controller = woodOverride;
			if (resourceType == ResourceType.Wood) controller = woodOverride;
			if (resourceType == ResourceType.Fiber) controller = fiberOverride;
			if (resourceType == ResourceType.Stone) controller = stoneOverride;
			if (resourceType == ResourceType.Gold) controller = coinOverride;

			Animator newEffect = ObjectPoolManager.SpawnObject(effectResourcePrefab, position, Quaternion.identity, PlayerResources.Instance.transform);
            newEffect.runtimeAnimatorController = controller;

            float moveDistance = 2f;
			float duration = 0.5f;
			newEffect.transform.DOBlendableLocalMoveBy(new Vector3(0, moveDistance), duration).SetEase(Ease.InSine).OnComplete(() => {
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
            Vector3 offset = new Vector3(0, 2.7f);
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