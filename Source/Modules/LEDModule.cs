using UnityEngine;

namespace Source
{
    public class LEDModule : Module, IIndicator
    {
        [SerializeField] private Light light;
        [SerializeField] private float sensitivity = 0.001f;

        private void Awake()
        {
            Reset();
        }

        public void SetValue(int value)
        {
            light.intensity = value * sensitivity;
        }

        public void Reset()
        {
            light.intensity = 0f;
        }
    }
}