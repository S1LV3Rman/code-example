using System;
using UnityEngine;

namespace Source
{
    public class ConsoleInterface
    {
        private readonly ConsoleView _view;

        private RectTransform _viewTransform;
        
        public event Action OnResizeBegin;
        public event Action OnResizeEnd;
        public event Action OnResizerEnter;
        public event Action OnResizerExit; 

        public float Height
        {
            get => _viewTransform.sizeDelta.y;
            set
            {
                var size = _viewTransform.sizeDelta;
                size.y = value;
                _viewTransform.sizeDelta = size;
            }
        }

        public float ParentHeight => ((RectTransform) _view.transform.parent).rect.height;

        public ConsoleInterface(ConsoleView view)
        {
            _view = view;
            _viewTransform = (RectTransform) _view.transform;
            
            _view.ResizeSlider.Button.OnDown.AddListener(ResizeBegin);
            _view.ResizeSlider.Button.OnClick.AddListener(ResizeEnd);
            _view.ResizeSlider.OnEnter.AddListener(ResizerEnter);
            _view.ResizeSlider.OnExit.AddListener(ResizerExit);
        }

        public void AddMessage(string message)
        {
            var line = $"[{DateTime.Now:hh:mm:ss}] {message}\n";
            if (_view.Text.textInfo.lineCount > 100)
            {
                _view.Text.text = _view.Text.text.Remove(0, _view.Text.textInfo.lineInfo[0].characterCount) + line;
            }
            else
            {
                _view.Text.text += line;
            }
        }
        
        private void ResizerEnter()
        {
            OnResizerEnter?.Invoke();
        }

        private void ResizerExit()
        {
            OnResizerExit?.Invoke();
        }

        private void ResizeBegin()
        {
            OnResizeBegin?.Invoke();
        }

        private void ResizeEnd()
        {
            OnResizeEnd?.Invoke();
        }
    }
}