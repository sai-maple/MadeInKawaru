using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using MadeInKawaru.Extensions;
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
        private readonly List<IGame> _games;
        private readonly RectTransform _content;
        private readonly CompositeDisposable _disposable = new();
        private readonly CancellationTokenSource _cancellation = new();


        public void Initialize()
        {
        }

        private async UniTaskVoid PlayAsync()
        {
            // todo ライフが残っているかの判定
            var i = 0;
            while (i < 5)
            {
                // イントロ
                // ゲームタイトル
                // 拡大
                var game = _games.RandomOne().Create(_content);
                // タイマーに時間を
                var result = await game.PlayAsync(5, 1, 1, _cancellation.Token);
                // クリアの可否で演出の分岐
                i++;
            }
            
             // ゲームオーバー
             // タイトルに戻る
        }

        public void Dispose()
        {
            _disposable.Dispose();
            _cancellation.Cancel();
            _cancellation.Dispose();
        }
    }
}