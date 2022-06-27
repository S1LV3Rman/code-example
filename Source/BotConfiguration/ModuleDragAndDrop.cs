using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Source
{
    public class ModuleDragAndDrop
    {
        private readonly ConfiguratorController controller;
        private readonly int moduleId;

        private Transform _dragTarget;

        public event Action<int> OnPlace;
        public event Action OnDrop;

        public ModuleDragAndDrop(ConfiguratorController controller, int moduleId)
        {
            this.controller = controller;
            this.moduleId = moduleId;

            _dragTarget = controller.modules[moduleId].Transform.parent;
        }

        public void Process()
        {
            if (_dragTarget == null) return;
            
            // todo: move constant to config
            _dragTarget.position = controller.Mouse.Hover.GetWorldPosition(5f, controller.UiCamera);

            var tiedSlotId = controller.modules[moduleId].SlotId;
            if (Physics.Raycast(controller.Mouse.Hover.GetRay(controller.UiCamera), out var hit, Mathf.Infinity, controller.UiLayer))
            {
                // todo: move constant to config
                if (hit.transform.CompareTag("PlacementLocation"))
                {
                    int slotId = hit.transform.parent.GetComponent<ID>();
                    if (tiedSlotId != slotId)
                    {
                        if (tiedSlotId >= 0)
                            controller.UntieModule(moduleId, _dragTarget);
                        controller.TieModule(moduleId, slotId);
                    }
                }
                else if (tiedSlotId >= 0)
                    controller.UntieModule(moduleId, _dragTarget);
            }
            else if (tiedSlotId >= 0)
                controller.UntieModule(moduleId, _dragTarget);

            if (controller.Mouse.Up)
            {
                tiedSlotId = controller.modules[moduleId].SlotId;
                if (tiedSlotId >= 0)
                {
                    controller.ConfirmTie(moduleId);
                    OnPlace?.Invoke(moduleId);
                }
                else
                {
                    controller.RemoveModule(moduleId);
                    OnDrop?.Invoke();
                }

                Object.Destroy(_dragTarget.gameObject);
                _dragTarget = null;
            }
        }
    }
}