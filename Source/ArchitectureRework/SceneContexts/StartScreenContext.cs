using System;
using UnityEngine;

namespace Source
{
    public class StartScreenContext : SceneContext
    {
        public StartScreenView StartScreen;
        public MenusView Menus;
        public ConfirmExitMenuView ConfirmExit;
        public GameModeSelectorMenuView GameModeSelector;
        public LoadingPlaceholderView LoadingPlaceholder;
    }
}