using UnityEngine;

namespace Source
{
    public class OffConfiguratorState : IConfiguratorState
    {
        private readonly ConfiguratorController controller;

        public bool Running { get; private set; }

        public OffConfiguratorState(ConfiguratorController controller)
        {
            this.controller = controller;
        }

        public void Start()
        {
            if (!Running)
            {
                controller.Configurator.GameObject.SetActive(false);
                controller.Configurator.ClearTip();
                
                Running = true;
            }
        }

        public void Run()
        {
            if (!Running) return;

            if (controller.Mouse.PressedButton == 0 && !controller.Mouse.IsOverUI)
            {
                if (controller.Mouse.Press.Tap)
                {
                    if (Physics.Raycast(controller.Mouse.Hover.GetRay(controller.MainCamera.Camera), out var hit,
                        Mathf.Infinity, controller.OmegaBotLayer))
                    {
                        controller.Amplitude.SendEvent("open-configurator");
                        controller.Configurator.GameObject.SetActive(true);
                        controller.SetState(new IdleConfiguratorState(controller));
                    }
                }
            }
        }

        public void Stop()
        {
            if (Running)
            {
                Running = false;
            }
        }
    }
}