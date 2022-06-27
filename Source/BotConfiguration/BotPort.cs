

using System;

namespace Source
{
    public enum BotPort
    {
        None,
        A0,
        A1,
        A2,
        A3,
        D2,
        D3,
        D8,
        D9,
        D10,
        D11
    }

    public static class BotPortExtension
    {
        public static string ToFriendlyString(this BotPort port)
        {
            return port switch
            {
                BotPort.None => "--",
                _ => port.ToString()
            };
        }
        
        public static bool IsPWM(this BotPort port)
        {
            return port == BotPort.D3 ||
                   port == BotPort.D9 ||
                   port == BotPort.D10 ||
                   port == BotPort.D11;
        }
        
        public static bool IsDigital(this BotPort port)
        {
            return port == BotPort.D2 ||
                   port == BotPort.D8;
        }
        
        public static bool IsAnalog(this BotPort port)
        {
            return port == BotPort.A0 ||
                   port == BotPort.A1 ||
                   port == BotPort.A2 ||
                   port == BotPort.A3;
        }
    }
}