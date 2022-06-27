using Lean.Gui;
using UnityEngine;
using UnityEngine.Events;

namespace Source
{
    [RequireComponent(typeof(LeanButton)), RequireComponent(typeof(LeanToggle))]
    public class DropdownButtonVariant : MonoBehaviour
    {
        private LeanToggle toggleCache;
        private LeanButton buttonCache;

        public LeanToggle Toggle
        {
            get
            {
                if (toggleCache == null)
                    toggleCache = GetComponent<LeanToggle>();

                return toggleCache;
            }
        }

        public LeanButton Button
        {
            get
            {
                if (buttonCache == null)
                    buttonCache = GetComponent<LeanButton>();

                return buttonCache;
            }
        }
    }
}