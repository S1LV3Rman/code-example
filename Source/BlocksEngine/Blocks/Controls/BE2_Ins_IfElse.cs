using System.Collections;
using System.Collections.Generic;
using Source;
using UnityEngine;

public class BE2_Ins_IfElse : BE2_InstructionBase, I_BE2_Instruction
{
    I_BE2_BlockSectionHeaderInput _input0;
    string _value;
    bool _isFirstPlay = true;

    private string _name = "if-else";

    protected override void OnButtonStop()
    {
        _isFirstPlay = true;
    }

    public override void OnStackActive()
    {
        AmplitudeService.SendEvent_Bad("play-block",
            new Property("type", _name));
        _isFirstPlay = true;
    }

    protected override void OnAwake()
    {
        AmplitudeService.SendEvent_Bad("pick-block",
            new Property("type", _name));
    }

    public void Function()
    {
        if (_isFirstPlay)
        {
            _input0 = Section0Inputs[0];
            _value = _input0.StringValue;
            //EndLoop = true;

            if (_value == "1" || _value.ToLower() == "true")
            {
                _isFirstPlay = false;
                ExecuteSection(0);
            }
            else
            {
                _isFirstPlay = false;
                ExecuteSection(1);
            }
        }
        else
        {
            _isFirstPlay = true;
            ExecuteNextInstruction();
        }
    }
}
