using System.Collections;
using System.Collections.Generic;
using Source;
using UnityEngine;
using UnityEngine.UI;

public class BE2_Ins_SetVariable : BE2_InstructionBase, I_BE2_Instruction
{
    private string _name = "variable-set";
    
    public override void OnStackActive()
    {
        AmplitudeService.SendEvent_Bad("play-block",
            new Property("type", _name));
    }

    protected override void OnAwake()
    {
        AmplitudeService.SendEvent_Bad("pick-block",
            new Property("type", _name));
    }
    
    protected override void OnStart()
    {
        _variablesManager = BE2_VariablesManager.instance;
        _dropdown = GetSectionInputs(0)[0].Transform.GetComponent<Dropdown>();
        _dropdown.onValueChanged.AddListener(delegate { _lastValue = _dropdown.options[_dropdown.value].text; });

        BE2_MainEventsManager.Instance.StartListening(BE2EventTypes.OnAnyVariableAddedOrRemoved, PopulateDropdown);

        // v2.1 - bugfix: fixed variable blocks not updating dropdown when new variables were crated
        PopulateDropdown();
    }

    void OnDisable()
    {
        BE2_MainEventsManager.Instance.StopListening(BE2EventTypes.OnAnyVariableAddedOrRemoved, PopulateDropdown);
    }

    void PopulateDropdown()
    {
        _dropdown.ClearOptions();
        foreach (KeyValuePair<string, string> variable in _variablesManager.variablesList)
        {
            _dropdown.options.Add(new Dropdown.OptionData(variable.Key));
        }
        _dropdown.RefreshShownValue();
        _dropdown.value = _dropdown.options.FindIndex(option => option.text == _lastValue);
    }

    string _lastValue;
    Dropdown _dropdown;
    BE2_VariablesManager _variablesManager;

    public void Function()
    {
        _variablesManager.AddOrUpdateVariable(Section0Inputs[0].StringValue, Section0Inputs[1].StringValue);
        ExecuteNextInstruction();
    }
}
