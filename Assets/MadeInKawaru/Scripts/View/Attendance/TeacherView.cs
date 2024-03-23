using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace MadeInKawaru.View.Attendance
{
    public sealed class TeacherView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _name;

        private void Awake()
        {
            _name.enabled = false;
        }

        public async UniTaskVoid CallAsync(string player, float speed, CancellationToken token)
        {
            _name.text = $"\\{player}/";
            _name.enabled = true;
            await UniTask.Delay(TimeSpan.FromSeconds(1f / speed), cancellationToken: token);
            _name.enabled = false;
        }
    }
}