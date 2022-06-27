using System;
using Lean.Touch;
using UnityEngine;

namespace Source
{
    [Serializable]
    public class EnvironmentContext
    {
        public CameraView MainCamera;
        public Camera UICamera;
        public OmegaBotView OmegaBot;
        public ModulesView Modules;
    }
}