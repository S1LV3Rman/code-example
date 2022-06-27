using System;
using Sirenix.Utilities;
using UnityEngine;

namespace Source
{
    public class StartScreenBinding : IInitBinding
    {
        private readonly StartScreenController _startScreen;
        private readonly BlockerController _blocker;
        private readonly ConfirmExitMenuController _confirmExitMenu;
        private readonly GameModeSelectorMenuController _gameModeSelector;
        private readonly LoadingPlaceholderController _loading;
        private readonly FileSystemService _fileSystem;
        private readonly AmplitudeService _amplitude;


        private string _selectedFile;
        public event Action OnSwitchToWorkspace;
        
        public StartScreenBinding(StartScreenController startScreen, BlockerController blocker,
            ConfirmExitMenuController confirmExitMenu, GameModeSelectorMenuController gameModeSelector, LoadingPlaceholderController loading,
            FileSystemService fileSystem, AmplitudeService amplitude)
        {
            _startScreen = startScreen;
            _blocker = blocker;
            _confirmExitMenu = confirmExitMenu;
            _gameModeSelector = gameModeSelector;
            _loading = loading;
            _fileSystem = fileSystem;
            _amplitude = amplitude;
        }

        public void Init()
        {
            _loading.OnShow += ShowBlockerUnderLoading;
            _confirmExitMenu.OnShow += ShowBlockerUnderConfirm;
            _gameModeSelector.OnShow += ShowBlockerUnderSelector;
            
            _confirmExitMenu.OnResult += QuitApplication;
            _gameModeSelector.OnResult += SwitchToSelectedMode;

            _startScreen.View.Quit.OnClick.AddListener(OpenExitConfirmation);
            _startScreen.View.File.OnClick.AddListener(OpenFile);
            _startScreen.View.Site.OnClick.AddListener(OpenSite);
            
            // _startScreen.View.Tutorial.OnClick.AddListener(SwitchToWorkspace);
            // _startScreen.View.Missions.OnClick.AddListener(SwitchToWorkspace);
            _startScreen.View.Sandbox.OnClick.AddListener(SwitchToWorkspace);
        }

        private void SwitchToSelectedMode(GameModeSelectorResult result)
        {
            switch (result)
            {
                case GameModeSelectorResult.Sandbox:
                    App.Shared.Data.Push(new FilePath
                    {
                        Value = _selectedFile
                    });
                    _selectedFile = null;
                    _blocker.HideAll();
                    SwitchToWorkspace();
                    break;
                case GameModeSelectorResult.Close:
                    _blocker.ToPreviousMenu();
                    break;
            }
        }

        private void SwitchToWorkspace()
        {
            _loading.Open();
            _amplitude.SendEvent("to-polygon");
            OnSwitchToWorkspace?.Invoke();
        }

        private void ShowBlockerUnderLoading()
        {
            _blocker.ShowUnderMenu(_loading);
            _blocker.BlockClosing();
        }

        private void OpenExitConfirmation()
        {
            _confirmExitMenu.Open();
        }

        private void ShowBlockerUnderConfirm()
        {
            _blocker.ShowUnderMenu(_confirmExitMenu);
        }

        private void OpenFile()
        {
            _selectedFile = _fileSystem.OpenSaveFile();
            if (_selectedFile.IsNullOrWhitespace())
                return;
            
            _gameModeSelector.Open();
        }

        private void ShowBlockerUnderSelector()
        {
            _blocker.ShowUnderMenu(_gameModeSelector);
        }

        private void OpenSite()
        {
            _amplitude.SendEvent("site-open");
            Application.OpenURL("https://omegabot.ru/");
        }

        private void QuitApplication(bool quit)
        {
            if (quit)
            {
                _amplitude.SendEvent("quit-startscreen");
                Application.Quit();
            }
            _blocker.ToPreviousMenu();
        }
    }
}