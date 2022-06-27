using System;

namespace Source
{
    public class TopToolbarInterface
    {
        private TopToolbarView _view;

        public event Action OnBack;
        public event Action OnQuitClick;
        public event Action OnOpenClick;
        public event Action OnAddClick;
        public event Action OnSaveAsClick;
        public event Action OnSaveClick;

        public TopToolbarInterface(TopToolbarView view)
        {
            _view = view;
            
            _view.BackButton.OnClick.AddListener(Back);
            _view.QuitButton.OnClick.AddListener(QuitClick);
            
            _view.OpenButton.OnClick.AddListener(OpenClick);
            _view.AddButton.OnClick.AddListener(AddClick);
            _view.SaveButton.OnClick.AddListener(SaveClick);
            _view.SaveAsButton.OnClick.AddListener(SaveAsClick);
        }

        private void Back()
        {
            OnBack?.Invoke();
        }
        
        private void AddClick()
        {
            _view.FileButton.TurnOff();
            OnAddClick?.Invoke();
        }
        
        private void OpenClick()
        {
            _view.FileButton.TurnOff();
            OnOpenClick?.Invoke();
        }
        
        private void SaveClick()
        {
            _view.FileButton.TurnOff();
            OnSaveClick?.Invoke();
        }
        private void SaveAsClick()
        {
            _view.FileButton.TurnOff();
            OnSaveAsClick?.Invoke();
        }

        private void QuitClick()
        {
            OnQuitClick?.Invoke();
        }
    }
}