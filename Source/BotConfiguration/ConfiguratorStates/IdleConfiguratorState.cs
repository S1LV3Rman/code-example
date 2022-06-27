using UnityEngine;

namespace Source
{
    public class IdleConfiguratorState : IConfiguratorState
    {
        private readonly ConfiguratorController controller;

        private ModuleInteractionTrigger _moduleInteractionTrigger;

        public bool Running { get; private set; }
        
        public IdleConfiguratorState(ConfiguratorController controller)
        {
            this.controller = controller;

            _moduleInteractionTrigger = new ModuleInteractionTrigger(controller);
        }

        public void Start()
        {
            if (!Running)
            {
                controller.InitializeData();
                
                controller.Configurator.HideSlots();
                controller.Configurator.LowlightPorts();
                controller.Configurator.UnlockScroll();
                controller.Configurator.ClearTip();
                
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
            controller.Amplitude.SendEvent("select-module");
            controller.SetState(new PortConfiguratorState(controller, moduleId));
        }

        private void DragPlacedModule(int moduleId)
        {
            controller.SetState(new DragConfiguratorState(controller, moduleId));
        }

        private void SelectPort(int portId)
        {
            if (controller.modules.Count > 0)
            {
                controller.Amplitude.SendEvent("select-port");
                controller.SetState(new ModuleConfiguratorState(controller, portId));
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