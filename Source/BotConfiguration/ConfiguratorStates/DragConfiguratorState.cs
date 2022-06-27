using System.Linq;
using UnityEngine;

namespace Source
{
    public class DragConfiguratorState : IConfiguratorState
    {
        private readonly ConfiguratorController controller;
        private readonly int moduleId;

        private ModuleDragAndDrop _moduleDragAndDrop;
        
        public bool Running { get; private set; }

        public DragConfiguratorState(ConfiguratorController controller, int moduleId)
        {
            this.controller = controller;
            this.moduleId = moduleId;

            _moduleDragAndDrop = new ModuleDragAndDrop(controller, moduleId);
        }

        private void PlaceModule(int _)
        {
            controller.Amplitude.SendEvent("replace-module");
            controller.RollbackState();
        }

        private void DropModule()
        {
            controller.Amplitude.SendEvent("remove-module");
            controller.RollbackState();
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
                
                controller.Amplitude.SendEvent("drag-old-module", 
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