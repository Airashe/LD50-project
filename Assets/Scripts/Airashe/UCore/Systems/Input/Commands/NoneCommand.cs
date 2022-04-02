using UnityEngine;

namespace Airashe.UCore.Systems.Input.Commands
{
    /// <summary>
    /// Отсутствующая команда.
    /// </summary>
    public class NoneCommand : InputCommand
    {
        public override KeyCode Key
        {
            get => KeyCode.None;
            set
            {
                return;
            }
        }

        public override int TypeIndex => 0;

        /// <summary>
        /// Отсутствующая команда.
        /// </summary>
        /// <param name="commandId">Индекс команды.</param>
        public NoneCommand(int commandId) : base(commandId)
        {
        }
    }
}
