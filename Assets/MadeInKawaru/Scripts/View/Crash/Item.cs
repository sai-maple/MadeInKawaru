using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

namespace MadeInKawaru.View.Crash
{
    /// <summary>
    /// 「か」を割るゲーム。ランダムに動く「か」のView
    /// </summary>
    public sealed class Item : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private Collider2D _collider;
        [SerializeField] private Animator _animator;
        private Action<Item> _callback;
        private bool _isCrash;
        private static readonly int Crash = Animator.StringToHash("Crash");

        public void Initialize(float speed, Action<Item> callback)
        {
            // ランダムな方向に移動開始。摩擦は0に設定している
            _rigidbody.AddForce(Random.insideUnitCircle.normalized * speed);
            _callback = callback;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_isCrash) return;
            _isCrash = true;
            _rigidbody.velocity = Vector2.zero;
            _animator.SetBool(Crash, true);
            _collider.enabled = false;
            _callback?.Invoke(this);
        }

        private void Reset()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _collider = GetComponent<Collider2D>();
            _animator = GetComponent<Animator>();
        }
    }
}