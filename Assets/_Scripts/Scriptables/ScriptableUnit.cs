using System;
using UnityEngine;

namespace IslandDefender.Units {

    [CreateAssetMenu(fileName = "NewUnit", menuName = "Units/BaseUnit")]
    public class ScriptableUnit : ScriptableObject {
        public Faction Faction;

        [SerializeField] private Stats _stats;
        public Stats BaseStats => _stats;

        // Used in game
        public GameObject Prefab;

        // Used in menus
        public string Description;
        public Sprite MenuSprite;
    }

    [Serializable]
    public struct Stats {
        public int Health;
        public float Damage;
        public float AttackCooldown;
        public float MoveSpeed;
        public float KnockBackable;
    }

    [Serializable]
    public enum Faction {
        Allies = 0,
        Enemies = 1,
        None = 2
    }
}