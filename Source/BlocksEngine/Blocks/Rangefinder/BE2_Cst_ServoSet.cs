using System;
using System.Collections;
using System.Collections.Generic;
using Source;
using UnityEngine;
using UnityEngine.UI;

public class BE2_Cst_ServoSet : BE2_InstructionBase, I_BE2_Instruction
{
    private CircuitBoard board;
    
    private PortDropdown _portDropdown;

    private int _angle;
    private string _direction;

    private string _name = "servo";
    
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
        _angle = (int) Section0Inputs[0].FloatValue;
        _angle = Mathf.Clamp(_angle, 0, 180);
        board.SetPortValue(_portDropdown.Value, _angle);

        ExecuteNextInstruction();
    }
}
