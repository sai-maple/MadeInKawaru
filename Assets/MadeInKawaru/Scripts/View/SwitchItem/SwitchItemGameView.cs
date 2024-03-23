using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using MadeInKawaru.Extensions;
using MadeInKawaru.View.Interface;
using UnityEngine;

namespace MadeInKawaru.View.SwitchItem
{
    /// <summary>
    /// 暗転中にアイテムが入れ替わるゲーム
    /// </summary>
    public sealed class SwitchItemGameView : MonoBehaviour, IGame
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private Item _itemPrefab;
        [SerializeField] private Sprite[] _itemSprites;
        [SerializeField] private Transform[] _contents;
        private List<Item> _items = new();
        private bool _isClear;
        private static readonly int Hide = Animator.StringToHash("Hide");

        public string Title => "どれが変わった？";

        public IGame Create(Transform content)
        {
            return Instantiate(this, content);
        }

        public async UniTask<bool> PlayAsync(float time, float speed, int stage, CancellationToken token = default)
        {
            // レベルに応じてアイテムの数を分岐
            var num = stage switch
            {
                < 5 => 5,
                < 10 => 6,
                < 15 => 7,
                < 20 => 8,
                _ => 9,
            };

            var itemSprites = _itemSprites.OrderBy(_ => Guid.NewGuid()).ToList();
            _items = new List<Item>();
            var contents = _contents.OrderBy(_ => Guid.NewGuid()).ToList();
            for (var i = 0; i < num; i++)
            {
                var item = Instantiate(_itemPrefab, contents[i]);
                item.Initialize(itemSprites[i], OnItemClick);
                item.IsActive = false;
                _items.Add(item);
            }
            
            await UniTask.Delay(TimeSpan.FromSeconds(1f / speed), cancellationToken: token);

            // 暗転
            _animator.SetBool(Hide, true);
            _animator.Speed(speed);
            await UniTask.Delay(TimeSpan.FromSeconds(0.3f / speed), cancellationToken: token);

            // アイテムの入れ替え
            _items.RandomOne().Switch(itemSprites.Last());
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f / speed), cancellationToken: token);

            // 暗転開け
            _animator.SetBool(Hide, false);
            await UniTask.Delay(TimeSpan.FromSeconds(0.3f / speed), cancellationToken: token);

            for (var i = 0; i < num; i++)
            {
                _items[i].IsActive = true;
            }

            await UniTask.Delay(TimeSpan.FromSeconds((time - 2.1f) / speed), cancellationToken: token);
            return _isClear;
        }

        public void Close()
        {
            Destroy(gameObject);
        }

        private void OnItemClick(bool isCorrect)
        {
            _isClear = isCorrect;
            foreach (var item in _items)
            {
                item.IsActive = false;
            }
        }
    }
}