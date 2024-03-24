using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using MadeInKawaru.View.Audio;
using MadeInKawaru.View.Interface;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace MadeInKawaru.View.LightGame
{
    public sealed class LightGameView : MonoBehaviour, IGame
    {
        [SerializeField] private Button _button;
        [SerializeField] private Image _light;
        [SerializeField] private Animator _lightAnime;
        private int _lightIndex;
        private static readonly int Light = Animator.StringToHash("Light");

        public float Time => 3f;
        public string Title => "灯りを消せ！";

        public IGame Create(Transform content)
        {
            return Instantiate(this, content);
        }

        public async UniTask<bool> PlayAsync(float time, float speed, int stage, CancellationToken token = default)
        {
            _lightIndex = Random.Range(0, 2);
            _lightAnime.SetInteger(Light, _lightIndex);
            _light.DOFade(_lightIndex * 0.33f, 0).ToUniTask(cancellationToken: token).Forget();
            _button.onClick.AddListener(() =>
            {
                _lightIndex++;
                _lightIndex %= 3;
                _light.DOFade(_lightIndex * 0.4f, 0);
                _lightAnime.SetInteger(Light, _lightIndex);
                AudioManager.Instance.PlayOneShot(_lightIndex == 2 ? SeName.Positive : SeName.Button);
            });

            await UniTask.Delay(TimeSpan.FromSeconds(time / speed), cancellationToken: token);

            return _lightIndex == 2;
        }

        public void Close()
        {
            Destroy(gameObject);
        }
    }
}