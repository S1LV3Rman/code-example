using UnityEngine;

namespace Source
{
    public class Module : Detail, IModule
    {
        [SerializeField] private BotPort port = BotPort.None;
        public BotPort Port
        {
            get => port;
            set => port = value;
        }
    }
}