using System;
using Cysharp.Threading.Tasks;
using MadeInKawaru.Entity.Common;
using MadeInKawaru.Enums;
using MadeInKawaru.View.Audio;
using MadeInKawaru.View.Menu;
using UniRx;
using VContainer.Unity;

namespace MadeInKawaru.Presenter.Menu
{
    public sealed class MenuPresenter : IInitializable, IDisposable
    {
        private readonly PhaseEntity _phaseEntity;
        private readonly MenuView _menuView;

        private readonly CompositeDisposable _disposable = new();

        public MenuPresenter(PhaseEntity phaseEntity, MenuView menuView)
        {
            _phaseEntity = phaseEntity;
            _menuView = menuView;
        }

        public void Initialize()
        {
            _phaseEntity.OnPhaseChangedAsObservable()
                .Where(phase => phase == Phase.Menu)
                .Subscribe(_ => _menuView.PresentAsync().Forget())
                .AddTo(_disposable);

            _menuView.OnStartAsObservable()
                .Subscribe(_ =>
                {
                    UniTask.Create(async () =>
                    {
                        AudioManager.Instance.PlayOneShot(SeName.GameStart);
                        AudioManager.Instance.PlayBgm(BgmName.None);
                        await _menuView.DismissAsync();
                        _phaseEntity.OnNext(Phase.Game);
                    }).Forget();
                })
                .AddTo(_disposable);

            _menuView.OnRankingAsObservable()
                .Subscribe(_ =>
                {
                    AudioManager.Instance.PlayOneShot(SeName.Button);
                    _phaseEntity.OnNext(Phase.Ranking);
                })
                .AddTo(_disposable);

            _menuView.OnOptionAsObservable()
                .Subscribe(_ =>
                {
                    AudioManager.Instance.PlayOneShot(SeName.Button);
                    _phaseEntity.OnNext(Phase.Option);
                })
                .AddTo(_disposable);

            _menuView.OnVolumeAsObservable()
                .Subscribe(volume =>
                {
                    AudioManager.Instance.VolumeChanged(0.33f * volume);
                })
                .AddTo(_disposable);
        }

        public void Dispose()
        {
            _phaseEntity?.Dispose();
            _disposable?.Dispose();
        }
    }
}