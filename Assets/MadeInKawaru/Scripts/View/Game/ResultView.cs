using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MadeInKawaru.View.Game
{
    public sealed class ResultView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private TextMeshProUGUI _result;
        [SerializeField] private Button _returnButton;
        [SerializeField] private Button _tweetButton;

        public void Initialize(Action back, Action tweet)
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
            _returnButton.onClick.AddListener(back.Invoke);
            _tweetButton.onClick.AddListener(tweet.Invoke);
        }

        public async UniTaskVoid FadeIn(int stage)
        {
            _result.text = $"ステージ{stage}\nまで到達した。";
            await _canvasGroup.DOFade(1, 0.3f);
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
        }

        public void FadeOut(float duration = 0)
        {
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.DOFade(0, duration);
        }
    }
}