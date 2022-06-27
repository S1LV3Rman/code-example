using System;

namespace Source
{
    public interface ISensor
    {
        public event Action<BotPort, int> OnValueChange;
    }
}