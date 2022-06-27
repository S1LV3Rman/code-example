using Lean.Touch;
using UnityEngine;

namespace Source
{
    public class DragCamera : LeanDragCamera
    {
        [HideInInspector] public bool IsMoving;

        private bool _active;

        protected override void Awake()
        {
            base.Awake();
            _active = true;
        }

        protected override void LateUpdate()
        {
            if (_active)
                base.LateUpdate();

            IsMoving = remainingDelta.magnitude > 0.0001f;
        }

        public void Lock()
        {
            _active = false;
        }

        public void Unlock()
        {
            _active = true;
        }
    }
}