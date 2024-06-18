using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace IslandDefender {

    public class DayNightCycle : MonoBehaviour {

        public static event Action OnDayTime;
        public static event Action OnNightTime;

        [SerializeField] private Volume dayVolume;
        [SerializeField] private Volume nightVolume;
        [SerializeField] private float cycleDuration = 60f; // Duration of a full day-night cycle in seconds

        private float cycleTimer;
        private bool turningNight = true;
        private bool isDay = true;

        void Start() {
            cycleTimer = 0f;
            dayVolume.weight = 1f;
            nightVolume.weight = 0f;
        }

        void Update() {
            cycleTimer += Time.deltaTime;

            if (cycleTimer >= cycleDuration) {
                cycleTimer = 0f;
                turningNight = !turningNight;
            }

            float t = cycleTimer / cycleDuration;
            dayVolume.weight = turningNight ? Mathf.Lerp(1, 0, t) : Mathf.Lerp(0, 1, t);
            nightVolume.weight = turningNight ? Mathf.Lerp(0, 1, t) : Mathf.Lerp(1, 0, t);

            if (!isDay && dayVolume.weight > 0.5f) {
                OnDayTime?.Invoke();
                isDay = true;
            }

            if (isDay && nightVolume.weight > 0.5f) {
                OnNightTime?.Invoke();
                isDay = false;
            }
        }
    }
}