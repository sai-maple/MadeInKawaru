using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using MadeInKawaru.View.Interface;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MadeInKawaru.View.SwitchItem
{
    public sealed class SwitchItemGameView : MonoBehaviour, IGame
    {
        [SerializeField] private float _time = 5;
        [SerializeField] private Animator _animator;
        [SerializeField] private Item _itemPrefab;
        [SerializeField] private Sprite[] _items;
        [SerializeField] private Transform[] _contents;
        private bool _isClear;
        private static readonly int Hide = Animator.StringToHash("Hide");

        public async UniTask<bool> PlayAsync(float speed, int level, CancellationToken token = default)
        {
            // レベルに応じてアイテムの数を分岐
            var num = level switch
            {
                < 2 => 5, 
                < 4 => 6, 
                < 6 => 8, 
                < 8 => 9, 
                _ => 10,
            };

            var itemSprites = _items.OrderBy(_ => Guid.NewGuid()).ToList();
            var items = new List<Item>();
            var contents = _contents.OrderBy(_ => Guid.NewGuid()).ToList();
            for (var i = 0; i < num; i++)
            {
                var item = Instantiate(_itemPrefab, contents[i]);
                item.Initialize(itemSprites[i], OnItemClick);
                item.IsActive = false;
                items.Add(item);
            }

            // 暗転
            _animator.SetBool(Hide, true);
            await UniTask.Delay(TimeSpan.FromSeconds(0.3f), cancellationToken: token);

            // アイテムの入れ替え
            items[Random.Range(0, items.Count)].Switch(itemSprites.Last());

            // 暗転開け
            _animator.SetBool(Hide, false);
            await UniTask.Delay(TimeSpan.FromSeconds(0.3f), cancellationToken: token);

            for (var i = 0; i < num; i++)
            {
                items[i].IsActive = false;
            }

            await UniTask.Delay(TimeSpan.FromSeconds(_time / speed), cancellationToken: token);
            return _isClear;
        }

        private void OnItemClick(bool isCorrect)
        {
            _isClear = isCorrect;
        }
    }
}