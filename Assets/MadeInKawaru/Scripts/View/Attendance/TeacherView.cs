using MadeInKawaru.Extensions;
using TMPro;
using UnityEngine;

namespace MadeInKawaru.View.Attendance
{
    public sealed class TeacherView : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private TextMeshProUGUI _name;
        private static readonly int CallHash = Animator.StringToHash("Call");

        public void Initialize(float speed)
        {
            _animator.Speed(speed);
        }

        public void Call(string player)
        {
            _name.text = player;
            _animator.SetTrigger(CallHash);
        }
    }
}