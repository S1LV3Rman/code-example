using System.Collections;
using System.Collections.Generic;
using Source;
using UnityEngine;

public class BE2_Ins_Wait : BE2_InstructionBase, I_BE2_Instruction
{
    I_BE2_BlockSectionHeaderInput _input0;
    bool _firstPlay = true;
    float _counter = 0;

    private string _name = "wait";
    
    public bool ExecuteInUpdate => true;

    protected override void OnButtonStop()
    {
        _firstPlay = true; 
        _counter = 0;
    }
    
    public override void OnStackActive()
    {
        AmplitudeService.SendEvent_Bad("play-block",
            new Property("type", _name));
        _firstPlay = true;
        _counter = 0;
    }

    protected override void OnAwake()
    {
        AmplitudeService.SendEvent_Bad("pick-block",
            new Property("type", _name));
    }

    public void Function()
    {
        if (_firstPlay)
        {
            _input0 = Section0Inputs[0];
            _counter = _input0.FloatValue;
            _firstPlay = false;
        }

        if (_counter > 0)
        {
            _counter -= Time.deltaTime;
        }
        else
        {
            _counter = 0;
            ExecuteNextInstruction();
            _firstPlay = true;
        }
    }
}
