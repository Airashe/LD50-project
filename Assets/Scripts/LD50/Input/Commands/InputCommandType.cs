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
        MoveForward = 1, 
        MoveBackward = 2, 
        MoveLeft = 3, 
        MoveRight = 4, 

        Interact = 5, 

        MovementMask = MoveForward | MoveBackward | MoveLeft | MoveRight, 
    }
}
