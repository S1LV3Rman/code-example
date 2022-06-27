using System.Linq;
using UnityEngine;

namespace Source
{
    public class DragNewConfiguratorState : IConfiguratorState
    {
        private readonly ConfiguratorController controller;
        private readonly int moduleId;

        private ModuleDragAndDrop _moduleDragAndDrop;
        
        public bool Running { get; private set; }

        public DragNewConfiguratorState(ConfiguratorController controller, ModuleType moduleType)
        {
            this.controller = controller;
            
            moduleId = controller.InstantiateModule(moduleType);

            _moduleDragAndDrop = new ModuleDragAndDrop(controller, moduleId);
        }

        private void PlaceModule(int id)
        {
            var module = controller.modules[id];
            controller.Amplitude.SendEvent("add-module",
                new Property("type", module.Type));
            if (module.Type == ModuleType.Bumper)
                controller.SetState(new IdleConfiguratorState(controller));
            else
                controller.SetState(new PortConfiguratorState(controller, id));
        }

        private void DropModule()
        {
            controller.SetState(new IdleConfiguratorState(controller));
        }
        
        public void Start()
        {
            if (!Running)
            {
                var moduleType = controller.modules[moduleId].Type;
                var bumperSet = controller.modules.Values.Any(module => module.Type == ModuleType.Bumper);
                controller.Configurator.ShowSlotsFor(moduleType, bumperSet);
                controller.Configurator.LowlightPorts();
                controller.Configurator.LockScroll();
                controller.Configurator.SetTip("Поставь модуль на любую из подсвеченных площадок");
                controller.SetSlotsColors(moduleType);
                
                controller.Amplitude.SendEvent("drag-new-module",
                    new Property("type", moduleType));
                
                SubscribeEvents();
                Running = true;
            }
        }

        public void Run()
        {
            if (!Running) return;
            
            _moduleDragAndDrop.Process();
        }

        public void Stop()
        {
            if (Running)
            {
                UnsubscribeEvents();
                Running = false;
            }
        }

        private void SubscribeEvents()
        {
            _moduleDragAndDrop.OnDrop += DropModule;
            _moduleDragAndDrop.OnPlace += PlaceModule;
        }

        private void UnsubscribeEvents()
        {
            _moduleDragAndDrop.OnDrop -= DropModule;
            _moduleDragAndDrop.OnPlace -= PlaceModule;
        }
    }
}