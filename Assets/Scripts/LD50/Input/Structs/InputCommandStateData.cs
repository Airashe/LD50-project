using Airashe.UCore.Systems.Input;
using LD50.Input.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD50.Input.Structs
{
    public struct InputCommandStateData
    {
        public InputCommandType CommandType => commandType;
        private InputCommandType commandType;

        public bool IsActive => isActive;
        private bool isActive;

        public InputCommandStateData(InputCommandType commandType, bool isActive)
        {
            this.commandType = commandType;
            this.isActive = isActive;
        }

        public static implicit operator InputCommandStateData (InputCommandStateChanged originalData)
        {
            return new InputCommandStateData((InputCommandType)originalData.CommandTypeIndex, originalData.IsActive);
        }
    }
}
