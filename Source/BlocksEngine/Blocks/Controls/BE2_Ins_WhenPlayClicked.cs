using Source;

public class BE2_Ins_WhenPlayClicked : BE2_InstructionBase, I_BE2_Instruction
{
    private string _name = "when-play";
    
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
    
    protected override void OnButtonPlay()
    {
        BlocksStack.IsActive = true;
    }

    public void Function()
    {
        ExecuteSection(0);
    }
}
