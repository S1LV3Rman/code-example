using Lean.Touch;
using UnityEngine;

namespace Source
{
    public class CameraInterface
    {
        private readonly CameraView _view;

        private Transform Transform => _view.transform;

        public Vector3 LocalRotation
        {
            get => Transform.localRotation.eulerAngles;
            set => Transform.localRotation = Quaternion.Euler(value);
        }

        public Vector3 Position
        {
            get => Transform.position;
            set => Transform.position = value;
        }

        public Camera Camera => _view.Camera;

        public bool IsMoving => _view.Drag.IsMoving;

        public CameraInterface(CameraView view)
        {
            _view = view;
        }

        public float CurrentZoom => _view.Zoom.Value;

        public void Zoom(float delta)
        {
            _view.Zoom.Zoom(delta);
        }

        public void LockZoom()
        {
            _view.Zoom.Lock();
        }

        public void UnlockZoom()
        {
            _view.Zoom.Unlock();
        }

        public void LockDrag()
        {
            _view.Drag.Lock();
        }

        public void UnlockDrag()
        {
            _view.Drag.Unlock();
        }

        public void LockRotation()
        {
            _view.Rotate.Lock();
        }

        public void UnlockRotation()
        {
            _view.Rotate.Unlock();
        }
    }
}