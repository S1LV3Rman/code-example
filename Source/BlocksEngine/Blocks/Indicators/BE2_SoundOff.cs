using System;
using Source;
using UnityEngine.UI;

public class BE2_SoundOff : BE2_InstructionBase, I_BE2_Instruction
{
    private PortDropdown _portDropdown;
    
    private CircuitBoard board;

    private string _name = "sound-off";
    
    public override void OnStackActive()
    {
        AmplitudeService.SendEvent_Bad("play-block",
            new Property("type", _name));
    }

    protected override void OnAwake()
    {
        AmplitudeService.SendEvent_Bad("pick-block",
            new Property("type", _name));
    
        var dropdown = Section0Inputs[0].Transform.GetComponent<Dropdown>();
        _portDropdown = new PortDropdown(dropdown);
    }

    protected override void OnTargetChanged()
    {
        if (TargetObject is CircuitBoard omegaBotTarget)
            board = omegaBotTarget;
    }

    public void Function()
    {
        board.SetPortValue(_portDropdown.Value, 0);

        ExecuteNextInstruction();
    }
}
