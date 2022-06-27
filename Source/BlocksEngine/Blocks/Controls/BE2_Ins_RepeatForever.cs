using System.Collections;
using System.Collections.Generic;
using Source;
using UnityEngine;

public class BE2_Ins_RepeatForever : BE2_InstructionBase, I_BE2_Instruction
{
    private string _name = "repeat-forever";
    
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
        ExecuteSection(0);
    }
}
