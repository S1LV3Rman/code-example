using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Source
{
    public class ConfiguratorInterface
    {
        private readonly ConfiguratorView _view;

        public event Action<ModuleType> OnModuleButtonDown;
        public event Action<int> OnPortClick;
        public event Action OnSave;
        public event Action OnClear;
        public event Action OnClose;

        public ConfiguratorInterface(ConfiguratorView view)
        {
            _view = view;
            
            _view.ClearButton.OnClick.AddListener(Clear);
            _view.CloseButton.OnClick.AddListener(Close);
            _view.Blocker.OnClick.AddListener(Close);
            _view.SaveButton.OnClick.AddListener(Save);

            foreach (var module in _view.ModulesButtons)
                module.Button.OnDown.AddListener(() => ModuleButtonDown(module.Type));

            foreach (var port in _view.Ports)
                port.Button.OnDown.AddListener(() => PortClick(port.ID));
        }

        public GameObject GameObject => _view.gameObject;
        public Transform DragArea => _view.Preview;
        public List<ConfigurationSlot> Slots => _view.Slots;
        public List<PortMarker> Ports => _view.Ports;

        public void SetTip(string tip)
        {
            _view.Tips.text = tip;
        }

        public void ClearTip()
        {
            _view.Tips.text = "";
        }

        private void Save()
        {
            OnSave?.Invoke();
        }

        private void Clear()
        {
            OnClear?.Invoke();
        }

        private void Close()
        {
            OnClose?.Invoke();
        }

        public void ChangePortColor(BotPort portType, Color on, Color off, Color outline)
        {
            var port = _view.Ports.First(p => p.Type == portType);
            
            port.BackgroundOnColor = on;
            port.BackgroundOffColor = off;
            port.OutlineColor = outline;
        }

        public void ChangeSlotsColor(Color color)
        {
            foreach (var slot in _view.Slots)
            {
                slot.SinglePlane.color = color;
                if (slot.DoubleLocation != null)
                    slot.DoublePlane.color = color;
            }
        }

        public void HighlightPort(BotPort portType)
        {
            _view.Ports.First(p => p.Type == portType).Toggle.TurnOn();
        }

        public void HighlightPorts()
        {
            foreach (var port in _view.Ports)
                port.Toggle.TurnOn();
        }

        public void LowlightPorts()
        {
            foreach (var port in _view.Ports)
                port.Toggle.TurnOff();
        }

        public void LockScroll()
        {
            _view.ModulesScroll.enabled = false;
        }

        public void UnlockScroll()
        {
            _view.ModulesScroll.enabled = true;
        }

        public void HideSlots()
        {
            foreach (var slot in _view.Slots)
            {
                slot.SingleLocation.SetActive(false);
                if (slot.DoubleLocation != null)
                    slot.DoubleLocation.SetActive(false);
            }
        }

        public void ShowSlotsFor(ModuleType moduleType, bool bumperSet)
        {
            switch (moduleType)
            {
                case ModuleType.LED:
                case ModuleType.Sound:
                case ModuleType.Touch:
                case ModuleType.Button:
                case ModuleType.Line:
                case ModuleType.Illumination:
                    ShowSingleSlots(bumperSet);
                    break;
                case ModuleType.Rangefinder:
                    ShowRangefinderSlots();
                    break;
                case ModuleType.Bumper:
                    ShowBumperSlots();
                    break;
            }
        }
        
        private void ShowSingleSlots(bool bumperSet)
        {
            foreach (var slot in _view.Slots)
            {
                if (bumperSet && (slot.Type == SlotName.BumperMaster || slot.Type == SlotName.BumperSlave))
                    continue;
                slot.SingleLocation.SetActive(true);
            }
        }

        private void ShowBumperSlots()
        {
            foreach (var slot in _view.Slots)
                if (slot.Type == SlotName.BumperMaster)
                    slot.DoubleLocation.SetActive(true);
        }

        private void ShowRangefinderSlots()
        {
            foreach (var slot in _view.Slots)
                if (slot.Type == SlotName.RangefinderMaster)
                    slot.DoubleLocation.SetActive(true);
        }

        private void ModuleButtonDown(ModuleType moduleType)
        {
            OnModuleButtonDown?.Invoke(moduleType);
        }

        private void PortClick(int portId)
        {
            OnPortClick?.Invoke(portId);
        }
    }
}