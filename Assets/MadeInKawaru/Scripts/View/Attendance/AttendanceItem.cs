using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MadeInKawaru.View.Attendance
{
    public sealed class AttendanceItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private Image _reaction;
        [SerializeField] private GameObject _student;
        [SerializeField] private GameObject _empty;
        private Player _player;
        private Action<Player> _callback;

        public void Initialize(Player player, string nameString, Action<Player> callback)
        {
            _player = player;
            _name.text = nameString;
            _callback = callback;
            _reaction.enabled = false;
            _student.SetActive(player != Player.Absentee);
            _empty.SetActive(player == Player.Absentee);
        }

        public async UniTaskVoid ReactionAsync(Player player, float delay)
        {
            if (_player != player) return;
            if (_player == Player.Absentee) return;
            await UniTask.Delay(TimeSpan.FromSeconds(delay));
            _reaction.enabled = true;
            _callback.Invoke(_player);
            await UniTask.Delay(TimeSpan.FromSeconds(0.3f));
            _reaction.enabled = false;
        }

        public async UniTaskVoid AttendanceAsync()
        {
            _reaction.enabled = true;
            _callback.Invoke(Player.Absentee);
            await UniTask.Delay(TimeSpan.FromSeconds(0.3f));
            _reaction.enabled = false;
        }
    }
}