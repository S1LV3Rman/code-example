using System;
using UnityEngine;

namespace Source
{
    public class ButtonModule : Module, ISensor
    {
        public event Action<BotPort, int> OnValueChange;
        
        private bool _click;
        private bool _hover;

        private bool Click
        {
            get => _click;
            set
            {
                _click = value;
                OnValueChange?.Invoke(Port, _click ? 1 : 0);
            }
        }

        private void OnMouseDown()
        {
            if (_hover)
                Click = true;
        }

        private void OnMouseUp()
        {
            if(_hover)
                Click = false;
        }

        private void OnMouseEnter()
        {
            _hover = true;
        }

        private void OnMouseExit()
        {
            _hover = false;
            if (Click)
                Click = false;
        }
    }
}