using IslandDefender.Audio;
using IslandDefender.Utilities;
using UnityEngine;

namespace IslandDefender {
	public class RainManager : MonoBehaviour {

		[SerializeField] private ParticleSystem rainParticles;
		[SerializeField] private int rate;

		private bool raining;

        [SerializeField] private RandomFloat clearDuration;
        [SerializeField] private RandomFloat rainDuration;
		private float rainTimer;

        private void Start() {
            clearDuration.Randomize();
            rainDuration.Randomize();

            var emission = rainParticles.emission;
            emission.rateOverTime = 0;
        }

        private void Update() {

            rainTimer += Time.deltaTime;

			if (raining) {
				if (rainTimer > rainDuration.Value) {
					rainTimer = 0;
					clearDuration.Randomize();
					raining = false;

					var emission = rainParticles.emission;
					emission.rateOverTime = 0;

                    AudioManager.Instance.StopRain();
                }
			}
			else {
				if (rainTimer > clearDuration.Value) {
					rainTimer = 0;
					rainDuration.Randomize();
					raining = true;

                    var emission = rainParticles.emission;
                    emission.rateOverTime = rate;

                    AudioManager.Instance.PlayRain();
                }
            }
        }
    }
}