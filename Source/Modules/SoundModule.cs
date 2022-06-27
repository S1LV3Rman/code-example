using System;
using UnityEngine;

namespace Source
{
    public class SoundModule : Module, IIndicator
    {
        [SerializeField] private ProceduralAudio audio;
        [SerializeField] private float sensitivity = 1f;

        private void Awake()
        {
            Reset();
        }

        public void SetValue(int value)
        {
            audio.frequency = value * sensitivity;
        }

        public void Reset()
        {
            audio.frequency = 0;
        }
    }
}