using UnityEngine;

namespace Source
{
    [CreateAssetMenu(fileName = "SlotsConfig", menuName = "Config/Slots", order = 0)]
    public class SlotsConfig : ScriptableObject
    {
        public Color Indicator;
        public Color Sensor;
        public Color Other;
    }
}