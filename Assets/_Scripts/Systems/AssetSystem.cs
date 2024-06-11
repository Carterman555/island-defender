using UnityEngine;

namespace IslandDefender.Systems {
    public class AssetSystem : StaticInstance<AssetSystem> {

        [SerializeField] private PoisonBallProjectile example;
        public PoisonBallProjectile Example => example;
    }
}