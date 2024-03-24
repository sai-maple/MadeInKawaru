using System;
using MadeInKawaru.Extensions;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MadeInKawaru.View.Discount
{
    public sealed class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler
    {
        [SerializeField] private RectTransform _rectTransform;
        private RectTransform _parent;
        private Camera _mainCamera;
        private Vector2 _startTap;
        private Vector2 _start;
        private RectTransform _target;
        private Action _callback;

        private bool _isLock;

        public void Initialize(RectTransform target, Action callBack)
        {
            _parent = (RectTransform)_rectTransform.parent;
            _mainCamera = Camera.main;
            _target = target;
            _callback = callBack;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(_parent, eventData.position, _mainCamera,
                    out var position))
            {
                return;
            }

            _startTap = position;
            _start = _rectTransform.localPosition;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_isLock) return;
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(_parent, eventData.position, _mainCamera,
                    out var position))
            {
                return;
            }

            var diff = position - _startTap;
            _rectTransform.localPosition = _start + diff;
            // かさなり
            if (Vector3.Distance(_rectTransform.position, _target.position) > 0.2f) return;
            _isLock = true;
            _callback.Invoke();
        }

        private void Reset()
        {
            _rectTransform = GetComponent<RectTransform>();
        }
    }
}