using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MadeInKawaru.View.Flower
{
    /// <summary>
    /// 雨を降らせる雲のView、左右にドラッグできる
    /// </summary>
    public sealed class CloudView : MonoBehaviour, IBeginDragHandler, IDragHandler
    {
        [SerializeField] private RectTransform _rectTransform;
        private Vector2 _startTap;
        private Vector3 _start;
        private float _targetY;
        private Action _callback;

        private bool _isLock;

        public void Initialize(float targetY, Action callBack)
        {
            _targetY = targetY;
            _callback = callBack;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _startTap = eventData.position;
            _start = _rectTransform.transform.localPosition;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_isLock) return;
            var delta = eventData.position - _startTap;
            var position = _start;
            position.y += delta.y;
            _rectTransform.transform.localPosition = position;
            if (Mathf.Abs(position.y - _targetY) > 10) return;
            _isLock = true;
            _callback.Invoke();
        }

        private void Reset()
        {
            _rectTransform = GetComponent<RectTransform>();
        }
    }
}