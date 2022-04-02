using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD50.Input.Commands
{
    [Flags]
    public enum InputCommandType : int
    {
        None = 0, 
        MoveForward = 0x1, 
        MoveBackward = 0x2, 
        MoveLeft = 0x4, 
        MoveRight = 0x8, 

        Interact = 0x10, 

        MovementMask = MoveForward | MoveBackward | MoveLeft | MoveRight, 
    }
}
