using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace MadeInKawaru.View.ShapeGame
{
    public sealed class ShapeItem : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private Sprite[] _shapes;
        [SerializeField] private Button _button;
        private int _shape;

        public void Initialize(int shape)
        {
            _shape = shape;
            _image.sprite = _shapes[shape];
            _image.rectTransform.sizeDelta = _shapes[shape].rect.size / 2;
        }

        public void ChangeOther(Action<int> callBack)
        {
            _shape = Enumerable.Range(0, _shapes.Length).First(shape => shape != _shape);
            _image.sprite = _shapes[_shape];
            _image.rectTransform.sizeDelta = _shapes[_shape].rect.size / 2;
            _button.onClick.AddListener(() =>
            {
                _shape++;
                _shape %= _shapes.Length;
                _image.sprite = _shapes[_shape];
                _image.rectTransform.sizeDelta = _shapes[_shape].rect.size / 2;
                callBack.Invoke(_shape);
            });
        }

    }
}