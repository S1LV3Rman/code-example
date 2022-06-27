using System.Collections;
using System.Collections.Generic;
using Source;
using UnityEngine;

public class BE2_Op_BiggerThan : BE2_InstructionBase, I_BE2_Instruction
{
    I_BE2_BlockSectionHeaderInput _input0;
    I_BE2_BlockSectionHeaderInput _input1;
    BE2_InputValues _v0;
    BE2_InputValues _v1;

    private string _name = "bigger-than";
    
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

    public string Operation()
    {
        _input0 = Section0Inputs[0];
        _input1 = Section0Inputs[1];
        _v0 = _input0.InputValues;
        _v1 = _input1.InputValues;

        if (_v0.isText || _v1.isText)
        {
            return _v0.stringValue.Length > _v1.stringValue.Length ? "1" : "0";
        }
        else
        {
            return _v0.floatValue > _v1.floatValue ? "1" : "0";
        }
    }
}
