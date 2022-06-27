using System;
using UnityEngine;

namespace Source
{
    public class ModuleInteractionTrigger
    {
        private readonly ConfiguratorController controller;

        private bool _moduleHit;
        private int _hitModuleId;

        public event Action<int> OnTap;
        public event Action<int> OnDrag;

        public ModuleInteractionTrigger(ConfiguratorController controller)
        {
            this.controller = controller;
        }

        public void Process()
        {
            if (controller.Mouse.PressedButton == 0)
            {
                if (_moduleHit)
                {
                    if (controller.Mouse.Up)
                    {
                        _moduleHit = false;
                    }
                    else if (!Physics.Raycast(controller.Mouse.Hover.GetRay(controller.UiCamera), out var hit, Mathf.Infinity, controller.ModulesLayer) ||
                             hit.transform.GetComponent<ID>() != _hitModuleId)
                    {
                        var target = new GameObject("Module").transform;
                        target.SetParent(controller.Configurator.DragArea, false);
                        target.position = controller.Mouse.Hover.GetWorldPosition(5f, controller.UiCamera);

                        controller.UntieModule(_hitModuleId, target);

                        OnDrag?.Invoke(_hitModuleId);
                        _moduleHit = false;
                    }
                }
                else
                {
                    if (controller.Mouse.Down &&
                        Physics.Raycast(controller.Mouse.Hover.GetRay(controller.UiCamera), out var hit, Mathf.Infinity,
                            controller.ModulesLayer))
                    {
                        _hitModuleId = hit.transform.GetComponent<ID>();
                        _moduleHit = true;
                    }
                }

                if (controller.Mouse.Press.Tap)
                {
                    if (Physics.Raycast(controller.Mouse.Hover.GetRay(controller.UiCamera), out var hit, Mathf.Infinity,
                        controller.ModulesLayer))
                    {
                        _hitModuleId = hit.transform.GetComponent<ID>();
                        OnTap?.Invoke(_hitModuleId);
                        _moduleHit = false;
                    }
                }
            }
        }
    }
}