using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using MadeInKawaru.View.Audio;
using MadeInKawaru.View.Interface;
using UnityEngine;
using Random = UnityEngine.Random;

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

        public string Title => "咲かせろ！";

        public IGame Create(Transform content)
        {
            return Instantiate(this, content);
        }

        public async UniTask<bool> PlayAsync(float time, float speed, int stage, CancellationToken token = default)
        {
            _animator.SetBool(Bloom, false);
            var flower = _target.localPosition;
            flower.x = Random.Range(0, 2) == 0 ? 350 : -350;
            _target.localPosition = flower;
            _cloudView.Initialize(flower.x, OnRain);
            await UniTask.Delay(TimeSpan.FromSeconds(time / speed), cancellationToken: token);
            return _isClear;
        }

        public void Close()
        {
            Destroy(gameObject);
        }

        private void OnRain()
        {
            _animator.SetBool(Bloom, true);
            _isClear = true;
            AudioManager.Instance.PlayOneShot(SeName.Positive);
        }
    }
}