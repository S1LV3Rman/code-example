using Lean.Gui;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Source
{
    [RequireComponent(typeof(LeanToggle))]
    public class DropdownButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private LeanToggle toggleCache;

        public LeanToggle Toggle
        {
            get
            {
                if (toggleCache == null)
                    toggleCache = GetComponent<LeanToggle>();

                return toggleCache;
            }
        }

        public UnityEvent OnEnter;
        public UnityEvent OnExit;


        public void OnPointerEnter(PointerEventData eventData)
        {
            OnEnter.Invoke();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnExit.Invoke();
        }
    }
}