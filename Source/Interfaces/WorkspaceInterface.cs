using System;
using UnityEngine;

namespace Source
{
    public class WorkspaceInterface
    {
        private WorkspaceView _view;

        private RectTransform _viewTransform;

        public event Action OnClearPressed;
        public event Action OnResizeBegin;
        public event Action OnResizeEnd;
        public event Action OnResizerEnter;
        public event Action OnResizerExit;
        public bool Active { get; private set; }
        public I_BE2_ProgrammingEnv BE2Program => _view.Environment;

        public float Width
        {
            get => _viewTransform.sizeDelta.x;
            set
            {
                var size = _viewTransform.sizeDelta;
                size.x = value;
                _viewTransform.sizeDelta = size;
            }
        }

        public float ParentWidth => ((RectTransform) _view.transform.parent).rect.width;

        public WorkspaceInterface(WorkspaceView view)
        {
            _view = view;
            _viewTransform = (RectTransform) _view.transform;
            
            _view.ResizeSlider.Button.OnDown.AddListener(ResizeBegin);
            _view.ResizeSlider.Button.OnClick.AddListener(ResizeEnd);
            _view.ResizeSlider.OnEnter.AddListener(ResizerEnter);
            _view.ResizeSlider.OnExit.AddListener(ResizerExit);
            
            _view.ClearButton.OnClick.AddListener(ClearPressed);

            Active = true;
        }

        private void ClearPressed()
        {
            OnClearPressed?.Invoke();
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

        public void ClearEnvironment()
        {
            _view.Environment.ClearBlocks();
        }

        public void Open()
        {
            _view.Canvas.alpha = 1f;
            _view.Canvas.blocksRaycasts = true;

            Active = true;
        }

        public void Close()
        {
            _view.Canvas.alpha = 0f;
            _view.Canvas.blocksRaycasts = false;

            Active = false;
        }
    }
}