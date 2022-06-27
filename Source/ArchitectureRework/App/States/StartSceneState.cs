using System.Diagnostics;

namespace Source
{
    public class StartSceneState : IAppState<StartScreenContext>
    {
        private App _app;
        private AppCore _core;
        
        public bool Initialized { get; private set; }

        public StartSceneState(App app)
        {
            app.SceneChanger.SwitchToStartScreen();
            _app = app;
        }

        public void Init(StartScreenContext context)
        {
            var startScreen = new StartScreenController(context.StartScreen);
            var menus = new BlockerController(context.Menus);
            var confirmExit = new ConfirmExitMenuController(context.ConfirmExit);
            var gameModeSelector = new GameModeSelectorMenuController(context.GameModeSelector);
            var loading = new LoadingPlaceholderController(context.LoadingPlaceholder);
            
            var startScreenBinding = new StartScreenBinding(startScreen, menus, confirmExit, gameModeSelector, loading,
                _app.FileSystem, _app.Amplitude);
            startScreenBinding.OnSwitchToWorkspace += SwitchStateToWorkspace;
            
            _core = new AppCore();
            _core.Add(startScreenBinding);
            _core.Init();

            Initialized = true;
        }

        public void Run()
        {
            _core.Run();
        }

        private void SwitchStateToWorkspace()
        {
            _app.SwitchState(new WorkspaceState(_app));
        }
    }
}