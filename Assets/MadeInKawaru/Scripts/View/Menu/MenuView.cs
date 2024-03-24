using System;
using Cysharp.Threading.Tasks;
using MadeInKawaru.Extensions;
using MadeInKawaru.View.Audio;
using UniRx;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

namespace MadeInKawaru.View.Menu
{
    public sealed class MenuView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _rankingButton;
        [SerializeField] private Button _optionButton;
        [SerializeField] private Button _volumeButton;
        [SerializeField] private Image[] _volumeIcons;
        [SerializeField] private PlayableDirector _present;
        [SerializeField] private PlayableDirector _dismiss;
        private int _volumeIndex = 2;

        private void Start()
        {
            AudioManager.Instance.VolumeChanged(0.33f * _volumeIndex);
            for (var i = 0; i < _volumeIcons.Length; i++)
            {
                _volumeIcons[i].enabled = i == _volumeIndex;
            }
        }

        public async UniTask PresentAsync()
        {
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
            await _present.PlayAsync();
        }
        
        public async UniTask DismissAsync()
        {
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
            await _dismiss.PlayAsync();
        }

        public IObservable<Unit> OnStartAsObservable()
        {
            return _startButton.OnClickAsObservable();
        }

        public IObservable<Unit> OnRankingAsObservable()
        {
            return _rankingButton.OnClickAsObservable();
        }
        
        public IObservable<Unit> OnOptionAsObservable()
        {
            return _optionButton.OnClickAsObservable();
        }

        public IObservable<int> OnVolumeAsObservable()
        {
            return _volumeButton.OnClickAsObservable().Select(_ => OnVolumeChange());
        }

        private int OnVolumeChange()
        {
            _volumeIndex++;
            if (_volumeIndex >= _volumeIcons.Length) _volumeIndex = 0;
            for (var i = 0; i < _volumeIcons.Length; i++)
            {
                _volumeIcons[i].enabled = i == _volumeIndex;
            }

            return _volumeIndex;
        }
    }
}