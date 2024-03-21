using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using MadeInKawaru.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

namespace MadeInKawaru.View.Game
{
    public sealed class GameMenuView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private PlayableDirector _titleDirector;
        [SerializeField] private PlayableDirector _gameOver;
        [SerializeField] private PlayableDirector _speedUp;
        [SerializeField] private Animator _life;
        private static readonly int Life = Animator.StringToHash("Life");

        public async UniTask FadeAsync(float endValue, float duration = 0.3f, CancellationToken token = default)
        {
            await _canvasGroup.DOFade(endValue, duration).WithCancellation(token);
        }

        public async UniTask PlayAsync(string title, float speed, CancellationToken token = default)
        {
            _title.text = title;
            _titleDirector.Speed(speed);
            await _titleDirector.PlayAsync(token: token);
        }
        
        public async UniTask SpeedUpAsync(float speed, CancellationToken token = default)
        {
            _speedUp.Speed(speed);
            await _speedUp.PlayAsync(token: token);
        }

        public void LifeView(int life)
        {
            _life.SetInteger(Life, life);
        }

        public async UniTask GameOverAsync(CancellationToken token = default)
        {
            // ライフの減少演出待機
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken: token);
            await _gameOver.PlayAsync(token: token);
            FadeAsync(0, token: token).Forget();
        }
    }
}