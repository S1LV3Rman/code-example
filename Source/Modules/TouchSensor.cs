using System;
using UnityEngine;

namespace Source
{
    public class TouchSensor : Module, ISensor
    {
        public event Action<BotPort, int> OnValueChange;
        
        private int _triggers;

        private int Triggers
        {
            get => _triggers;
            set
            {
                _triggers = value;
                OnValueChange?.Invoke(Port, Mathf.Clamp(_triggers, 0, 1));
            }
        }

        private void OnTriggerEnter(Collider _)
        {
            Triggers++;
        }

        private void OnTriggerExit(Collider _)
        {
            Triggers--;
        }
    }
}