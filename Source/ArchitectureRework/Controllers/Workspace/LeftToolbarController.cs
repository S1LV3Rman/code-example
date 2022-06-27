using System;

namespace Source
{
    public class LeftToolbarController
    {
        private readonly LeftToolbarView _view;
            
        public event Action OnPlay;
        public event Action OnStop;
        public event Action OnReset;

        public LeftToolbarController(LeftToolbarView view)
        {
            view.PlayButton.OnClick.AddListener(PlayPressed);
            view.ResetButton.OnClick.AddListener(ResetPressed);
            
            _view = view;
        }

        private void PlayPressed()
        {
            _view.PlayToggle.Toggle();
            
            if (_view.PlayToggle.On)
                OnPlay?.Invoke();
            else
                OnStop?.Invoke();
        }

        private void ResetPressed()
        {
            _view.PlayToggle.TurnOff();
            OnReset?.Invoke();
        }
    }
}