using System;
using UnityEngine;

namespace Source
{
    public class BlockSelectionInterface
    {
        private BlockSelectionView _view;
        
        private RectTransform _viewTransform;
        
        public event Action OnResizeBegin;
        public event Action OnResizeEnd;
        public event Action OnResizerEnter;
        public event Action OnResizerExit;        

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

        public BlockSelectionInterface(BlockSelectionView view)
        {
            _view = view;
            _viewTransform = (RectTransform) _view.transform;
            
            _view.ResizeSlider.Button.OnDown.AddListener(ResizeBegin);
            _view.ResizeSlider.Button.OnClick.AddListener(ResizeEnd);
            _view.ResizeSlider.OnEnter.AddListener(ResizerEnter);
            _view.ResizeSlider.OnExit.AddListener(ResizerExit);
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

        public void SetColor(Color color)
        {
            _view.Background.color = color;
        }

        public void SetHeader(string text)
        {
            _view.Header.text = text;
        }

        public void OpenSection(BlockSectionType sectionType)
        {
            if (!_view.gameObject.activeSelf)
                _view.gameObject.SetActive(true);
            
            SetSectionActive(sectionType, true);
        }

        public void CloseSection(BlockSectionType sectionType)
        {
            if (_view.gameObject.activeSelf)
                _view.gameObject.SetActive(false);
            
            SetSectionActive(sectionType, false);
        }

        public void Close()
        {
            _view.gameObject.SetActive(false);
        }

        public void SetSectionActive(BlockSectionType sectionType, bool active)
        {
            switch (sectionType)
            {
                case BlockSectionType.Controls:
                    _view.Controls.gameObject.SetActive(active);
                    break;
                case BlockSectionType.Movement:
                    _view.Movement.gameObject.SetActive(active);
                    break;
                case BlockSectionType.Rangefinder:
                    _view.Rangefinder.gameObject.SetActive(active);
                    break;
                case BlockSectionType.Sensors:
                    _view.Sensors.gameObject.SetActive(active);
                    break;
                case BlockSectionType.Indicators:
                    _view.Indicators.gameObject.SetActive(active);
                    break;
                case BlockSectionType.Operators:
                    _view.Operators.gameObject.SetActive(active);
                    break;
                case BlockSectionType.Variables:
                    _view.Variables.gameObject.SetActive(active);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(sectionType), sectionType, null);
            }
        }
    }
}