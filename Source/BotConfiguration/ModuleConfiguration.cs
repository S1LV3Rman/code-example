using UnityEngine;

namespace Source
{
    public struct ModuleConfiguration
    {
        public ModuleType Type;
        public Vector3 LocalPosition;
        public Quaternion LocalRotation;
        public Vector3 LocalScale;
        public Transform Transform;
        public GameObject GameObject;
        public int PortId;
        public int SlotId;
    }
}