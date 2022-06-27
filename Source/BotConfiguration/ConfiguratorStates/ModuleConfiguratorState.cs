using UnityEngine;

namespace Source
{
    public class ModuleConfiguratorState : IConfiguratorState
    {
        private readonly ConfiguratorController controller;
        private readonly int portId;

        private ModuleInteractionTrigger _moduleInteractionTrigger;

        public bool Running { get; private set; }
        
        public ModuleConfiguratorState(ConfiguratorController controller, int portId)
        {
            this.controller = controller;
            this.portId = portId;

            _moduleInteractionTrigger = new ModuleInteractionTrigger(controller);
        }

        public void Start()
        {
            if (!Running)
            {
                controller.Configurator.HideSlots();
                controller.Configurator.LowlightPorts();
                controller.Configurator.UnlockScroll();
                controller.Configurator.HighlightPort(controller.ports[portId].Type);
                controller.Configurator.SetTip("Выбери модуль, который ты хочешь подключить к порту");
                
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

        private void SelectModule(int moduleId)
        {
            var module = controller.modules[moduleId];
            if (module.PortId == portId)
            {
                controller.Amplitude.SendEvent("disconnect-module");
                controller.DisconnectPort(portId);
            }
            else
            {
                controller.Amplitude.SendEvent("connect-module");
                controller.ConnectPort(portId, moduleId);
            }
            
            controller.SetState(new IdleConfiguratorState(controller));
        }

        private void DragPlacedModule(int moduleId)
        {
            controller.SetState(new DragConfiguratorState(controller, moduleId));
        }

        private void SelectPort(int id)
        {
            if (portId == id)
            {
                controller.Amplitude.SendEvent("deselect-port");
                controller.SetState(new IdleConfiguratorState(controller));
            }
            else
            {
                controller.Amplitude.SendEvent("select-port");
                controller.SetState(new ModuleConfiguratorState(controller, id));
            }
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