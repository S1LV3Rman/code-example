using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Source
{
    public class ConfigurationSlot : MonoBehaviour, ISlot
    {
        [SerializeField] private SlotName type;
        public SlotName Type => type;
        
        public GameObject SingleLocation;
        public Graphic SinglePlane;
        
        public GameObject DoubleLocation;
        public Graphic DoublePlane;

        public List<ID> BlockedSlots;
    }
}