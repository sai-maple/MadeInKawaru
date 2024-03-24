using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using MadeInKawaru.Extensions;
using MadeInKawaru.View.Audio;
using MadeInKawaru.View.Interface;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MadeInKawaru.View.ShapeGame
{
    public sealed class ShapeGameView : MonoBehaviour, IGame
    {
        [SerializeField] private List<ShapeItem> _shapeItems;
        private int _shape;
        private bool _isClear;

        public float Time => 3f;
        public string Title => "揃えろ！";

        public IGame Create(Transform content)
        {
            return Instantiate(this, content);
        }

        public async UniTask<bool> PlayAsync(float time, float speed, int stage, CancellationToken token = default)
        {
            _shape = Random.Range(0, 4);
            foreach (var item in _shapeItems)
            {
                item.Initialize(_shape);
            }

            var target = _shapeItems.RandomOne();
            target.ChangeOther(shape =>
            {
                _isClear = _shape == shape;
                // todo カチッ的なSE
                var se = _isClear ? SeName.Positive : SeName.Button;
                AudioManager.Instance.PlayOneShot(se);
            });

            await UniTask.Delay(TimeSpan.FromSeconds(time / speed), cancellationToken: token);
            return _isClear;
        }

        public void Close()
        {
            Destroy(gameObject);
        }
    }
}