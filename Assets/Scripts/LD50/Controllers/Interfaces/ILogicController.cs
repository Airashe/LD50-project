using LD50.Controllers;
using LD50.Core.Interact;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD50.Controllers.Interfaces
{
    public interface ILogicController
    {
        public IUIController UiController { get; }
        public Unit ControlledUnit { get; }

        public InteractionResult UseItemOnWorld<T>(T item);
    }
}
