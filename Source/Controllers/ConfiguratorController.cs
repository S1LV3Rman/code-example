using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Source
{
    public class ConfiguratorController : IController
    {
        public readonly ConfiguratorInterface Configurator;
        public readonly OmegaBotInterface OmegaBot;
        public readonly Camera UiCamera;
        public readonly CameraView MainCamera;
        public readonly ModulesConfig ModulesPrefabs;
        public readonly PortsConfig PortsColors;
        public readonly SlotsConfig SlotsColors;
        public readonly MouseInputService Mouse;
        public readonly AmplitudeService Amplitude;
        public readonly ModulesController Modules;

        public readonly int UiLayer;
        public readonly int ModulesLayer;
        public readonly int OmegaBotLayer;
        
        private IConfiguratorState currentState;
        private IConfiguratorState previousState;

        private bool _initialized;
        
        public Dictionary<int, ModuleConfiguration> modules;
        public Dictionary<int, SlotConfiguration> slots;
        public Dictionary<int, PortConfiguration> ports;

        public ConfiguratorController(
            ConfiguratorInterface configurator, OmegaBotInterface omegaBot,
            Camera uiCamera, CameraView mainCamera,
            ModulesController modulesController,
            ModulesConfig modulesPrefabs, PortsConfig portsColors, SlotsConfig slotsColors,
            MouseInputService mouse, AmplitudeService amplitude)
        {
            Configurator = configurator;
            OmegaBot = omegaBot;
            UiCamera = uiCamera;
            Modules = modulesController;
            ModulesPrefabs = modulesPrefabs;
            PortsColors = portsColors;
            SlotsColors = slotsColors;
            Mouse = mouse;
            Amplitude = amplitude;
            MainCamera = mainCamera;
            
            UiLayer       = LayerMask.GetMask("UI");
            ModulesLayer  = LayerMask.GetMask("Module");
            OmegaBotLayer = LayerMask.GetMask("OmegaBot");

            modules = new Dictionary<int, ModuleConfiguration>();
            slots   = new Dictionary<int, SlotConfiguration>();
            ports   = new Dictionary<int, PortConfiguration>();
        }

        public void Init()
        {
            Configurator.OnClear += RemoveAllModules;
            Configurator.OnClose += SaveAndClose;
            Configurator.OnSave += SaveAndClose;
            
            SetState(new OffConfiguratorState(this));
        }
        
        public void InitializeData()
        {
            if (_initialized) return;
            
            foreach (var slot in Configurator.Slots)
                AddSlot(slot);

            foreach (var port in Configurator.Ports)
            {
                int id = port.ID;
                var portConfiguration = new PortConfiguration
                {
                    Type = port.Type,
                    ModuleId = -1
                };
                ports.Add(id, portConfiguration);
            }

            _initialized = true;
        }

        private void SaveAndClose()
        {
            OmegaBot.RemoveAllModules();
            Modules.ClearModules();
            
            if (modules.Values.Any(module => module.Type == ModuleType.Bumper))
            {
                var bumper = modules.Values.First(module => module.Type == ModuleType.Bumper);
                
                var slot = slots[bumper.SlotId];
                var bumperGO = Object.Instantiate(ModulesPrefabs.prefabs[bumper.Type].real);
                
                var newBumper = bumperGO.GetComponent<Bumper>();
                OmegaBot.AddBumper(newBumper, slot.Type);
            }

            foreach (var module in modules.Values)
            {
                if (module.Type == ModuleType.Bumper) continue;
                
                var slot = slots[module.SlotId];
                var moduleGO = Object.Instantiate(ModulesPrefabs.prefabs[module.Type].real);
                
                Module newModule;
                if (module.Type == ModuleType.Rangefinder)
                {
                    var rangefinder = moduleGO.GetComponent<Rangefinder>();
                    OmegaBot.SetRangefinder(rangefinder);
                    
                    newModule = moduleGO.GetComponent<Servo>();
                }
                else
                {
                    newModule = moduleGO.GetComponent<Module>();
                }
                
                if (module.PortId >= 0)
                    newModule.Port = ports[module.PortId].Type;
                
                OmegaBot.AddModule(newModule, slot.Type);
                Modules.AddModule(newModule);
            }

            Amplitude.SendEvent("close-configurator");
            SetState(new OffConfiguratorState(this));
        }

        public void Run()
        {
            if (!currentState.Running)
                currentState.Start();
            
            currentState.Run();
        }

        private bool AddSlot(ConfigurationSlot slot)
        {
            int id = slot.GetComponent<ID>();
            if (slots.ContainsKey(id)) return false;
            
            var type = slot.Type;
            var bindedId = type switch
            {
                SlotName.BumperMaster => Configurator.Slots.First(s => s.Type == SlotName.BumperSlave).GetComponent<ID>(),
                SlotName.BumperSlave => Configurator.Slots.First(s => s.Type == SlotName.BumperMaster).GetComponent<ID>(),
                SlotName.RangefinderMaster => Configurator.Slots.First(s => s.Type == SlotName.RangefinderSlave).GetComponent<ID>(),
                SlotName.RangefinderSlave => Configurator.Slots.First(s => s.Type == SlotName.RangefinderMaster).GetComponent<ID>(),
                _ => -1
            };
                
            var slotConfiguration = new SlotConfiguration
            {
                Type = type,
                Transform = slot.transform,
                BlockedSlotsId = slot.BlockedSlots.Select(ID => ID.id).ToList(),
                BindedSlotId = bindedId,
                ModuleId = -1
            };
            slots.Add(id, slotConfiguration);

            return true;
        }

        public void SetState(IConfiguratorState newState)
        {
            currentState?.Stop();

            previousState = currentState;
            currentState = newState;
        }

        public void RollbackState()
        {
            currentState.Stop();
            currentState = previousState;
        }

        public int InstantiateModule(ModuleType moduleType)
        {
            var moduleParentGO = Object.Instantiate(ModulesPrefabs.prefabs[moduleType].configuration, Configurator.DragArea);
            var moduleTransform = moduleParentGO.transform.GetChild(0);
            var moduleGO = moduleTransform.gameObject;
            int id = moduleTransform.GetComponent<ID>();

            var moduleConfiguration = new ModuleConfiguration
            {
                Type = moduleType,
                LocalPosition = moduleTransform.localPosition,
                LocalRotation = moduleTransform.localRotation,
                LocalScale = moduleTransform.localScale,
                Transform = moduleTransform,
                GameObject = moduleGO,
                PortId = -1,
                SlotId = -1
            };
            modules.Add(id, moduleConfiguration);
            
            return id;
        }

        private void HideModule(int id)
        {
            if (id >= 0)
            {
                var placedModule = modules[id];
                placedModule.GameObject.SetActive(false);
            }
        }

        private void ShowModule(int id)
        {
            if (id >= 0)
            {
                var placedModule = modules[id];
                placedModule.GameObject.SetActive(true);
            }
        }

        public void TieModule(int moduleId, int slotId)
        {
            var module = modules[moduleId];
            var slot = slots[slotId];

            if (module.Type == ModuleType.Bumper || module.Type == ModuleType.Rangefinder)
            {
                var bindedSlot = slots[slot.BindedSlotId];
                HideModule(bindedSlot.ModuleId);
            }

            HideModule(slot.ModuleId);
            foreach (var blockedId in slot.BlockedSlotsId)
            {
                var blockedSlot = slots[blockedId];
                HideModule(blockedSlot.ModuleId);
            }

            module.Transform.SetParent(slot.Transform, false);

            module.Transform.localPosition = Vector3.zero;
            module.Transform.localRotation = Quaternion.identity;
            module.Transform.localScale    = Vector3.one;

            module.SlotId = slotId;
            
            modules[moduleId] = module;
        }

        public void UntieModule(int moduleId, Transform newParent)
        {
            var module = modules[moduleId];
            var slot = slots[module.SlotId];

            var doubleModule = module.Type == ModuleType.Bumper || module.Type == ModuleType.Rangefinder;
            var placedModuleId = slot.ModuleId;
            if (placedModuleId == moduleId)
            {
                if (doubleModule)
                    FreeSlot(slot.BindedSlotId);
                
                FreeSlot(module.SlotId);
            }
            else
            {
                if (doubleModule)
                {
                    var bindedSlot = slots[slot.BindedSlotId];
                    ShowModule(bindedSlot.ModuleId);
                }
                
                ShowModule(placedModuleId);
                
                foreach (var blockedId in slot.BlockedSlotsId)
                {
                    var blockedSlot = slots[blockedId];
                    ShowModule(blockedSlot.ModuleId);
                }
            }

            module.Transform.SetParent(newParent, false);

            module.Transform.localPosition = module.LocalPosition;
            module.Transform.localRotation = module.LocalRotation;
            module.Transform.localScale = module.LocalScale;

            module.SlotId = -1;

            modules[moduleId] = module;
        }

        public void ConfirmTie(int moduleId)
        {
            var module = modules[moduleId];
            var slot = slots[module.SlotId];
            
            if (module.Type == ModuleType.Bumper || module.Type == ModuleType.Rangefinder)
            {
                var bindedSlot = slots[slot.BindedSlotId];
                var bindedModuleId = bindedSlot.ModuleId;
                if (bindedModuleId >= 0)
                    RemoveModule(bindedModuleId);

                bindedSlot.ModuleId = moduleId;

                slots[slot.BindedSlotId] = bindedSlot;
            }
            
            var placedModuleId = slot.ModuleId;
            if (placedModuleId >= 0)
                RemoveModule(placedModuleId);
                
            foreach (var blockedId in slot.BlockedSlotsId)
            {
                var blockedSlot = slots[blockedId];
                RemoveModule(blockedSlot.ModuleId);
            }

            slot.ModuleId = moduleId;

            slots[module.SlotId] = slot;

            if (module.Type == ModuleType.Bumper)
            {
                var bumper = module.GameObject.GetComponent<BumperConfiguration>();
                foreach (var bumperSlot in bumper.Slots)
                    if (AddSlot(bumperSlot))
                        Configurator.Slots.Add(bumperSlot);
            }
        }

        public void RemoveModule(int moduleId)
        {
            if (!modules.ContainsKey(moduleId)) return;
            
            var moduleToRemove = modules[moduleId];

            if (moduleToRemove.SlotId >= 0)
                FreeSlot(moduleToRemove.SlotId);
            
            if (moduleToRemove.PortId >= 0)
                DisconnectPort(moduleToRemove.PortId);

            if (moduleToRemove.Type == ModuleType.Bumper)
            {
                var bumper = moduleToRemove.GameObject.GetComponent<BumperConfiguration>();
                foreach (var bumperSlot in bumper.Slots)
                {
                    int bumperSlotId = bumperSlot.GetComponent<ID>();

                    if (slots.ContainsKey(bumperSlotId))
                    {
                        var slot = slots[bumperSlotId];
                        RemoveModule(slot.ModuleId);
                        slots.Remove(bumperSlotId);
                        Configurator.Slots.Remove(bumperSlot);
                    }
                }
            }
            
            Object.Destroy(moduleToRemove.GameObject);
            modules.Remove(moduleId);
        }

        private void RemoveAllModules()
        {
            Amplitude.SendEvent("clear-configurator");
            var ids = modules.Keys.ToArray();
            foreach (var id in ids)
                RemoveModule(id);
        }

        private void FreeSlot(int slotId)
        {
            var slot = slots[slotId];
            
            var module = modules[slot.ModuleId];
            module.SlotId = -1;
            modules[slot.ModuleId] = module;
            
            slot.ModuleId = -1;
            slots[slotId] = slot;
        }

        public void DisconnectPort(int portId)
        {
            var port = ports[portId];
            
            var module = modules[port.ModuleId];
            module.PortId = -1;
            modules[port.ModuleId] = module;
            
            port.ModuleId = -1;
            ports[portId] = port;
            
            SetDefaultPort(portId);
        }

        public void ConnectPort(int portId, int moduleId)
        {
            var port = ports[portId];
            var module = modules[moduleId];
            
            if (port.ModuleId >= 0)
                DisconnectPort(portId);
            
            if (module.PortId >= 0)
                DisconnectPort(module.PortId);
            
            module.PortId = portId;
            modules[moduleId] = module;
            
            port.ModuleId = moduleId;
            ports[portId] = port;

            switch (module.Type)
            {
                case ModuleType.LED:
                case ModuleType.Sound:
                    SetIndicatorPort(portId);
                    break;
                case ModuleType.Touch:
                case ModuleType.Button:
                case ModuleType.Line:
                case ModuleType.Illumination:
                case ModuleType.Rangefinder:
                    SetSensorPort(portId);
                    break;
            }
        }

        private void SetDefaultPort(int portId)
        {
            var portType = ports[portId].Type;
            Configurator.ChangePortColor(portType,
                PortsColors.DefaultBackgroundOn, PortsColors.DefaultBackgroundOff, PortsColors.DefaultOutline);
        }

        private void SetIndicatorPort(int portId)
        {
            var portType = ports[portId].Type;
            Configurator.ChangePortColor(portType,
                PortsColors.IndicatorBackgroundOn, PortsColors.IndicatorBackgroundOff, PortsColors.IndicatorOutline);
        }

        private void SetSensorPort(int portId)
        {
            var portType = ports[portId].Type;
            Configurator.ChangePortColor(portType,
                PortsColors.SensorBackgroundOn, PortsColors.SensorBackgroundOff, PortsColors.SensorOutline);
        }

        public void SetSlotsColors(ModuleType type)
        {
            switch (type)
            {
                case ModuleType.LED:
                case ModuleType.Sound:
                    Configurator.ChangeSlotsColor(SlotsColors.Indicator);
                    break;
                case ModuleType.Touch:
                case ModuleType.Button:
                case ModuleType.Line:
                case ModuleType.Illumination:
                    Configurator.ChangeSlotsColor(SlotsColors.Sensor);
                    break;
                case ModuleType.Rangefinder:
                case ModuleType.Bumper:
                    Configurator.ChangeSlotsColor(SlotsColors.Other);
                    break;
            }
        }
    }
}