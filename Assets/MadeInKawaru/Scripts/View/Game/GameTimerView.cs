using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace MadeInKawaru.View.Game
{
    public sealed class GameTimerView : MonoBehaviour
    {
        [SerializeField] private RectTransform _content;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Animator _timer;
        private static readonly int Timer = Animator.StringToHash("Timer");

        public RectTransform Transform => _content;

        public async UniTask FadeAsync(float endValue, float duration = 0.3f, CancellationToken token = default)
        {
            await _canvasGroup.DOFade(endValue, duration).WithCancellation(token);
        }

        public async UniTaskVoid TimerAsync(float time, float speed, CancellationToken token = default)
        {
            var divide = time / 5f;
            var t = 0f;
            while (t < time)
            {
                _timer.SetInteger(Timer, Mathf.CeilToInt((time - t) / divide));
                await UniTask.Yield(cancellationToken: token);
                t += Time.deltaTime * speed;
            }

            _timer.SetInteger(Timer, 0);
        }
    }
}