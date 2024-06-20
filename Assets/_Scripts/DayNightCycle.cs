using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace IslandDefender {

    public class DayNightCycle : StaticInstance<DayNightCycle> {

        public static event Action OnNightTime;
        public static event Action OnDayTime;
        public static event Action OnTurningNightTime;

        [SerializeField] private Volume dayVolume;
        [SerializeField] private Volume nightVolume;

        [SerializeField] private float dayDuration = 80f;
        [SerializeField] private float transitionDuration = 20f;

        private float cycleTimer;

        private CycleStage currentStage;

        void Start() {
            cycleTimer = 0f;
            dayVolume.weight = 1f;
            nightVolume.weight = 0f;

            currentStage = CycleStage.Day;
        }

        void Update() {

            cycleTimer += Time.deltaTime;

            if (currentStage == CycleStage.Day) {
                if (cycleTimer >= dayDuration) {
                    cycleTimer = 0f;
                    currentStage = CycleStage.TurningNight;
                    OnTurningNightTime?.Invoke();
                }
            }

            else if (currentStage == CycleStage.TurningNight) {

                dayVolume.weight = Mathf.InverseLerp(transitionDuration, 0, cycleTimer);
                nightVolume.weight = Mathf.InverseLerp(0, transitionDuration, cycleTimer);

                if (cycleTimer >= transitionDuration) {
                    cycleTimer = 0f;
                    currentStage = CycleStage.Night;
                    OnNightTime?.Invoke();
                }
            }

            else if (currentStage == CycleStage.TurningDay) {

                dayVolume.weight = Mathf.InverseLerp(0, transitionDuration, cycleTimer);
                nightVolume.weight = Mathf.InverseLerp(transitionDuration, 0, cycleTimer);

                if (cycleTimer >= transitionDuration) {
                    cycleTimer = 0f;
                    currentStage = CycleStage.Day;
                    OnDayTime?.Invoke();
                }
            }
        }
        
        public void EndNightTime() {

            if (currentStage != CycleStage.Night) {
                Debug.LogWarning("Tried Switching From Night But Not Night Time!");
                return;
            }

            cycleTimer = 0f;
            currentStage = CycleStage.TurningDay;
        }
    }

    public enum CycleStage {
        Day,
        TurningNight,
        Night,
        TurningDay
    }
}