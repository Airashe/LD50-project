using Airashe.UCore.Systems.Input;
using UnityEngine;

namespace LD50.Input.Commands
{
    public sealed class InteractCommand : InputCommand
    {
        public InteractCommand(int commandId) : base(commandId) => this.typeIndex = (int)InputCommandType.Interact;
        public override KeyCode Key { get => KeyCode.Mouse0; set { return; } }

        public override int TypeIndex => typeIndex;
        private int typeIndex;
    }
}
