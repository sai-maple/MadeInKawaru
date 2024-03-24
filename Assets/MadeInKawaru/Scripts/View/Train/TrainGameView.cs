using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using MadeInKawaru.Extensions;
using MadeInKawaru.View.Audio;
using MadeInKawaru.View.Interface;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace MadeInKawaru.View.Train
{
    public sealed class TrainGameView : MonoBehaviour, IGame
    {
        [SerializeField] private Animator _train;
        [SerializeField] private Animator _wheel;
        [SerializeField] private Button _button;
        [SerializeField] private Image _switchingOff;
        [SerializeField] private Image _switchingOn;
        private bool _isClear;
        private static readonly int Straight = Animator.StringToHash("Straight");

        public string Title => "切り替えろ！";

        public IGame Create(Transform content)
        {
            return Instantiate(this, content);
        }

        public async UniTask<bool> PlayAsync(float time, float speed, int stage, CancellationToken token = default)
        {
            _isClear = Random.Range(0, 2) == 0;
            _switchingOn.enabled = _isClear;
            _switchingOff.enabled = !_isClear;
            var t = time / 2f;
            _train.Speed(speed);
            _wheel.Speed(speed);
            _train.SetBool(Straight, _isClear);
            _button.onClick.AddListener(OnClick);
            await UniTask.Delay(TimeSpan.FromSeconds(t / speed), cancellationToken: token);
            var se = _isClear ? SeName.Positive : SeName.Negative;
            AudioManager.Instance.PlayOneShotAsync(se).Forget();
            _button.onClick.RemoveAllListeners();
            await UniTask.Delay(TimeSpan.FromSeconds(t / speed), cancellationToken: token);
            return _isClear;
        }

        private void OnClick()
        {
            _isClear = !_isClear;
            _switchingOn.enabled = _isClear;
            _switchingOff.enabled = !_isClear;
            _train.SetBool(Straight, _isClear);
            _button.onClick.RemoveAllListeners();
        }

        public void Close()
        {
            Destroy(gameObject);
        }
    }
}