using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace MadeInKawaru.View.Game
{
    public sealed class GameTimerView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Animator _timer;
        private static readonly int Timer = Animator.StringToHash("Timer");

        public Transform Transform => transform;

        public async UniTask FadeAsync(float endValue, float duration = 0.3f, CancellationToken token = default)
        {
            await _canvasGroup.DOFade(endValue, duration).WithCancellation(token);
        }

        public async UniTaskVoid TimerAsync(float time, float speed, CancellationToken token = default)
        {
            var t = 0f;
            while (t < time)
            {
                _timer.SetInteger(Timer, Mathf.CeilToInt(time - t));
                await UniTask.Yield(cancellationToken: token);
                t += Time.deltaTime * speed;
            }

            _timer.SetInteger(Timer, 0);
        }
    }
}