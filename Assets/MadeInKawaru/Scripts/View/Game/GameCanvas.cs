using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace MadeInKawaru.View.Game
{
    public sealed class GameCanvas : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Animator _bomb;
        [SerializeField] private Slider _slider;
        private static readonly int Bomb = Animator.StringToHash("Bomb");

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
                _slider.value = Mathf.CeilToInt(time - t);
                await UniTask.Yield(cancellationToken: token);
                t += Time.deltaTime * speed;
            }

            _slider.value = 0;
            _bomb.SetTrigger(Bomb);
        }
    }
}