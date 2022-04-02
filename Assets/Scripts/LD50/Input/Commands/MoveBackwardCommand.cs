using Airashe.UCore.Systems.Input;
using UnityEngine;

namespace LD50.Input.Commands
{
    public sealed class MoveBackwardCommand : InputCommand
    {
        public MoveBackwardCommand(int commandId) : base(commandId) => this.typeIndex = (int)InputCommandType.MoveBackward;
        public override KeyCode Key { get => KeyCode.S; set { return; } }
        public override int TypeIndex => typeIndex;
        private int typeIndex;
    }
}
