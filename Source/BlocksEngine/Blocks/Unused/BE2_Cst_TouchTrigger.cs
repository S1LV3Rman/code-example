using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BE2_Cst_TouchTrigger : BE2_InstructionBase, I_BE2_Instruction
{
    // --- Method used to implement Function Blocks (will only be called by types: simple, condition, loop, trigger)
    public void Function()
    {
        ExecuteSection(0);
    }
}
