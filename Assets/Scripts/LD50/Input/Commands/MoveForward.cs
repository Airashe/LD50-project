using Airashe.UCore.Systems.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LD50.Input.Commands
{
    public sealed class MoveForward : InputCommand
    {
        public MoveForward(int commandId) : base(commandId)
        {
        }

        public override int TypeIndex => (int)InputCommandType.MoveForward;

        public override KeyCode Key { get => KeyCode.W; set { return; } }
    }
}
