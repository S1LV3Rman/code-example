using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Source
{
    public class BE2_Cst_MoveStop : BE2_InstructionBase, I_BE2_Instruction
    {
        private CircuitBoard board;

        private string _name = "move-stop";
    
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

        public void Function()
        {
            board.SetMotorsPower(0, 0);

            ExecuteNextInstruction();
        }
    }
}
