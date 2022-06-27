using System;
using System.Collections;
using System.Collections.Generic;
using Source;
using UnityEngine;
using UnityEngine.UI;

public class BE2_Cst_Console : BE2_InstructionBase, I_BE2_Instruction
{
    private CircuitBoard board;

    private float _timeout = 0.1f;
    private float _timeLeft;
    private bool _play;

    private string _name = "console";
    
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

    protected override void OnTargetChanged()
    {
        if (TargetObject is CircuitBoard omegaBotTarget)
            board = omegaBotTarget;
    }

    protected override void OnButtonPlay()
    {
        _play = true;
    }

    protected override void OnButtonStop()
    {
        _play = false;
    }

    private void Update()
    {
        if(_play)
            _timeLeft -= Time.deltaTime;
    }

    public void Function()
    {
        if (_timeLeft < 0)
        {
            var message = Section0Inputs[0].StringValue;
            board.SendMessageToConsole(message);
            _timeLeft = _timeout;
        }

        ExecuteNextInstruction();
    }
}
