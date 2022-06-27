using Lean.Gui;
using UnityEngine;

namespace Source
{
    public class StartScreenView : MonoBehaviour
    {
        [Header("Game modes")]
        public LeanButton Tutorial;
        public LeanButton Missions;
        public LeanButton Sandbox;
        
        [Header("Other")]
        public LeanButton File;
        public LeanButton Site;
        public LeanButton Quit;
    }
}