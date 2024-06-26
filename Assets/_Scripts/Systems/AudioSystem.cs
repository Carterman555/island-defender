using DG.Tweening;
using UnityEngine;

namespace IslandDefender.Audio {
    public class AudioSystem : Singleton<AudioSystem> {

        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource sfxSource;

        [SerializeField] private ScriptableSounds soundClips;
        public static ScriptableSounds SoundClips => Instance.soundClips;

        private float musicVolume = 0.1f;
        private float sfxVolume = 0.3f;

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

        #endregion

        private void Start() {
            musicSource.clip = SoundClips.DayMusic;
            musicSource.Play();
        }

        public void PlaySound(AudioClip clip, float vol = 1, float pitchRandomize = 0) {
            sfxSource.pitch = Random.Range(1f - pitchRandomize, 1f + pitchRandomize);

            sfxSource.PlayOneShot(clip, sfxVolume * vol);
        }

        [SerializeField] private AudioSource rainSource;
        private float rainVolume = 0.2f;

        public void PlayRain() {
            rainSource.Play();
            rainSource.volume = 0;

            float duration = 2f;
            rainSource.DOFade(rainVolume, duration);
        }

        public void StopRain() {

            rainSource.volume = rainVolume;

            float duration = 2f;
            rainSource.DOFade(0, duration).OnComplete(() => {
                rainSource.Stop();
            });
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
    }
}
