using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Source
{
    public class WorkspaceState : IAppState<WorkspaceContext>
    {
        private App _app;
        private AppCore _core;
        public bool Initialized { get; private set; }

        private List<IController> _controllers;

        public WorkspaceState(App app)
        {
            app.SceneChanger.SwitchToWorkspace();
            _app = app;
        }

        public void Init(WorkspaceContext context)
        {
            var omegaBot = new OmegaBotInterface(context.Environment.OmegaBot);
            var mainCamera = new CameraInterface(context.Environment.MainCamera);
            var executionManager = new ExecutionManagerInterface(context.BE2.ExecutionManager);

            var leftToolbar = new LeftToolbarInterface(context.UI.LeftToolbar);
            var workspace = new WorkspaceInterface(context.UI.Workspace);
            var blockSelection = new BlockSelectionInterface(context.UI.BlockSelection);
            var topToolbar = new TopToolbarInterface(context.UI.TopToolbar);
            var playArea = new PlayAreaInterface(context.UI.PlayArea);
            var configurator = new ConfiguratorInterface(context.UI.Configurator);
            var console = new ConsoleInterface(context.UI.Console);

            var leftToolbarC = new LeftToolbarController(context.UI.LeftToolbar);
            var be2C = new BE2Controller(context.BE2);
            var modulesC = new ModulesController(context.Environment.Modules);
            var omegaBotC = new OmegaBotController(context.Environment.OmegaBot);

            var workspaceController = new WorkspaceController(workspace, blockSelection, leftToolbar, topToolbar,
                _app.Amplitude, context.sectionsConfig, context.cursorsConfig);
            workspaceController.OnSwitchToStartScreen += SwitchStateToStartScreen;
            
            _controllers = new List<IController>
            {
                workspaceController,
                new PlayAreaController(playArea, executionManager, mainCamera, omegaBot, _app.MouseInput, _app.Amplitude),
                new BotController(console, omegaBot, context.CircuitBoard, context.cursorsConfig),
                new ConfiguratorController(configurator, omegaBot, context.Environment.UICamera,
                    context.Environment.MainCamera, modulesC, context.modulesConfig, context.portsConfig, context.slotsConfig,
                    _app.MouseInput, _app.Amplitude)
            };
            
            _core = new AppCore();
            _core.Add(new FPSBinding(_app.Amplitude))
                 .Add(new PlayerBinding(leftToolbarC, be2C, modulesC, omegaBotC, _app.Amplitude));
            _core.Init();

            foreach (var controller in _controllers)
                controller.Init();

            Initialized = true;
        }

        public void Run()
        {
            _core.Run();
            
            foreach (var controller in _controllers)
                controller.Run();
        }

        private void SwitchStateToStartScreen()
        {
            _app.SwitchState(new StartSceneState(_app));
        }
    }
}