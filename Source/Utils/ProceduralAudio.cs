using UnityEngine;

namespace Source
{ 
    //By Benjamin Outram
    //C# script for use in Unity 

    // create a new file and copy paste this.  Name the file:
    // ProceduralAudio.cs
    // attach script to a GameObject that has an AudioSource on it.
    // adjust the public variables to experiment.

    [RequireComponent(typeof(AudioLowPassFilter))]
    public class ProceduralAudio : MonoBehaviour
    {
        private float sampling_frequency = 48000;

        //for tonal part

        public float frequency = 440f;
        public float gain = 0.05f;

        private float increment;
        private float phase;

        void Awake()
        {
            sampling_frequency = AudioSettings.outputSampleRate;
        }

        void OnAudioFilterRead(float[] data, int channels)
        {
            float tonalPart = 0;

            // update increment in case frequency has changed
            increment = frequency * 2f * Mathf.PI / sampling_frequency;

            for (int i = 0; i < data.Length; i++)
            {
                phase = phase + increment;
                if (phase > 2 * Mathf.PI) phase = 0;

                //tone
                tonalPart = gain * Mathf.Sin(phase);

                //together
                data[i] = tonalPart;

                // if we have stereo, we copy the mono data to each channel
                if (channels == 2)
                {
                    data[i + 1] = data[i];
                    i++;
                }
            }
        }
    }
}