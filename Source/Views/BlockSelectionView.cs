using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Source
{
    public class BlockSelectionView : MonoBehaviour
    {
        public Graphic Background;
        public TMP_Text Header;
        public ResizeSlider ResizeSlider;

        [Header("Sections")]
        
        public BE2_UI_SelectionPanel Controls;
        public BE2_UI_SelectionPanel Movement;
        public BE2_UI_SelectionPanel Rangefinder;
        public BE2_UI_SelectionPanel Sensors;
        public BE2_UI_SelectionPanel Indicators;
        public BE2_UI_SelectionPanel Operators;
        public BE2_UI_SelectionPanel Variables;
    }
}