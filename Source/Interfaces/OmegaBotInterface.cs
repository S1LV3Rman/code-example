using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Source
{
    public class OmegaBotInterface
    {
        private readonly OmegaBotView _view;

        private Rigidbody rigidbody;

        public event Action<Module> OnAddModule;
        public event Action<int> OnRangefinderValueChange;

        public event Action<BotPort, int> OnChangeSensorValue;

        public OmegaBotInterface(OmegaBotView view)
        {
            _view = view;
            rigidbody = _view.GetComponent<Rigidbody>();
            rigidbody.centerOfMass = _view.CenterOfMass.localPosition;
            
            if (_view.Rangefinder != null)
                _view.Rangefinder.OnValueChange += RangefinderValueChange;
            
            foreach (var slot in _view.Slots)
                if (slot.Module != null && slot.Module is ISensor sensor)
                    sensor.OnValueChange += SensorValueChanged;
        }

        public void ResetVerticalRotation()
        {
            var rotation = Rotation;
            rotation.x = 0f;
            rotation.z = 0f;
            Rotation = rotation;
        }

        public void ResetVelocity()
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
        }

        public void DisablePhysics()
        {
            rigidbody.isKinematic = true;
        }

        public void EnablePhysics()
        {
            rigidbody.isKinematic = false;
        }

        public void RemoveAllModules()
        {
            _view.Rangefinder = null;
            for (var i = 0; i < _view.Slots.Count;)
            {
                var slot = _view.Slots[i];
                if (slot.Detail != null)
                    Object.Destroy(slot.Detail.gameObject);
                slot.Detail = null;

                if (slot.Type == SlotName.Bumper1 ||
                    slot.Type == SlotName.Bumper2 ||
                    slot.Type == SlotName.Bumper3 ||
                    slot.Type == SlotName.Bumper4 ||
                    slot.Type == SlotName.Bumper5)
                {
                    _view.Slots.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }
        }

        public void AddModule(Module module, SlotName slotType)
        {
            foreach (var slot in _view.Slots)
                if (slot.Type == slotType)
                {
                    module.transform.SetParent(slot.transform, false);
                    slot.Detail = module;
                    
                    if (module is ISensor sensor)
                        sensor.OnValueChange += SensorValueChanged;
                    return;
                }
        }
        
        public void AddBumper(Bumper bumper, SlotName slotType)
        {
            foreach (var slot in _view.Slots)
                if (slot.Type == slotType)
                {
                    bumper.transform.SetParent(slot.transform, false);
                    slot.Detail = bumper;

                    foreach (var bumperSlot in bumper.Slots)
                        _view.Slots.Add(bumperSlot);
                    
                    return;
                }
        }
        
        public void SetRangefinder(Rangefinder rangefinder)
        {
            _view.Rangefinder = rangefinder;
            _view.Rangefinder.OnValueChange += RangefinderValueChange;
        }

        private void SensorValueChanged(BotPort port, int value)
        {
            OnChangeSensorValue?.Invoke(port, value);
        }

        private void RangefinderValueChange(int value)
        {
            OnRangefinderValueChange?.Invoke(value);
        }

        public Vector3 Position
        {
            get => _view.transform.position;
            set => _view.transform.position = value;
        }

        public Quaternion Rotation
        {
            get => _view.transform.rotation;
            set => _view.transform.rotation = value;
        }

        public void SetMotorsPower(int left, int right)
        {
            _view.Motor.Move(left, right);
        }

        public void SetModuleValue(BotPort port, int value)
        {
            var module = GetModule(port);

            if (module != null && module is IIndicator indicator)
                indicator.SetValue(value);
        }
        
        private Module GetModule(BotPort port)
        {
            foreach (var slot in _view.Slots)
            {
                if (slot.Module != null &&
                    slot.Module.Port == port)
                    return slot.Module;
            }

            return null;
        }
    }
}