using System;
using UnityEngine;

namespace Source
{
    public class ModuleSlot : MonoBehaviour, ISlot
    {
        [SerializeField] private SlotName type;
        public SlotName Type => type;

        private Detail detail;
        public Detail Detail
        {
            get => detail;
            set
            {
                detail = value;
                if (detail is Module module)
                    Module = module;
                else
                    Module = null;
            }
        }

        public Module Module { get; private set; }
    }
}