using System;
using System.Collections;
using System.Collections.Generic;
using Source;
using UnityEngine;

public class BE2_Cst_ServoRotate : BE2_InstructionBase, I_BE2_Instruction
{
    private CircuitBoard board;

    private int _angle;
    private string _direction;

    protected override void OnTargetChanged()
    {
        if (TargetObject is CircuitBoard omegaBotTarget)
            board = omegaBotTarget;
    }

    public void Function()
    {
        _angle = (int) Section0Inputs[0].FloatValue;
        _direction = Section0Inputs[1].StringValue;


        ExecuteNextInstruction();
    }
}
