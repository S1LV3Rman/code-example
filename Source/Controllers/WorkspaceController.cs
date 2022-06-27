using System;
using System.IO;
using System.Linq;
using Crosstales.FB;
using Sirenix.Utilities;
using UnityEngine;

namespace Source
{
    public class WorkspaceController : IController
    {
        private WorkspaceInterface _workspace;
        private BlockSelectionInterface _blockSelection;
        private LeftToolbarInterface _leftToolbar;
        private TopToolbarInterface _topToolbar;
        private readonly AmplitudeService _amplitude;

        private BlockSectionsConfig _sectionsConfig;
        private CursorsConfig _cursorsConfig;

        private BlockSectionType _currentSection;
        private bool _resizing;
        private bool _resizerHovered;
        private bool _resizeWorkspace;
        private float _resizeStartWidth;
        private Vector3 _resizeStartPos;

        private string _currentFile;
        private string _saveDirectory;
        
        public event Action OnSwitchToStartScreen;

        public WorkspaceController(
            WorkspaceInterface workspace, BlockSelectionInterface blockSelection, LeftToolbarInterface leftToolbar, TopToolbarInterface topToolbar,
            AmplitudeService amplitude,BlockSectionsConfig sectionsConfig, CursorsConfig cursorsConfig)
        {
            _workspace = workspace;
            _blockSelection = blockSelection;
            _leftToolbar = leftToolbar;
            _topToolbar = topToolbar;
            _amplitude = amplitude;
            _sectionsConfig = sectionsConfig;
            _cursorsConfig = cursorsConfig;

            _currentSection = BlockSectionType.None;

            _saveDirectory = BE2_Paths.TranslateMarkupPath(BE2_Paths.SavedCodesPath);
            if (!Directory.Exists(_saveDirectory))
                Directory.CreateDirectory(_saveDirectory);
        }
        
        public void Init()
        {
            _topToolbar.OnBack += SwitchToStartScreen;
            _topToolbar.OnQuitClick += Quit;
            _topToolbar.OnOpenClick += OpenFile;
            _topToolbar.OnAddClick += AddFile;
            _topToolbar.OnSaveClick += SaveFile;
            _topToolbar.OnSaveAsClick += SaveFileAs;
            
            _leftToolbar.OnWorkspaceToggle += ToggleWorkspace;
            _leftToolbar.OnSectionSelected += OpenSection;
            _leftToolbar.OnSectionDeselected += CloseSection;

            _workspace.OnClearPressed += Clear;
            _workspace.OnResizeBegin += BeginWorkspaceResizing;
            _workspace.OnResizeEnd += EndResizing;
            _workspace.OnResizerEnter += ResizerEnter;
            _workspace.OnResizerExit += ResizerExit;
            //todo: Move constant to config
            _workspace.Width = _workspace.ParentWidth * 0.6f;

            _blockSelection.OnResizeBegin += BeginBlockSectionsResizing;
            _blockSelection.OnResizeEnd += EndResizing;
            _blockSelection.OnResizerEnter += ResizerEnter;
            _blockSelection.OnResizerExit += ResizerExit;
            //todo: Move constant to config
            _blockSelection.Width = _blockSelection.ParentWidth * 0.2f;

            if (App.Shared.Data.TryPop<FilePath>(out var fileToOpen))
            {
                _workspace.ClearEnvironment();
                BE2_BlocksSerializer.LoadCode(fileToOpen.Value, _workspace.BE2Program);
                _currentFile = fileToOpen.Value;
            }
        }

        private void Clear()
        {
            _amplitude.SendEvent("clear-workspace");
            _workspace.ClearEnvironment();
        }

        private void SwitchToStartScreen()
        {
            OnSwitchToStartScreen?.Invoke();
        }

        private void LoadFile(bool increment)
        {
            var fileToOpen = FileBrowser.OpenSingleFile("Выберите файл", _saveDirectory, "BE2");
            if (fileToOpen.IsNullOrWhitespace())
                return;
            
            if (!increment)
                _workspace.ClearEnvironment();

            var extension = Path.GetExtension(fileToOpen);
            switch (extension)
            {
                case ".BE2":
                    BE2_BlocksSerializer.LoadCode(fileToOpen, _workspace.BE2Program);
                    if (!increment)
                        _currentFile = fileToOpen;
                    _amplitude.SendEvent("file-load-success");
                    return;
                default:
                    //todo: "Invalid extension" notification
                    break;
            }

            _amplitude.SendEvent("file-load-fail");
        }

