using System;
using UnityEngine;

namespace Source
{
    public class ConfirmExitMenuController : IResultMenu<bool>
    {
        private readonly ConfirmExitMenuView _view;

        private bool _result;

        public bool BlockClosing => false;
        public Transform Transform => _view.transform;
        
        public event Action OnShow;
        public event Action<bool> OnResult;

        public ConfirmExitMenuController(ConfirmExitMenuView view)
        {
            view.Exit.OnClick.AddListener(() => Result(true));
            view.Stay.OnClick.AddListener(() => Result(false));
            
            view.Close.OnClick.AddListener(Close);
            view.Window.OnOff.AddListener(OnClose);
            view.Window.OnOn.AddListener(OnOpen);

            _result = false;
            _view = view;
        }

        public void Open()
        {
            _view.Window.TurnOn();
        }

        private void Close()
        {
            _view.Window.TurnOff();
        }

        private void Result(bool exit)
        {
            _result = exit;
            Close();
        }

        private void OnOpen()
        {
            OnShow?.Invoke();
        }

        private void OnClose()
        {
            OnResult?.Invoke(_result);
        }
    }
}