using System;
using UnityEngine;

namespace Source
{
    public class PlayerBinding : IInitBinding
    {
        private readonly LeftToolbarController _leftToolbar;
        private readonly BE2Controller _be2;
        private readonly ModulesController _modules;
        private readonly OmegaBotController _omegaBot;
        private readonly AmplitudeService _amplitude;

        public PlayerBinding(LeftToolbarController leftToolbar, BE2Controller be2, ModulesController modules, OmegaBotController omegaBot, AmplitudeService amplitude)
        {
            _leftToolbar = leftToolbar;
            _be2 = be2;
            _modules = modules;
            _omegaBot = omegaBot;
            _amplitude = amplitude;
        }
        
        public void Init()
        {
            _leftToolbar.OnPlay += Play;
            _leftToolbar.OnStop += StopPress;
            _leftToolbar.OnReset += Reset;

        }

        private void Play()
        {
            _amplitude.SendEvent("play");
            _be2.Play();
            EnableModules();
        }

        private void StopPress()
        {
            _amplitude.SendEvent("stop");
            Stop();
        }

        private void Reset()
        {
            _amplitude.SendEvent("reset");
            
            Stop();

            _omegaBot.Reset();
            _modules.Reset();
        }

        private void Stop()
        {
            _be2.Stop();
            DisableModules();
        }

        private void EnableModules()
        {
            _modules.Enable();
            foreach (var module in _modules.Installed)
            {
                var moduleType = module switch
                {
                    ButtonModule _ => ModuleType.Button,
                    IlluminationSensor _ => ModuleType.Illumination,
                    LEDModule _ => ModuleType.LED,
                    LineSensor _ => ModuleType.Line,
                    Servo _ => ModuleType.Rangefinder,
                    SoundModule _ => ModuleType.Sound,
                    TouchSensor _ => ModuleType.Touch,
                    _ => throw new ArgumentOutOfRangeException(nameof(module))
                };

                _amplitude.SendEvent("play-module", new Property("module-type", moduleType));
            }
        }

        private void DisableModules()
        {
            _modules.Disable();
        }
    }
}