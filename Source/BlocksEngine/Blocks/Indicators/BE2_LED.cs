using System;
using Source;
using UnityEngine.UI;

public class BE2_LED : BE2_InstructionBase, I_BE2_Instruction
{
    private PortDropdown _portDropdown;
    
    private CircuitBoard board;

    private string _name = "led";
    
    public override void OnStackActive()
    {
        AmplitudeService.SendEvent_Bad("play-block",
            new Property("type", _name));
    }

    protected override void OnAwake()
    {
        AmplitudeService.SendEvent_Bad("pick-block",
            new Property("type", _name));
        
        var dropdown = Section0Inputs[1].Transform.GetComponent<Dropdown>();
        _portDropdown = new PortDropdown(dropdown);
    }

    protected override void OnTargetChanged()
    {
        if (TargetObject is CircuitBoard omegaBotTarget)
            board = omegaBotTarget;
    }

    public void Function()
    {
        var state = Section0Inputs[0].StringValue;
        switch (state)
        {
            case "Вкл.":
                board.SetPortValue(_portDropdown.Value, 1);
                break;
            case "Выкл.":
                board.SetPortValue(_portDropdown.Value, 0);
                break;
            default:
                throw new ArgumentOutOfRangeException(state);
        }

        ExecuteNextInstruction();
    }
}
