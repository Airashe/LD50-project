using Airashe.UCore.Systems.Input;
using UnityEngine;

namespace LD50.Input.Commands
{
    public sealed class MoveForwardCommand : InputCommand
    {
        public MoveForwardCommand(int commandId) : base(commandId) => this.typeIndex = (int)InputCommandType.MoveForward;
        public override KeyCode Key { get => KeyCode.W; set { return; } }

        public override int TypeIndex => typeIndex;
        private int typeIndex;
    }
}
