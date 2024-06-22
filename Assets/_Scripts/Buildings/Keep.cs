using IslandDefender.Management;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IslandDefender.Environment.Building {
    public class Keep : Health {

        public static event Action<int> OnUpgrade;

        [SerializeField] private TriggerContactTracker playerContactTracker;

        private int maxLevel = 4;
        private int level = 1;
        private bool playerTouching;

        [Header("Shooting")]
        [SerializeField] private TowerProjectile weakProjectilePrefab;
        [SerializeField] private TowerProjectile strongProjectilePrefab;
        [SerializeField] private Transform projectileSpawnPoint;
        [SerializeField] private TriggerContactTracker enemiesInRangeTracker;

        private TowerProjectile currentProjectilePrefab;

        [Header("Stats")]
        [SerializeField] private float baseDamage;
        [SerializeField] private float levelDamageIncrease;
        [SerializeField] private float levelHealthIncrease;
        [SerializeField] private float shootCooldown;
        private float shootTimer;

        [Header("Visual")]
        [SerializeField] private Animator animator;

        [SerializeField] private int startingCost = 10;
        [SerializeField] private int costIncrease = 5;
        private int currentCost;

        protected override void OnEnable() {
            base.OnEnable();
            playerContactTracker.OnEnterContact += PlayerEnter;
            playerContactTracker.OnLeaveContact += PlayerExit;

            currentCost = startingCost;
        }

        protected override void OnDisable() {
            base.OnDisable();
            playerContactTracker.OnEnterContact -= PlayerEnter;
            playerContactTracker.OnLeaveContact -= PlayerExit;

        }

        private void PlayerEnter(GameObject player) {
            if (level >= maxLevel) {
                return;
            }

            playerTouching = true;

            Vector2 offset = new Vector2(0, 2.6f);
            string text = "Press R to Upgrade Keep\nCost: " + currentCost + " Gold";
            WorldPopupText.Instance.ShowText((Vector2)transform.position + offset, text);
        }

        private void PlayerExit(GameObject player) {
            playerTouching = false;
            WorldPopupText.Instance.HideText();
        }

        private void Update() {
            bool canAfford = PlayerResources.Instance.GetResourceAmount(ResourceType.Gold) >= currentCost;
            if (playerTouching && canAfford && Input.GetKeyDown(KeyCode.R)) {
                LevelUp();
            }

            shootTimer += Time.deltaTime;

            bool highEnoughLevel = level >= 3;
            if (highEnoughLevel && shootTimer > shootCooldown && EnemyInRange(out Vector3 closestEnemyPos)) {
                shootTimer = 0;
                Shoot(closestEnemyPos);
            }
        }

        private void LevelUp() {

            level++;

            PlayerResources.Instance.RemoveResource(ResourceType.Gold, currentCost);

            currentCost = startingCost + ((level - 1) * costIncrease);

            if (level >= 3) {
                currentProjectilePrefab = strongProjectilePrefab;
            }
            else if (level >= 2) {
                currentProjectilePrefab = weakProjectilePrefab;
            }

            SetMaxHealth(GetMaxHealth() + levelHealthIncrease);
            health += levelHealthIncrease;

            animator.SetInteger("KeepLevel", level);

            OnUpgrade?.Invoke(level);

            if (level >= maxLevel) {
                playerTouching = false;
                WorldPopupText.Instance.HideText();
                return;
            }

            Vector2 offset = new Vector2(0, 2.6f);
            string text = "Press R to Upgrade Keep\nCost: " + currentCost + " Gold";
            WorldPopupText.Instance.ShowText((Vector2)transform.position + offset, text);
        }

        private void Shoot(Vector3 closestEnemyPos) {
            TowerProjectile projectile = ObjectPoolManager.SpawnObject(currentProjectilePrefab,
                projectileSpawnPoint.position,
                Quaternion.identity,
                Containers.Instance.Projectiles);

            float damage = baseDamage + (level * levelDamageIncrease);
            projectile.Shoot(closestEnemyPos, damage);
        }

        private bool EnemyInRange(out Vector3 position) {

            GameObject closestEnemy = enemiesInRangeTracker.GetContacts()
                .OrderBy(go => Vector2.Distance(go.transform.position, transform.position))
                .FirstOrDefault();

            if (closestEnemy == null) {
                position = Vector3.zero;
                return false;
            }
            else {
                position = closestEnemy.transform.position;
                return true;
            }
        }

        public override void Die() {
            GameManager.Instance.GameOver();
        }
    }
}
