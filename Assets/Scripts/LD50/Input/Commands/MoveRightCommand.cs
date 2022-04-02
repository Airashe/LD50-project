using Airashe.UCore.Systems.Input;
using UnityEngine;

namespace LD50.Input.Commands
{
    public sealed class MoveRightCommand : InputCommand
    {
        public MoveRightCommand(int commandId) : base(commandId) => this.typeIndex = (int)InputCommandType.MoveRight;
        public override KeyCode Key { get => KeyCode.D; set { return; } }

        public override int TypeIndex => typeIndex;
        private int typeIndex;
    }
}
