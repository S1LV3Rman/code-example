using Lean.Gui;
using UnityEngine;
using UnityEngine.UI;

namespace Source
{
    public class PortMarker : MonoBehaviour
    {
        [SerializeField] private Graphic backgroundOff;
        [SerializeField] private Graphic backgroundOn;
        [SerializeField] private Graphic outline;
        
        [SerializeField] private LeanToggle toggle;
        [SerializeField] private LeanButton button;

        public BotPort Type;
        public ID ID;

        public Color BackgroundOffColor
        {
            get => backgroundOff.color;
            set => backgroundOff.color = value;
        }

        public Color BackgroundOnColor
        {
            get => backgroundOn.color;
            set => backgroundOn.color = value;
        }

        public Color OutlineColor
        {
            get => outline.color;
            set => outline.color = value;
        }

        public LeanToggle Toggle => toggle;
        
        public LeanButton Button => button;
    }
}