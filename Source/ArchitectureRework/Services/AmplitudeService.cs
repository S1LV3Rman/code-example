using System.IO;
using System.Linq;
using AmplitudeAnalytics;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Networking;

namespace Source
{
    public class AmplitudeService
    {
        private readonly string _userIdPath;
        private readonly string _configPath;
        private static string _userId;
        
        public AmplitudeService()
        {
            _userIdPath = Application.persistentDataPath + "/UserId.txt";
            _configPath = Application.persistentDataPath + "/config.txt";
            
            InitializeUserId();
            
            Amplitude.Init("018e02a0987599f2a50b0b7fa4b029e6", _userId);
        }

        public UnityWebRequestAsyncOperation SendEvent(string eventName, params Property[] properties)
        {
            var amplitudeEvent = AmplitudeEvent.Builder
                .EventType(eventName)
                .UserProperty("id", _userId);

            foreach (var property in properties)
                amplitudeEvent.EventProperty(property.Name, property.Value);

            return Amplitude.Send(amplitudeEvent);
        }

        // todo: remove this garbage
        public static UnityWebRequestAsyncOperation SendEvent_Bad(string eventName, params Property[] properties)
        {
            var amplitudeEvent = AmplitudeEvent.Builder
                .EventType(eventName)
                .UserProperty("id", _userId);

            foreach (var property in properties)
                amplitudeEvent.EventProperty(property.Name, property.Value);

            return Amplitude.Send(amplitudeEvent);
        }

        private void InitializeUserId()
        {
            if (File.Exists(_configPath))
            {
                var config = File.ReadAllLines(_configPath);
                var disableAnalytics = config.FirstOrDefault(line => line.StartsWith("disable-analytics"));

                if (!disableAnalytics.IsNullOrWhitespace())
                {
                    Amplitude.Disable();
                    return;
                }
            }
            
            if (!File.Exists(_userIdPath))
            {
                _userId = SystemInfo.deviceUniqueIdentifier;
                File.WriteAllText(_userIdPath, _userId);
            }
            else
            {
                _userId = File.ReadAllText(_userIdPath);
            }
        }
    }

    public struct Property
    {
        public string Name;
        public string Value;

        public Property(string name, object value)
        {
            Name = name;
            Value = value.ToString().ToLowerInvariant();
        }
    }
}