using UnityEngine;

namespace IslandDefender {

	[CreateAssetMenu(fileName = "Sounds", menuName = "Sounds")]
	public class ScriptableSounds : ScriptableObject {

        [SerializeField] private AudioClip dayMusic;
        public AudioClip DayMusic => dayMusic;

        [SerializeField] private AudioClip nightMusic;
        public AudioClip NightMusic => nightMusic;

        [SerializeField] private AudioClip[] steps;
		public AudioClip[] Steps => steps;

        [SerializeField] private AudioClip uiSlide;
        public AudioClip UISlide => uiSlide;

        [SerializeField] private AudioClip test;
        public AudioClip Test => test;

    }
}