using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Source
{
    public class App
    {
        public static App Shared { get; private set; }
        public AppData Data { get; private set; }
        
        private IAppState _currentState;
        private SceneContext _currentContext;
        private bool _waitingContext;
        
        
        public MouseInputService MouseInput { get; private set; }
        public FileSystemService FileSystem { get; private set; }
        public SceneChangerService SceneChanger { get; private set; }
        public AmplitudeService Amplitude { get; private set; }

        
        [RuntimeInitializeOnLoadMethod]
        private static void EntryPoint()
        {
            Shared = new App();
        }
        
        private App()
        {
            Init();
        }

        private void Init()
        {
            Data = new AppData();
            
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            
            var appCore = new GameObject("AppUpdate", typeof(AppUpdate));
            Object.DontDestroyOnLoad(appCore);
            
            MouseInput = new MouseInputService();
            SceneChanger = new SceneChangerService();
            FileSystem = new FileSystemService();
            Amplitude = new AmplitudeService();
            
            SendSystemInfo();
            
            _waitingContext = true;
            InitStateAsync();
        }

        private void SendSystemInfo()
        {
            var os = new Property("OS", SystemInfo.operatingSystem);
            var gpuName = new Property("GraphicsDeviceName", SystemInfo.graphicsDeviceName);
            var gpuVersion = new Property("GraphicsDeviceVersion", SystemInfo.graphicsDeviceVersion);
            var gpuMemory = new Property("GraphicsMemorySize", SystemInfo.graphicsMemorySize);
            var cpu = new Property("ProcessorType", SystemInfo.processorType);
            var ram = new Property("RAM", SystemInfo.systemMemorySize);
            
            Amplitude.SendEvent("application-start", os, gpuName, gpuVersion, gpuMemory, cpu, ram);
        }

        public void Run()
        {
            if (_currentState == null) return;
            
            if (_currentState.Initialized)
                _currentState.Run();
        }

        public void Destroy()
        {
            var operation = Amplitude.SendEvent("application-quit");
            
            while (!operation.isDone) 
                Thread.Sleep(15);
        }

        public void BindContext(SceneContext context)
        {
            _currentContext = context;
            _waitingContext = false;
        }
        
        private async void InitStateAsync()
        {
            while (_waitingContext)
                await Task.Delay(50);
            
            _currentState ??= _currentContext switch
            {
                StartScreenContext _ => new StartSceneState(this),
                WorkspaceContext _ => new WorkspaceState(this),
                _ => throw new ArgumentOutOfRangeException(nameof(_currentContext))
            };

            switch (_currentContext)
            {
                case StartScreenContext startScreenCore when _currentState is StartSceneState startScreenState:
                    startScreenState.Init(startScreenCore);
                    break;
                case WorkspaceContext workspaceCore when _currentState is WorkspaceState workspaceState:
                    workspaceState.Init(workspaceCore);
                    break;
                default:
                    throw new Exception($"Wrong state transition!");
            }
        }

        public void SwitchState(IAppState state)
        {
            _waitingContext = true;
            _currentState = state;
            InitStateAsync();
        }
    }
}