        private void AddFile()
        {
            _amplitude.SendEvent("file-add");
            LoadFile(true);
        }

        private void OpenFile()
        {
            _amplitude.SendEvent("file-open");
            LoadFile(false);
        }

        private void SaveFile()
        {
            _amplitude.SendEvent("file-save");
            if (_currentFile.IsNullOrWhitespace())
            {
                SaveFileAs();
            }
            else
            {
                BE2_BlocksSerializer.SaveCode(_currentFile, _workspace.BE2Program);
                _amplitude.SendEvent("file-save-success");
            }
        }

        private void SaveFileAs()
        {
            var savePath = FileBrowser.SaveFile("Сохранение программы", _saveDirectory, null, "BE2");
            if (savePath.IsNullOrWhitespace())
                return;
            
            BE2_BlocksSerializer.SaveCode(savePath, _workspace.BE2Program);
            _currentFile = savePath;

            _amplitude.SendEvent("file-save-success");
        }

        private void Quit()
        {
            _amplitude.SendEvent("quit-workspace");
            Application.Quit();
        }

        public void Run()
        {
            if (_resizing)
            {
                var mousePos = Input.mousePosition;
                var delta = mousePos.x - _resizeStartPos.x;

                //todo: Move constants to config
                if(_resizeWorkspace)
                    _workspace.Width = Mathf.Clamp(_resizeStartWidth + delta,
                        _workspace.ParentWidth * 0.3f, _workspace.ParentWidth * 1.0f);
                else
                    _blockSelection.Width = Mathf.Clamp(_resizeStartWidth + delta,
                        _blockSelection.ParentWidth * 0.1f, _blockSelection.ParentWidth * 0.225f);
            }
        }

        private void ResizerEnter()
        {
            _resizerHovered = true;
            SetResizeCursor();
        }

        private void ResizerExit()
        {
            _resizerHovered = false;
            if(!_resizing)
                SetDefaultCursor();
        }

        private void SetResizeCursor()
        {
            Cursor.SetCursor(_cursorsConfig.HResize, new Vector2(16f, 16f), CursorMode.Auto);
        }

        private void SetDefaultCursor()
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }

        private void BeginWorkspaceResizing()
        {
            _resizeWorkspace = true;
            _resizeStartWidth = _workspace.Width;
            BeginResizing();
        }

        private void BeginBlockSectionsResizing()
        {
            _resizeWorkspace = false;
            _resizeStartWidth = _blockSelection.Width;
            BeginResizing();
        }

        private void BeginResizing()
        {
            _resizeStartPos = Input.mousePosition;
            _resizing = true;
        }

        private void EndResizing()
        {
            _resizing = false;
            if(!_resizerHovered)
                SetDefaultCursor();
        }

        private void OpenSection(BlockSectionType sectionType)
        {
            if (_currentSection != BlockSectionType.None && 
                _currentSection != sectionType)
            {
                _blockSelection.CloseSection(_currentSection);
                _leftToolbar.SetSectionToggle(_currentSection, false);
            }
            
            if(!_workspace.Active)
            {
                _leftToolbar.SetWorkspaceToggle(true);
                _workspace.Open();
            }
            
            var section = _sectionsConfig.blockSections[sectionType];
            
            _blockSelection.SetColor(section.color);
            _blockSelection.SetHeader(section.name);
            _blockSelection.OpenSection(sectionType);
            _currentSection = sectionType;
        }

        private void CloseSection(BlockSectionType sectionType)
        {
            _blockSelection.CloseSection(sectionType);
            _currentSection = BlockSectionType.None;
        }

        private void ToggleWorkspace(bool value)
        {
            if (value)
            {
                _workspace.Open();
                if(_currentSection != BlockSectionType.None)
                {
                    _blockSelection.OpenSection(_currentSection);
                    _leftToolbar.SetSectionToggle(_currentSection, true);
                }
            }
            else
            {
                _workspace.Close();
                if (_currentSection != BlockSectionType.None)
                {
                    _blockSelection.Close();
                    _leftToolbar.SetSectionToggle(_currentSection, false);
                }
            }
        }
    }
}