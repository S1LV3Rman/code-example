using Lean.Gui;
using UnityEngine;

namespace Source
{
    public class PlayAreaView : MonoBehaviour
    {
        [Header("Camera controls")] 
        public LeanButton ZoomIn;
        public LeanButton ZoomOut;
        public LeanButton Focus;
        
        [Header("Camera modes")]
        public DropdownButton Modes;
        public DropdownButtonVariant FreeMode;
        public DropdownButtonVariant TopDownMode;
        public LeanToggle FollowMode;

        [Header("Controls")]
        public float CameraMoveDamping;
        public float CameraMoveSensitivity;
        public float CameraRotateDamping;
        public float BotMoveDamping;
        public float BotRotateDamping;
        public float BotRotateSensitivity;
    }
}