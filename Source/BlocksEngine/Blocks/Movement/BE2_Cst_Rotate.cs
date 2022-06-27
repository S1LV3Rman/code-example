using System;
using Source;

public class BE2_Cst_Rotate : BE2_InstructionBase, I_BE2_Instruction
{
    private CircuitBoard board;

    private string _direction;
    private int _speed;

    private string _name = "rotate";
    
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
        _direction = Section0Inputs[0].StringValue;
        _speed = (int) Section0Inputs[1].FloatValue;

        switch (_direction)
        {
            case "направо":
                board.SetMotorsPower(_speed, -_speed);
                break;
            case "налево":
                board.SetMotorsPower(-_speed, _speed);
                break;
            default:
                throw new ArgumentOutOfRangeException(_direction);
        }

        ExecuteNextInstruction();
    }
}
