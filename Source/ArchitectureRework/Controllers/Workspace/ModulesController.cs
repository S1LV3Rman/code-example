using System.Collections.Generic;

namespace Source
{
    public class ModulesController
    {
        private readonly ModulesView _view;
        
        public readonly List<Module> Installed;
        
        public ModulesController(ModulesView view)
        {
            Installed = new List<Module>();
            
            _view = view;
        }

        public void ClearModules()
        {
            Installed.Clear();
        }

        public void AddModule(Module module)
        {
            Installed.Add(module);
        }

        public void Reset()
        {
            foreach (var module in Installed)
                if (module is IIndicator indicator)
                    indicator.Reset();
        }

        public void Enable()
        {
            
        }

        public void Disable()
        {
            
        }
    }
}