using System;
using DG.Tweening;
using MadeInKawaru.Entity.Common;
using MadeInKawaru.Enums;
using MadeInKawaru.View.Audio;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer.Unity;

namespace MadeInKawaru.Presenter.Menu
{
    public sealed class OptionPresenter : IInitializable, IDisposable
    {
        private readonly PhaseEntity _phaseEntity;
        private readonly CanvasGroup _canvasGroup;
        private readonly Button _button;
        private readonly CompositeDisposable _disposable = new();

        public OptionPresenter(PhaseEntity phaseEntity, CanvasGroup canvasGroup, Button button)
        {
            _phaseEntity = phaseEntity;
            _canvasGroup = canvasGroup;
            _button = button;
        }

        public void Initialize()
        {
            _phaseEntity.OnPhaseChangedAsObservable()
                .Where(phase => phase == Phase.Option)
                .Subscribe(_ =>
                {
                    _canvasGroup.interactable = true;
                    _canvasGroup.blocksRaycasts = true;
                    _canvasGroup.DOFade(1, 0.3f);
                });

            _button.OnClickAsObservable()
                .Subscribe(_ =>
                {
                    _canvasGroup.interactable = false;
                    _canvasGroup.blocksRaycasts = false;
                    _canvasGroup.DOFade(0, 0.3f);
                    _phaseEntity.OnNext(Phase.Menu);
                    AudioManager.Instance.PlayOneShot(SeName.Button);
                });
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.alpha = 0;
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}