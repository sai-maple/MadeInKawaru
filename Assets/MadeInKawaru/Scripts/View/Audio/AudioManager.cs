using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace MadeInKawaru.View.Audio
{
    public enum SeName
    {
        Button,

        GameIntro1,
        GameIntro2,
        Positive, // ミニゲーム正解時のピロン
        Negative,
        Correct, // ミニゲームクリア時のいえーい
        Incorrect,
        SpeedUp,
        GameOver,
    }

    public enum BgmName
    {
        None,
        MainBgm,
        GameBgm,
    }

    [RequireComponent(typeof(AudioSource))]
    public sealed class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [SerializeField] private AudioSource _audioSource;
        private readonly Dictionary<SeName, AudioClip> _audioClips = new();
        private float _volume = 0.2f;

        public void Initialize()
        {
            Instance = this;
            PlayBgm(BgmName.MainBgm);
            DontDestroyOnLoad(gameObject);
        }

        public void VolumeChanged(float value)
        {
            _audioSource.volume = value;
            _volume = value;
        }

        public void Speed(float speed)
        {
            _audioSource.pitch = speed;
        }

        public async void PlayBgm(BgmName bgm)
        {
            PlayerPrefs.SetInt("Bgm", (int)bgm);
            await _audioSource.DOFade(0, 0.5f);
            if (bgm == BgmName.None)
            {
                _audioSource.Stop();
                _audioSource.volume = _volume;
            }

            var bgmClip = await Resources.LoadAsync<AudioClip>($"AudioClips/Bgm/{bgm}") as AudioClip;
            _audioSource.clip = bgmClip;
            _audioSource.Play();
            await _audioSource.DOFade(_volume, 0.5f);
        }

        public async UniTask PlayOneShotAsync(SeName seName)
        {
            PlayOneShot(seName);
            await UniTask.Delay(TimeSpan.FromSeconds(_audioClips[seName].length));
        }

        public async void PlayOneShot(SeName seName)
        {
            if (_audioClips.ContainsKey(seName))
            {
                await UniTask.WaitUntil(() => _audioClips[seName] != null);
                _audioSource.PlayOneShot(_audioClips[seName]);
                return;
            }

            var result = await Resources.LoadAsync<AudioClip>($"AudioClips/Se/{seName}") as AudioClip;
            _audioSource.PlayOneShot(result);
            _audioClips.Add(seName, result);
        }

        private void Reset()
        {
            _audioSource = GetComponent<AudioSource>();
        }
    }
}