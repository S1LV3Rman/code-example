using UnityEngine;

namespace Source
{
    public class PortConfiguratorState : IConfiguratorState
    {
        private readonly ConfiguratorController controller;
        private readonly int moduleId;

        private ModuleInteractionTrigger _moduleInteractionTrigger;

        public bool Running { get; private set; }
        
        public PortConfiguratorState(ConfiguratorController controller, int moduleId)
        {
            this.controller = controller;
            this.moduleId = moduleId;

            _moduleInteractionTrigger = new ModuleInteractionTrigger(controller);
        }

        public void Start()
        {
            if (!Running)
            {
                if (!controller.modules.ContainsKey(moduleId))
                {
                    controller.SetState(new IdleConfiguratorState(controller));
                    return;
                }
                
                controller.Configurator.HideSlots();
                controller.Configurator.HighlightPorts();
                controller.Configurator.UnlockScroll();
                controller.Configurator.SetTip("Выбери порт, к которому ты хочешь подключить модуль");
                
                SubscribeEvents();
                Running = true;
            }
        }

        public void Run()
        {
            if (!Running) return;
            
            _moduleInteractionTrigger.Process();
        }

        public void Stop()
        {
            if (Running)
            {
                UnsubscribeEvents();
                Running = false;
            }
        }

        private void SelectModule(int id)
        {
            if (moduleId == id)
            {
                controller.Amplitude.SendEvent("deselect-module");
                controller.SetState(new IdleConfiguratorState(controller));
            }
            else
            {
                controller.Amplitude.SendEvent("select-module");
                controller.SetState(new PortConfiguratorState(controller, id));
            }
        }

        private void DragPlacedModule(int id)
        {
            controller.SetState(new DragConfiguratorState(controller, id));
        }

        private void SelectPort(int portId)
        {
            var module = controller.modules[moduleId];
            if (module.PortId == portId)
            {
                controller.Amplitude.SendEvent("disconnect-port");
                controller.DisconnectPort(portId);
            }
            else
            {
                controller.Amplitude.SendEvent("connect-port");
                controller.ConnectPort(portId, moduleId);
            }
            
            controller.SetState(new IdleConfiguratorState(controller));
        }

        private void DragNewModule(ModuleType moduleType)
        {
            controller.SetState(new DragNewConfiguratorState(controller, moduleType));
        }

        private void SubscribeEvents()
        {
            controller.Configurator.OnModuleButtonDown += DragNewModule;
            controller.Configurator.OnPortClick += SelectPort;
                
            _moduleInteractionTrigger.OnDrag += DragPlacedModule;
            _moduleInteractionTrigger.OnTap += SelectModule;
        }

        private void UnsubscribeEvents()
        {
            controller.Configurator.OnModuleButtonDown -= DragNewModule;
            controller.Configurator.OnPortClick -= SelectPort;
            
            _moduleInteractionTrigger.OnDrag -= DragPlacedModule;
            _moduleInteractionTrigger.OnTap -= SelectModule;
        }
    }
}