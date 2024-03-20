using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using MadeInKawaru.Extensions;
using MadeInKawaru.View.Interface;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace MadeInKawaru.View.Attendance
{
    /// <summary>
    /// 代返ゲーム
    /// </summary>
    public sealed class AttendanceGameView : MonoBehaviour, IGame
    {
        [SerializeField] private AttendanceItem[] _items;
        [SerializeField] private TeacherView _teacherView;
        [SerializeField] private Button _button;
        private readonly Dictionary<Player, string> _names = new();
        private bool _isClear;
        private Player _callTarget;

        public string Title => "代返しろ！";

        public IGame Create(Transform content)
        {
            return Instantiate(this, content);
        }

        public async UniTask<bool> PlayAsync(float time, float speed, int level, CancellationToken token = default)
        {
            var names = new List<string> { "田中", "山田", "鈴木" }.RandomSort().ToList();
            var index = 0;
            var interval = time / 4f;
            _names.Add(Player.Player, "あなた");
            foreach (Player player in Enum.GetValues(typeof(Player)))
            {
                if (player == Player.Player) continue;
                _names.Add(player, names[index]);
                index++;
            }

            _teacherView.Initialize(speed);
            _button.onClick.AddListener(() => OnReaction(Player.Absentee));

            foreach (var value in _names.RandomSort().Select((v, i) => new { v.Key, v.Value, i }))
            {
                _items[value.i].Initialize(value.Key, value.Value, OnReaction);
            }

            await UniTask.Delay(TimeSpan.FromSeconds(interval), cancellationToken: token);

            foreach (var (key, value) in _names.RandomSort().Where(v => v.Key != Player.Player))
            {
                _callTarget = key;
                _teacherView.Call(value);
                foreach (var item in _items)
                {
                    var delay = Random.Range(0.1f, 0.5f) / speed;
                    item.Reaction(key, delay);
                }

                await UniTask.Delay(TimeSpan.FromSeconds(interval), cancellationToken: token);
            }

            return _isClear;
        }

        private void OnReaction(Player player)
        {
            if (player != Player.Absentee) return;
            // todo Audio
            _isClear = _callTarget == Player.Absentee;
            _button.enabled = false;
        }
    }

    public enum Player
    {
        Player,
        Npc1,
        Npc2,
        Absentee,
    }
}