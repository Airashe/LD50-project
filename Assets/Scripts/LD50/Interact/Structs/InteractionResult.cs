using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD50.Core.Interact
{
    public enum InteractionResult : byte
    {
        None = 0, 
        Success = 1, 
        NoInteractableItem = 2, 
        NotInUseRange = 3, 
        NotValidTarget = 4, 
    }
}
