using Lean.Gui;
using UnityEngine;

namespace Source
{
    public class LeftToolbarView : MonoBehaviour
    {
        public LeanButton PlayButton;
        public LeanToggle PlayToggle;
        public LeanButton ResetButton;
        public LeanToggle WorkspaceButton;

        [Header("Block sections")]
        
        public LeanToggle ControlsSection;
        public LeanToggle MovementSection;
        public LeanToggle RangefinderSection;
        public LeanToggle SensorsSection;
        public LeanToggle IndicatorsSection;
        public LeanToggle OperationsSection;
        public LeanToggle VariablesSection;
    }
}