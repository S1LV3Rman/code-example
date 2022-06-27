using System;
using System.Collections;
using UnityEngine;

namespace Source
{
    public class IlluminationSensor : Module, ISensor
    {
        [SerializeField] private Camera camera;
        [SerializeField] [Range(0.01f, 1f)] private float updateFrequency = 0.1f;

        private Texture2D _texture;
        private bool _isPlaying;
        private Color _illumination;
        
        public event Action<BotPort, int> OnValueChange;

        private void OnEnable()
        {
            StartCoroutine(SensorLoop());
        }

        private void OnDisable()
        {
            _isPlaying = false;
        }

        private IEnumerator SensorLoop()
        {
            _isPlaying = true;
            
            while (_isPlaying)
            {
                _illumination = GetIllumination();
                OnValueChange?.Invoke(Port, (int) (_illumination.grayscale * 1023f));
                yield return new WaitForSeconds(updateFrequency);
            }
        }

        private Color GetIllumination()
        {
            RenderTexture currentRT = RenderTexture.active;
            RenderTexture.active = camera.targetTexture;
            camera.Render();
            
            var targetTexture = camera.targetTexture;
            _texture = new Texture2D(targetTexture.width, targetTexture.height);
            _texture.ReadPixels(new Rect(0, 0, targetTexture.width, targetTexture.height), 0, 0);
            _texture.Apply();

            var pixels = _texture.GetPixels();
            var color = new Color();

            var count = pixels.Length;
            foreach (var pixel in pixels)
                color += pixel / count;
            
            RenderTexture.active = currentRT;

            return color;
        }
    }
}