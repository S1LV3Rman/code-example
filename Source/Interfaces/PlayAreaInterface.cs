using System;

namespace Source
{
    public class PlayAreaInterface
    {
        private PlayAreaView _view;

        private DropdownButtonVariant _currentMode;

        public float CameraMoveDamping => _view.CameraMoveDamping;
        public float CameraMoveSensitivity => _view.CameraMoveSensitivity;
        public float CameraRotateDamping => _view.CameraRotateDamping;
        public float BotMoveDamping => _view.BotMoveDamping;
        public float BotRotateDamping => _view.BotRotateDamping;
        public float BotRotateSensitivity => _view.BotRotateSensitivity;

        public event Action OnZoomIn;
        public event Action OnZoomOut;
        public event Action OnFocus;
        
        public event Action OnFreeMode;
        public event Action OnTopDownMode;
        public event Action OnFollowMode;

        public event Action OnModesEnter;
        public event Action OnModesExit;

        public event Action OnModesClick;
        
        public PlayAreaInterface(PlayAreaView view)
        {
            _view = view;

            _currentMode = _view.FreeMode;
            
            _view.ZoomIn.OnClick.AddListener(ZoomIn);
            _view.ZoomOut.OnClick.AddListener(ZoomOut);
            _view.Focus.OnClick.AddListener(Focus);
            
            _view.FreeMode.Button.OnClick.AddListener(FreeModeClick);
            _view.TopDownMode.Button.OnClick.AddListener(TopDownModeClick);
            
            _view.FreeMode.Toggle.OnOn.AddListener(FreeMode);
            _view.TopDownMode.Toggle.OnOn.AddListener(TopDownMode);
            
            _view.FollowMode.OnOn.AddListener(FollowMode);
            _view.FollowMode.OnOff.AddListener(FollowMode);

            _view.Modes.OnEnter.AddListener(ModesEnter);
            _view.Modes.OnExit.AddListener(ModesExit);
        }

        public void CollapseModes()
        {
            _currentMode.Toggle.TurnOff();
            _view.Modes.Toggle.TurnOff();
        }

        public void ExpandModes()
        {
            _currentMode.Toggle.TurnOn();
            _view.Modes.Toggle.TurnOn();
        }

        public void OffFollow()
        {
            _view.FollowMode.TurnOff();
        }

        private void FreeModeClick()
        {
            if (_view.Modes.Toggle.On)
            {
                _view.FreeMode.Toggle.TurnOn();
            }
            else
            {
                OnModesClick?.Invoke();
            }
        }

        private void TopDownModeClick()
        {
            if (_view.Modes.Toggle.On)
            {
                _view.TopDownMode.Toggle.TurnOn();
            }
            else
            {
                OnModesClick?.Invoke();
            }
        }

        private void FreeMode()
        {
            if (_currentMode == _view.FreeMode) return;
            
            _currentMode = _view.FreeMode;
            _view.FreeMode.transform.SetAsLastSibling();
            OnFreeMode?.Invoke();
        }

        private void TopDownMode()
        {
            if (_currentMode == _view.TopDownMode) return;

            _currentMode = _view.TopDownMode;
            _view.TopDownMode.transform.SetAsLastSibling();
            OnTopDownMode?.Invoke();
        }

        private void FollowMode()
        {
            OnFollowMode?.Invoke();
        }

        private void ZoomIn()
        {
            OnZoomIn?.Invoke();
        }

        private void ZoomOut()
        {
            OnZoomOut?.Invoke();
        }

        private void Focus()
        {
            OnFocus?.Invoke();
        }

        private void ModesEnter()
        {
            OnModesEnter?.Invoke();
        }

        private void ModesExit()
        {
            OnModesExit?.Invoke();
        }
    }
}