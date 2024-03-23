using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MadeInKawaru.View.SwitchItem
{
    /// <summary>
    /// 入れ替わりゲームのアイテム。暗転時に1種類だけイラストが入れ替わる。
    /// </summary>
    public sealed class Item : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image _image;
        [SerializeField] private Animator _correct;
        private bool _isSwitch;
        private Action<bool> _callback;

        public bool IsActive
        {
            get => _image.raycastTarget;
            set => _image.raycastTarget = value;
        }

        public void Initialize(Sprite item, Action<bool> callback)
        {
            _image.rectTransform.sizeDelta = item.rect.size;
            _image.sprite = item;
            _isSwitch = false;
            IsActive = false;
            _callback = callback;
        }

        public void Switch(Sprite item)
        {
            _image.rectTransform.sizeDelta = item.rect.size;
            _image.sprite = item;
            _isSwitch = true;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!IsActive) return;
            _callback?.Invoke(_isSwitch);
            _correct.SetTrigger(_isSwitch ? "Correct" : "Incorrect");
            // todo AudioManager
        }
    }
}