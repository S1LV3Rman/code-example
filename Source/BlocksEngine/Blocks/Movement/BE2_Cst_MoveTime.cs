using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Source
{
    public class BE2_Cst_MoveTime : BE2_InstructionBase, I_BE2_Instruction
    {
        private CircuitBoard board;

        private string _direction;
        private float _duration;
        private int _speed;

        private float _timePassed;

        private bool _running;

        private string _name = "move-time";
        
        public bool ExecuteInUpdate => true;

        protected override void OnTargetChanged()
        {
            if (TargetObject is CircuitBoard omegaBotTarget)
                board = omegaBotTarget;
        }

        public override void OnStackActive()
        {
            AmplitudeService.SendEvent_Bad("play-block",
                new Property("type", _name));
            _running = false;
        }

        protected override void OnAwake()
        {
            AmplitudeService.SendEvent_Bad("pick-block",
                new Property("type", _name));
        }

        public void Function()
        {
            if (!_running)
            {
                _direction = Section0Inputs[0].StringValue;
                _duration = Section0Inputs[1].FloatValue;
                _speed = (int) Section0Inputs[2].FloatValue;

                _timePassed = -Time.deltaTime;

                _running = true;
                
                switch (_direction)
                {
                    case "вперёд":
                        board.SetMotorsPower(_speed, _speed);
                        break;
                    case "назад":
                        board.SetMotorsPower(-_speed, -_speed);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(_direction);
                }
            }

            _timePassed += Time.deltaTime;
            if (_timePassed >= _duration)
            {
                board.SetMotorsPower(0, 0);
                _running = false;
                ExecuteNextInstruction();
            }
        }
    }
}
