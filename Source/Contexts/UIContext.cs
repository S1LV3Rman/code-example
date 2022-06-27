using System;
using UnityEngine;

namespace Source
{
    [Serializable]
    public class UIContext
    {
        public TopToolbarView TopToolbar;
        public LeftToolbarView LeftToolbar;
        public BlockSelectionView BlockSelection;
        public WorkspaceView Workspace;
        public PlayAreaView PlayArea;
        public ConfiguratorView Configurator;
        public ConsoleView Console;
    }
}