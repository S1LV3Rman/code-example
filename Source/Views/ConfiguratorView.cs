using System;
using System.Collections.Generic;
using Lean.Gui;
using Sirenix.OdinInspector;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Source
{
    public class ConfiguratorView : MonoBehaviour
    {
        [Header("Buttons")]
        public LeanButton Blocker;
        public LeanButton CloseButton;
        public LeanButton ClearButton;
        public LeanButton SaveButton;

        [Header("OmegaBot")]
        public Transform Preview;
        public List<ConfigurationSlot> Slots;
        public List<PortMarker> Ports;
        
        [Header("Modules")]
        public ScrollRect ModulesScroll;
        public List<ModuleButton> ModulesButtons;

        [Header("Other")]
        public TMP_Text Tips;
    }

    [Serializable]
    public struct ModuleButton
    {
        public ModuleType Type;
        public LeanButton Button;
        public Color SlotsColor;
    }
}