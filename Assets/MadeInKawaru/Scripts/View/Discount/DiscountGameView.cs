using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using MadeInKawaru.Extensions;
using MadeInKawaru.View.Audio;
using MadeInKawaru.View.Interface;
using UnityEngine;

namespace MadeInKawaru.View.Discount
{
    public sealed class DiscountGameView : MonoBehaviour, IGame
    {
        [SerializeField] private DragItem _dragItem;
        [SerializeField] private RectTransform _target;
        [SerializeField] private List<RectTransform> _content;
        private bool _isClear;

        public float Time => 3f;
        public string Title => "値引きしろ！";

        public IGame Create(Transform content)
        {
            return Instantiate(this, content);
        }

        public async UniTask<bool> PlayAsync(float time, float speed, int stage, CancellationToken token = default)
        {
            _target.SetParent(_content.RandomOne());
            _target.localPosition = Vector3.zero;
            _dragItem.Initialize(_target, OnDrop);
            await UniTask.Delay(TimeSpan.FromSeconds(time / speed), cancellationToken: token);

            return _isClear;
        }

        private void OnDrop()
        {
            _isClear = true;
            AudioManager.Instance.PlayOneShot(SeName.Positive);
        }

        public void Close()
        {
            Destroy(gameObject);
        }
    }
}