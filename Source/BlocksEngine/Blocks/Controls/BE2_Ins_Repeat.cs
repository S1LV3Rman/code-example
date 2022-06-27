using System.Collections;
using System.Collections.Generic;
using Source;
using UnityEngine;

public class BE2_Ins_Repeat : BE2_InstructionBase, I_BE2_Instruction
{
    I_BE2_BlockSectionHeaderInput _input0;
    int _counter = 0;
    float _value;

    private string _name = "repeat";

    protected override void OnButtonStop()
    {
        _counter = 0;
        //EndLoop = false;
    }

    public override void OnStackActive()
    {
        AmplitudeService.SendEvent_Bad("play-block",
            new Property("type", _name));
        _counter = 0;
        //EndLoop = false;
    }

    protected override void OnAwake()
    {
        AmplitudeService.SendEvent_Bad("pick-block",
            new Property("type", _name));
    }

    public void Function()
    {
        _input0 = Section0Inputs[0];
        _value = _input0.FloatValue;

        if (_counter != _value)
        {
            _counter++;
            ExecuteSection(0);
        }
        else
        {
            //EndLoop = true;
            _counter = 0;
            ExecuteNextInstruction();
        }
    }
}
