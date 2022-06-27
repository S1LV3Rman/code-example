﻿using System.Collections;
using System.Collections.Generic;
using Source;
using UnityEngine;

public class BE2_Op_Not : BE2_InstructionBase, I_BE2_Instruction
{
    I_BE2_BlockSectionHeaderInput _input0;
    BE2_InputValues _v0;

    private string _name = "not";
    
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
        _v0 = _input0.InputValues;
        string vs0 = _v0.stringValue;

        if (vs0 == "1" || vs0 == "true")
        {
            return "0";
        }
        else if (vs0 == "0" || vs0 == "false")
        {
            return "1";
        }
        else
        {
            if (_v0.isText)
            {
                char[] charArray = vs0.ToCharArray();
                System.Array.Reverse(charArray);

                return new string(charArray);
            }
            else
            {
                float result = _v0.floatValue * -1;
                return result.ToString();
            }
        }
    }
}