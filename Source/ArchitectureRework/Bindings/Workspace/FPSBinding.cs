using UnityEngine;

namespace Source
{
    public class FPSBinding : IRunBinding, IInitBinding
    {
        private readonly AmplitudeService _amplitude;
        
        private float _fpsRefreshRate = 60f;
        private float _timer;
        private int _fps;

        public FPSBinding(AmplitudeService amplitude)
        {
            _amplitude = amplitude;
        }
        
        public void Init()
        {
            _timer = Time.unscaledTime;
        }
        
        public void Run()
        {
            if (Time.unscaledTime > _timer)
            {
                _fps = (int)(1f / Time.unscaledDeltaTime);

                _amplitude.SendEvent("fps", new Property("value", _fps));
                
                _timer += _fpsRefreshRate;
            }
        }

    }
}