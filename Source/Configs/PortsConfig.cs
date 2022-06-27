using UnityEngine;

namespace Source
{
    [CreateAssetMenu(fileName = "PortsConfig", menuName = "Config/Ports", order = 0)]
    public class PortsConfig : ScriptableObject
    {
        [Header("Default")]
        public Color DefaultBackgroundOff;
        public Color DefaultBackgroundOn;
        public Color DefaultOutline;
        
        [Header("Sensor")]
        public Color SensorBackgroundOff;
        public Color SensorBackgroundOn;
        public Color SensorOutline;
        
        [Header("Indicator")]
        public Color IndicatorBackgroundOff;
        public Color IndicatorBackgroundOn;
        public Color IndicatorOutline;
    }
}