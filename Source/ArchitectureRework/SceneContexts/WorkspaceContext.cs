using System;
using UnityEngine;

namespace Source
{
    public class WorkspaceContext : SceneContext
    {
        [Header("Contexts")]
        public UIContext UI;
        public EnvironmentContext Environment;
        public BE2View BE2;
        public CircuitBoard CircuitBoard;
        
        [Header("Configs")]
        public BlockSectionsConfig sectionsConfig;
        public CursorsConfig cursorsConfig;
        public ModulesConfig modulesConfig;
        public PortsConfig portsConfig;
        public SlotsConfig slotsConfig;
    }
}