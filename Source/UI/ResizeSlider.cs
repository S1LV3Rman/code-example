using System;
using Lean.Gui;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Source
{
    [RequireComponent(typeof(LeanButton))]
    public class ResizeSlider : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private LeanButton buttonCache;

        public LeanButton Button
        {
            get
            {
                if (buttonCache == null)
                    buttonCache = GetComponent<LeanButton>();

                return buttonCache;
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