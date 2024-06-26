using UnityEngine;

namespace IslandDefender {

	[CreateAssetMenu(fileName = "Sounds", menuName = "Sounds")]
	public class ScriptableSounds : ScriptableObject {

        [SerializeField] private AudioClip dayMusic;
        public AudioClip DayMusic => dayMusic;

        [SerializeField] private AudioClip nightMusic;
        public AudioClip NightMusic => nightMusic;

        [SerializeField] private AudioClip grabGold;
        public AudioClip GrabGold => grabGold;

        [SerializeField] private AudioClip hitDamage;
        public AudioClip HitDamage => hitDamage;

        [SerializeField] private AudioClip meleeAttack;
        public AudioClip MeleeAttack => meleeAttack;

        [SerializeField] private AudioClip[] rangedAttacks;
        public AudioClip[] RangedAttacks => rangedAttacks;
    }
}