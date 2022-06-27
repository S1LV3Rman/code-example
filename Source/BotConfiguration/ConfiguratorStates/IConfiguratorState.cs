using System;

namespace Source
{
    public interface IConfiguratorState
    {
        public bool Running { get; }
        public void Start();
        public void Run();
        public void Stop();
    }
}