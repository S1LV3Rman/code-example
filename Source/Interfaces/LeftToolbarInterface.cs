using System;

namespace Source
{
    public class LeftToolbarInterface
    {
        private LeftToolbarView _view;

        public event Action<BlockSectionType> OnSectionSelected;
        public event Action<BlockSectionType> OnSectionDeselected;
            
        public event Action<bool> OnWorkspaceToggle;
        
        public LeftToolbarInterface(LeftToolbarView view)
        {
            _view = view;

            SubscribeWorkspaceButton();
            SubscribeSectionSelection();
        }

        public void SetWorkspaceToggle(bool value)
        {
            UnsubscribeWorkspaceButton();
            
            _view.WorkspaceButton.Set(value);
            
            SubscribeWorkspaceButton();
        }

        public void SetSectionToggle(BlockSectionType sectionType, bool value)
        {
            UnsubscribeSectionSelection();
            
            switch (sectionType)
            {
                case BlockSectionType.Controls:
                    _view.ControlsSection.Set(value);
                    break;
                case BlockSectionType.Movement:
                    _view.MovementSection.Set(value);
                    break;
                case BlockSectionType.Rangefinder:
                    _view.RangefinderSection.Set(value);
                    break;
                case BlockSectionType.Sensors:
                    _view.SensorsSection.Set(value);
                    break;
                case BlockSectionType.Indicators:
                    _view.IndicatorsSection.Set(value);
                    break;
                case BlockSectionType.Operators:
                    _view.OperationsSection.Set(value);
                    break;
                case BlockSectionType.Variables:
                    _view.VariablesSection.Set(value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(sectionType), sectionType, null);
            }
            
            SubscribeSectionSelection();
        }

        private void SubscribeWorkspaceButton()
        {
            _view.WorkspaceButton.OnOn.AddListener(WorkspaceSelected);
            _view.WorkspaceButton.OnOff.AddListener(WorkspaceDeselected);
        }

        private void UnsubscribeWorkspaceButton()
        {
            _view.WorkspaceButton.OnOn.RemoveAllListeners();
            _view.WorkspaceButton.OnOff.RemoveAllListeners();
        }

        private void SubscribeSectionSelection()
        {
            _view.ControlsSection.OnOn.AddListener(() => SectionSelected(BlockSectionType.Controls));
            _view.ControlsSection.OnOff.AddListener(() => SectionDeselected(BlockSectionType.Controls));
            
            _view.MovementSection.OnOn.AddListener(() => SectionSelected(BlockSectionType.Movement));
            _view.MovementSection.OnOff.AddListener(() => SectionDeselected(BlockSectionType.Movement));
            
            _view.RangefinderSection.OnOn.AddListener(() => SectionSelected(BlockSectionType.Rangefinder));
            _view.RangefinderSection.OnOff.AddListener(() => SectionDeselected(BlockSectionType.Rangefinder));
            
            _view.SensorsSection.OnOn.AddListener(() => SectionSelected(BlockSectionType.Sensors));
            _view.SensorsSection.OnOff.AddListener(() => SectionDeselected(BlockSectionType.Sensors));
            
            _view.IndicatorsSection.OnOn.AddListener(() => SectionSelected(BlockSectionType.Indicators));
            _view.IndicatorsSection.OnOff.AddListener(() => SectionDeselected(BlockSectionType.Indicators));
            
            _view.OperationsSection.OnOn.AddListener(() => SectionSelected(BlockSectionType.Operators));
            _view.OperationsSection.OnOff.AddListener(() => SectionDeselected(BlockSectionType.Operators));
            
            _view.VariablesSection.OnOn.AddListener(() => SectionSelected(BlockSectionType.Variables));
            _view.VariablesSection.OnOff.AddListener(() => SectionDeselected(BlockSectionType.Variables));
        }

        private void UnsubscribeSectionSelection()
        {
            _view.ControlsSection.OnOn.RemoveAllListeners();
            _view.ControlsSection.OnOff.RemoveAllListeners();
            
            _view.MovementSection.OnOn.RemoveAllListeners();
            _view.MovementSection.OnOff.RemoveAllListeners();
            
            _view.RangefinderSection.OnOn.RemoveAllListeners();
            _view.RangefinderSection.OnOff.RemoveAllListeners();
            
            _view.SensorsSection.OnOn.RemoveAllListeners();
            _view.SensorsSection.OnOff.RemoveAllListeners();
            
            _view.IndicatorsSection.OnOn.RemoveAllListeners();
            _view.IndicatorsSection.OnOff.RemoveAllListeners();
            
            _view.OperationsSection.OnOn.RemoveAllListeners();
            _view.OperationsSection.OnOff.RemoveAllListeners();
            
            _view.VariablesSection.OnOn.RemoveAllListeners();
            _view.VariablesSection.OnOff.RemoveAllListeners();
        }

        private void WorkspaceSelected()
        {
            OnWorkspaceToggle?.Invoke(true);
        }

        private void WorkspaceDeselected()
        {
            OnWorkspaceToggle?.Invoke(false);
        }

        private void SectionSelected(BlockSectionType sectionType)
        {
            OnSectionSelected?.Invoke(sectionType);
        }
        
        private void SectionDeselected(BlockSectionType sectionType)
        {
            OnSectionDeselected?.Invoke(sectionType);
        }
    }
}