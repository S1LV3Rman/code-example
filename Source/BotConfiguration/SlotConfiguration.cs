using System.Collections.Generic;
using UnityEngine;

namespace Source
{
    public struct SlotConfiguration
    {
        public SlotName Type;
        public Transform Transform;
        public List<int> BlockedSlotsId;
        public int BindedSlotId;
        public int ModuleId;
    }
}