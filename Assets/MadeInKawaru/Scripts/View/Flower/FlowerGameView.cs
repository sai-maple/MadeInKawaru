using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using MadeInKawaru.View.Interface;
using UnityEngine;

namespace MadeInKawaru.View.Flower
{
    /// <summary>
    /// 雨雲を動かして花を咲かせるゲーム
    /// </summary>
    public sealed class FlowerGameView : MonoBehaviour, IGame
    {
        [SerializeField] private CloudView _cloudView;
        [SerializeField] private RectTransform _target;
        [SerializeField] private Animator _animator;
        private bool _isClear;

        private static readonly int Bloom = Animator.StringToHash("Bloom");

        public async UniTask<bool> PlayAsync(float time, float speed, int level, CancellationToken token = default)
        {
            _cloudView.Initialize(_target.localPosition.y, OnRain);
            await UniTask.Delay(TimeSpan.FromSeconds(time), cancellationToken: token);
            return _isClear;
        }

        private void OnRain()
        {
            _animator.SetTrigger(Bloom);
            _isClear = true;
            // todo audio
        }
    }
}