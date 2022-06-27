using System;
using DG.Tweening;
using UnityEngine;

namespace Source
{
    public class LoadingPlaceholderController : IMenu
    {
        private readonly LoadingPlaceholderView _view;

        public bool BlockClosing => true;
        public Transform Transform => _view.transform;
        public event Action OnShow;

        public LoadingPlaceholderController(LoadingPlaceholderView view)
        {
            view.Window.OnOn.AddListener(Init);
            
            _view = view;
        }

        public void Open()
        {
            _view.Window.TurnOn();
        }

        private void Init()
        {
            _view.ImageInner.DOLocalRotate(new Vector3(0f, 0f, 360f), _view.RotationDuration, RotateMode.FastBeyond360)
                .SetLoops(-1).SetEase(Ease.Linear);
            _view.ImageOuter.DOLocalRotate(new Vector3(0f, 0f, -360f), _view.RotationDuration, RotateMode.FastBeyond360)
                .SetLoops(-1).SetEase(Ease.Linear);
            
            OnShow?.Invoke();
        }
    }
}