using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using MadeInKawaru.View.Interface;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MadeInKawaru.View.Crash
{
    /// <summary>
    /// 「か」を割るゲーム
    /// </summary>
    public sealed class CrashGameView : MonoBehaviour, IGame
    {
        [SerializeField] private Item[] _items;
        private readonly Dictionary<Item, bool> _result = new();
        public string Title => "割れ！";

        public IGame Create(Transform content)
        {
            return Instantiate(this, content);
        }

        public async UniTask<bool> PlayAsync(float time, float speed, int stage, CancellationToken token = default)
        {
            foreach (var item in _items)
            {
                _result.Add(item, false);
                var itemSpeed = stage switch
                {
                    < 5 => GetSpeed(5f, 8f),
                    < 10 => GetSpeed(5f, 10f),
                    < 15 => GetSpeed(5f, 12f),
                    < 20 => GetSpeed(5f, 14f),
                    _ => GetSpeed(5f, 16f),
                };
                item.Initialize(itemSpeed, OnCrash);
            }

            await UniTask.Delay(TimeSpan.FromSeconds(time), cancellationToken: token);
            return _result.Values.All(b => b);
        }

        public void Close()
        {
            Destroy(gameObject);
        }

        private static float GetSpeed(float min, float max)
        {
            return Random.Range(min, max);
        }

        private void OnCrash(Item sender)
        {
            _result[sender] = true;
        }
    }
}