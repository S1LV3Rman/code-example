using System.Collections.Generic;

namespace Source
{
    public class AppCore
    {
        private List<IInitBinding> _initBindings;
        private List<IRunBinding> _runBindings;

        public AppCore()
        {
            _initBindings = new List<IInitBinding>();
            _runBindings = new List<IRunBinding>();
        }
        
        public AppCore Add(IBinding binding)
        {
            if (binding is IInitBinding initSystem)
                _initBindings.Add(initSystem);
            
            if (binding is IRunBinding runSystem)
                _runBindings.Add(runSystem);
            
            return this;
        }

        public void Init()
        {
            foreach (var binding in _initBindings)
                binding.Init();
        }

        public void Run()
        {
            foreach (var binding in _runBindings)
                binding.Run();
        }
    }
}