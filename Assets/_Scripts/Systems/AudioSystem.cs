using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IslandDefender.Audio {
    public class AudioSystem : Singleton<AudioSystem> {

        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource sfxSource;

        [SerializeField] private ScriptableSounds soundClips;
        public static ScriptableSounds SoundClips => Instance.soundClips;

        private float musicVolume = 0.5f;

        private float sfxVolume = 0.5f;

        #region Get Methods

        public float GetMusicVolume() {
            return musicVolume;
        }

        public float GetSFXVolume() {
            return sfxVolume;
        }

        #endregion

        #region Set Methods

        public void SetMusicVolume(float volume) {
            musicVolume = volume;
            musicSource.volume = musicVolume;
        }

        public void SetSFXVolume(float volume) {
            sfxVolume = volume;
        }

        public void SetWalking(bool walking) {
            this.walking = walking;
            stepTimer = float.MaxValue; // to play walk sound right when start walking
        }

        #endregion

        private void Start() {
            musicSource.clip = SoundClips.DayMusic;
            musicSource.Play();
        }

        private void OnEnable() {
            SceneManager.sceneLoaded += StopWalkingSound;
        }

        private void OnDisable() {
            SceneManager.sceneLoaded -= StopWalkingSound;
        }

        private void StopWalkingSound(Scene arg0, LoadSceneMode arg1) {
            SetWalking(false);
        }

        private void Update() {
            HandleStepAudio();
        }

        public void PlaySound(AudioClip clip, float vol = 1, float pitchRandomize = 0) {
            sfxSource.pitch = Random.Range(1f - pitchRandomize, 1f + pitchRandomize);

            sfxSource.PlayOneShot(clip, sfxVolume * vol);
        }

        public void TransitionToDayMusic() {
            float duration = 8f;
            musicSource.DOFade(0, duration).OnComplete(() => {
                musicSource.clip = SoundClips.DayMusic;
                musicSource.DOFade(GetMusicVolume(), duration).SetEase(Ease.Linear);
            });
        }

        public void TransitionToNightMusic() {
            float duration = 8f;
            musicSource.DOFade(0, duration).OnComplete(() => {
                musicSource.clip = SoundClips.NightMusic;
                musicSource.Play();
                musicSource.DOFade(GetMusicVolume(), duration).SetEase(Ease.Linear);
            });
        }


        #region Walking Sound Effect

        private bool walking;
        private float stepTimer;

        private void HandleStepAudio() {
            if (walking) {
                float stepFrequency = 0.15f;
                stepTimer += Time.deltaTime;
                if (stepTimer > stepFrequency) {
                    stepTimer = 0;
                    PlaySound(soundClips.Steps.RandomItem(), 0);
                }
            }
        }

        #endregion
    }
}
