using IslandDefender.Audio;
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

        [Header("Sprites")]
        [SerializeField] private SpriteRenderer[] daySpriteRenderers;
        [SerializeField] private SpriteRenderer[] nightSpriteRenderers;

        private float cycleTimer;

        private CycleStage currentStage;

        void Start() {
            cycleTimer = 0f;
            dayVolume.weight = 1f;
            nightVolume.weight = 0f;

            currentStage = CycleStage.Day;

            SetRenderersFade(daySpriteRenderers, 1);
            SetRenderersFade(nightSpriteRenderers, 0);
        }

        void Update() {

            cycleTimer += Time.deltaTime;

            if (currentStage == CycleStage.Day) {
                if (cycleTimer >= dayDuration) {
                    cycleTimer = 0f;
                    currentStage = CycleStage.TurningNight;

                    ChangeSpriteOrdering(nightSpriteRenderers, daySpriteRenderers);
                    SetRenderersFade(nightSpriteRenderers, 1);

                    AudioSystem.Instance.TransitionToNightMusic();

                    OnTurningNightTime?.Invoke();
                }
            }

            else if (currentStage == CycleStage.TurningNight) {

                float dayFade = Mathf.InverseLerp(transitionDuration, 0, cycleTimer);
                SetRenderersFade(daySpriteRenderers, dayFade);

                if (cycleTimer >= transitionDuration) {
                    cycleTimer = 0f;
                    currentStage = CycleStage.Night;
                    OnNightTime?.Invoke();
                }
            }

            else if (currentStage == CycleStage.TurningDay) {

                float nightFade = Mathf.InverseLerp(transitionDuration, 0, cycleTimer);
                SetRenderersFade(nightSpriteRenderers, nightFade);

                if (cycleTimer >= transitionDuration) {
                    cycleTimer = 0f;
                    currentStage = CycleStage.Day;
                    OnDayTime?.Invoke();
                }
            }
        }

        [ContextMenu("End night")]
        public void EndNightTime() {

            if (currentStage != CycleStage.Night) {
                Debug.LogWarning("Tried Switching From Night But Not Night Time!");
                return;
            }

            cycleTimer = 0f;
            currentStage = CycleStage.TurningDay;

            ChangeSpriteOrdering(daySpriteRenderers, nightSpriteRenderers);
            SetRenderersFade(daySpriteRenderers, 1);

            AudioSystem.Instance.TransitionToDayMusic();
        }

        private void SetRenderersFade(SpriteRenderer[] spriteRenderers, float fade) {
            foreach (SpriteRenderer renderer in spriteRenderers) {
                renderer.Fade(fade);
            }
        }

        private void ChangeSpriteOrdering(SpriteRenderer[] backRenderers, SpriteRenderer[] frontRenderers) {

            int order = 0;
            foreach (SpriteRenderer renderer in backRenderers) {
                renderer.sortingOrder = order;
                order += 2;
            }

            order = 1;
            foreach (SpriteRenderer renderer in frontRenderers) {
                renderer.sortingOrder = order;
                order += 2;
            }
        }
    }

    public enum CycleStage {
        Day,
        TurningNight,
        Night,
        TurningDay
    }
}