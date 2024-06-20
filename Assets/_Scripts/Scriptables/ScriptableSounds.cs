using UnityEngine;

namespace IslandDefender {

	[CreateAssetMenu(fileName = "Sounds", menuName = "Sounds")]
	public class ScriptableSounds : ScriptableObject {

        [SerializeField] private AudioClip[] music;
        public AudioClip[] Music => music;

        [SerializeField] private AudioClip[] steps;
		public AudioClip[] Steps => steps;

        [SerializeField] private AudioClip uiSlide;
        public AudioClip UISlide => uiSlide;

        [SerializeField] private AudioClip test;
        public AudioClip Test => test;

    }
}