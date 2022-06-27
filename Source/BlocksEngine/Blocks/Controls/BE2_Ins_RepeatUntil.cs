using System.Collections;
using System.Collections.Generic;
using Source;
using UnityEngine;

public class BE2_Ins_RepeatUntil : BE2_InstructionBase, I_BE2_Instruction
{
    I_BE2_BlockSectionHeaderInput _input0;
    string _value;

    private string _name = "repeat-until";
    
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

    public void Function()
    {
        _input0 = Section0Inputs[0];
        _value = _input0.StringValue;

        if (_value != "1" && _value != "true")
        {
            ExecuteSection(0);
        }
        else
        {
            ExecuteNextInstruction();
        }
    }
}
