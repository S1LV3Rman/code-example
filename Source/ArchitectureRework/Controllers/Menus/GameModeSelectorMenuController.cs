using System;
using UnityEngine;

namespace Source
{
    public class GameModeSelectorMenuController : IResultMenu<GameModeSelectorResult>
    {
        private readonly GameModeSelectorMenuView _view;

        private GameModeSelectorResult _result;

        public bool BlockClosing => false;
        public Transform Transform => _view.transform;
        
        public event Action OnShow;
        public event Action<GameModeSelectorResult> OnResult;

        public GameModeSelectorMenuController(GameModeSelectorMenuView view)
        {
            view.Missons.OnClick.AddListener(() => Result(GameModeSelectorResult.Missions));
            view.Sandbox.OnClick.AddListener(() => Result(GameModeSelectorResult.Sandbox));
            
            view.Close.OnClick.AddListener(Close);
            view.Window.OnOff.AddListener(OnClose);
            view.Window.OnOn.AddListener(OnOpen);

            _result = GameModeSelectorResult.Close;
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

        private void Result(GameModeSelectorResult result)
        {
            _result = result;
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