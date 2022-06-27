using Lean.Gui;
using UnityEngine;

namespace Source
{
    public class MenusView : MonoBehaviour
    {
        [Header("Window Closer")]
        public LeanWindowCloser WindowCloser;
        
        [Header("Blocker")]
        public Transform Blocker;
        public LeanButton BlockerButton;
        public CanvasGroup BlockerCanvas;
    }
}