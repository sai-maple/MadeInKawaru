using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace MadeInKawaru.View.Attendance
{
    public sealed class AttendanceItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private Animator _animator;
        private Player _player;
        private Action<Player> _callback;
        private static readonly int ReactionHash = Animator.StringToHash("Reaction");

        public void Initialize(Player player, string nameString, Action<Player> callback)
        {
            _player = player;
            _name.text = nameString;
            _callback = callback;
        }

        public async void Reaction(Player player, float delay)
        {
            if (_player != player) return;
            await UniTask.Delay(TimeSpan.FromSeconds(delay));
            _animator.SetTrigger(ReactionHash);
            _callback.Invoke(_player);
        }
    }
}