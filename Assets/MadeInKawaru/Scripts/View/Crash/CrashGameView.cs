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

        public async UniTask<bool> PlayAsync(float time, float speed, int level, CancellationToken token = default)
        {
            foreach (var item in _items)
            {
                _result.Add(item, false);
                var itemSpeed = level switch
                {
                    < 2 => GetSpeed(0.5f, 2),
                    < 4 => GetSpeed(1f, 2.5f),
                    < 6 => GetSpeed(1f, 3f),
                    < 8 => GetSpeed(1f, 4f),
                    _ => GetSpeed(1f, 5f),
                };
                item.Initialize(itemSpeed, OnCrash);
            }

            await UniTask.Delay(TimeSpan.FromSeconds(time / speed), cancellationToken: token);
            return _result.Values.All(b => b);
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