using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using MadeInKawaru.Entity.Common;
using MadeInKawaru.Entity.Game;
using MadeInKawaru.Enums;
using MadeInKawaru.Extensions;
using MadeInKawaru.View.Game;
using MadeInKawaru.View.Interface;
using UniRx;
using UnityEngine;
using VContainer.Unity;

namespace MadeInKawaru.Presenter.Game
{
    /// <summary>
    /// ランダムにゲームを生成するPresenter
    /// </summary>
    public sealed class GamePresenter : IInitializable, IDisposable
    {
        private readonly PhaseEntity _phaseEntity;
        private readonly StageEntity _stageEntity;
        private readonly LifeEntity _lifeEntity;
        private readonly List<IGame> _games;
        private readonly GameTimerView _gameTimerView;
        private readonly GameMenuView _gameMenuView;
        private readonly CanvasGroup _canvasGroup;
        private readonly CompositeDisposable _disposable = new();
        private readonly CancellationTokenSource _cancellation = new();

        public GamePresenter(PhaseEntity phaseEntity, StageEntity stageEntity, LifeEntity lifeEntity, List<IGame> games,
            GameTimerView gameTimerView, GameMenuView gameMenuView, CanvasGroup canvasGroup)
        {
            _phaseEntity = phaseEntity;
            _stageEntity = stageEntity;
            _lifeEntity = lifeEntity;
            _games = games;
            _gameTimerView = gameTimerView;
            _gameMenuView = gameMenuView;
            _canvasGroup = canvasGroup;
        }

        public void Initialize()
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
            _phaseEntity.OnPhaseChangedAsObservable()
                .Where(phase => phase == Phase.Game)
                .Subscribe(_ => PlayAsync().Forget())
                .AddTo(_disposable);
        }

        private async UniTaskVoid PlayAsync()
        {
            _canvasGroup.alpha = 1;
            _stageEntity.Initialize();
            _lifeEntity.Initialize();
            _gameMenuView.LifeView(_lifeEntity.Life);
            _gameTimerView.FadeAsync(0, 0).Forget();
            await _gameMenuView.FadeAsync(1, token: _cancellation.Token);
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;

            while (_lifeEntity.IsLiving)
            {
                // todo イントロ
                // スピードアップ 演出
                if (_stageEntity.IsSpeedUp)
                {
                    await _gameMenuView.SpeedUpAsync(_stageEntity.Speed);
                }

                var game = _games.RandomOne().Create(_gameTimerView.Transform);

                // ステージ
                var stage = _stageEntity.Stage;
                // ゲームタイトル
                await _gameMenuView.PlayAsync(game.Title, _stageEntity.Speed, _cancellation.Token);
                // 拡大 ゲーム用Canvasの表示
                _gameTimerView.FadeAsync(1f).Forget();
                _gameMenuView.FadeAsync(0, token: _cancellation.Token).Forget();

                // タイマーに時間を
                _gameTimerView.TimerAsync(5, _stageEntity.Speed, _cancellation.Token).Forget();
                var result = await game.PlayAsync(5, _stageEntity.Speed, stage, _cancellation.Token);
                // タイマーの爆発演出待機
                await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken: _cancellation.Token);
                // ゲーム終了
                UniTask.Create(async () =>
                {
                    _gameMenuView.FadeAsync(1, token: _cancellation.Token).Forget();
                    await _gameTimerView.FadeAsync(0);
                    game.Close();
                }).Forget();

                // クリアの可否で演出の分岐
                _stageEntity.OnClear(result);
                _lifeEntity.OnClear(result);
                _gameMenuView.LifeView(_lifeEntity.Life);
            }

            // ゲームオーバー
            await _gameMenuView.GameOverAsync(_cancellation.Token);
            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
            // メニューに戻る
            _phaseEntity.OnNext(Phase.Menu);
        }

        public void Dispose()
        {
            _disposable.Dispose();
            _cancellation.Cancel();
            _cancellation.Dispose();
        }
    }
}