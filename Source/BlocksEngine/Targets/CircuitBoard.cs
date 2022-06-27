using System;
using System.Collections.Generic;
using UnityEngine;

namespace Source
{
    public class CircuitBoard : BE2_TargetObject
    {
        private Dictionary<BotPort, int> _values = new Dictionary<BotPort, int>();
        private int _leftMotorPower;
        private int _rightMotorPower;
        private int _rangefinderValue;

        public event Action<BotPort, int> OnSetPortValue;
        public event Action<int, int> OnSetMotorPower;
        public event Action<string> OnConsoleMessage;

        public void Reset()
        {
            _values.Clear();
            _leftMotorPower = 0;
            _rightMotorPower = 0;
        }

        public void SendMessageToConsole(string message)
        {
            OnConsoleMessage?.Invoke(message);
        }

        public void SetPortValue(BotPort port, int value)
        {
            if (port.IsDigital()) value = Mathf.Clamp(value, 0, 1);
            else if (port.IsPWM()) value = Mathf.Clamp(value, 0, 255);
            else if (port.IsAnalog()) value = Mathf.Clamp(value, 0, 1023);
            
            _values[port] = value;
            OnSetPortValue?.Invoke(port, value);
        }

        public void ChangePortValue(BotPort port, int value)
        {
            if (port.IsDigital()) value = Mathf.Clamp(value, 0, 1);
            else if (port.IsPWM()) value = Mathf.Clamp(value, 0, 255);
            else if (port.IsAnalog()) value = Mathf.Clamp(value, 0, 1023);
            
            _values[port] = value;
        }

        public int GetPortValue(BotPort port)
        {
            _values.TryGetValue(port, out var value);
            return value;
        }

        public int GetRange()
        {
            return _rangefinderValue;
        }

        public void SetRange(int value)
        {
            _rangefinderValue = value;
        }

        public void SetMotorsPower(int left, int right)
        {
            left = Mathf.Clamp(left, -255, 255);
            right = Mathf.Clamp(right, -255, 255);
            
            _leftMotorPower = left;
            _rightMotorPower = right;
            OnSetMotorPower?.Invoke(left, right);
        }
    }
}