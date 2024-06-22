using UnityEngine;

namespace IslandDefender.Systems {
    public class AssetSystem : StaticInstance<AssetSystem> {

        [SerializeField] private HitEffect hitEffect;
        public HitEffect HitEffect => hitEffect;
    }
}