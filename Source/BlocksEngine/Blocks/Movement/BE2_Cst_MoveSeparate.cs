using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Source
{
    public class BE2_Cst_MoveSeparate : BE2_InstructionBase, I_BE2_Instruction
    {
        private CircuitBoard board;

        private string _direction;
        private int _speedLeft;
        private int _speedRight;

        private string _name = "move-separate";
    
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
            _speedRight = (int) Section0Inputs[0].FloatValue;
            _speedLeft = (int) Section0Inputs[1].FloatValue;

            board.SetMotorsPower(_speedLeft, _speedRight);

            ExecuteNextInstruction();
        }
    }
}
