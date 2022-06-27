using System;
using System.Linq;
using UnityEngine.UI;

namespace Source
{
    public class PortDropdown
    {
        private Dropdown _dropdown;
        
        private BotPort _currentPort;
        public BotPort Value => _currentPort;

        public PortDropdown(Dropdown dropdown)
        {
            _dropdown = dropdown;
            
            _dropdown.onValueChanged.AddListener(i => ParsePort(_dropdown.options[i].text));
            
            _dropdown.ClearOptions();

            var allPorts = Enum.GetValues(typeof(BotPort)).Cast<BotPort>().ToArray();
            foreach (var port in allPorts)
                _dropdown.options.Add(new Dropdown.OptionData(port.ToFriendlyString()));
            
            _dropdown.value = _dropdown.options.FindIndex(option => option.text == "--");
            _dropdown.RefreshShownValue();
        }
        
        private void ParsePort(string portRaw)
        {
            BotPort port = BotPort.A0;
            try
            {
                port = (BotPort)Enum.Parse(typeof(BotPort), portRaw);
            }
            catch { }
            _currentPort = port;
        }
    }
}