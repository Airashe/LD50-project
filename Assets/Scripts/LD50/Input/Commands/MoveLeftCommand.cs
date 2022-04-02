using Airashe.UCore.Systems.Input;
using UnityEngine;

namespace LD50.Input.Commands
{
    public sealed class MoveLeftCommand : InputCommand
    {
        public MoveLeftCommand(int commandId) : base(commandId) => this.typeIndex = (int)InputCommandType.MoveLeft;
        public override KeyCode Key { get => KeyCode.A; set { return; } }

        public override int TypeIndex => typeIndex;
        private int typeIndex;
    }
}
