using System;
using UnityEngine;

namespace Airashe.UCore.Systems.Input
{
    /// <summary>
    /// Базовый класс для всех команд в игре.
    /// </summary>
    [Serializable]
    public abstract class InputCommand : IEquatable<InputCommand>
    {
        /// <summary>
        /// Идентификатор команды.
        /// </summary>
        public int CommandId => commandId;
        private int commandId;
        /// <summary>
        /// Активна ли команда.
        /// </summary>
        public bool IsActive
        {
            get => isActive;
            set => isActive = value;
        }
        private bool isActive;

        /// <summary>
        /// Индекс типа команды.
        /// </summary>
        public abstract int TypeIndex { get; }

        /// <summary>
        /// Клавиша, на которую активируется команда.
        /// </summary>
        public abstract KeyCode Key { get; set; }

        /// <summary>
        /// Базовый класс, для всех команд в игре.
        /// </summary>
        /// <param name="commandId">ID комманды.</param>
        protected InputCommand(int commandId)
        {
            this.commandId = commandId;
        }

        /// <summary>
        /// Сравнение экземпляра команды ввода с <paramref name="obj"/>.
        /// </summary>
        /// <param name="obj">Экземпляр команды ввода.</param>
        /// <returns>
        /// Возвращает <see langword="true"/> если <paramref name="obj"/> является объекто команды и идентификаторы команд равны. 
        /// В противном случае возвращает <see langword="false"/>.
        /// </returns>
        public override bool Equals(object obj)
        {
            var other = obj as InputCommand;
            if (other is null)
                return false;
            return Equals(other);
        }

        /// <summary>
        /// Сравнение экземпляра команды ввода с <paramref name="other"/>.
        /// </summary>
        /// <param name="other">Экземпляр команды ввода.</param>
        /// <returns>
        /// Возвращает <see langword="true"/> если идентификаторы команд равны. 
        /// В противном случае возвращает <see langword="false"/>.
        /// </returns>
        public bool Equals(InputCommand other)
        {
            if (other == null)
                return false;
            return other.commandId == commandId;
        }

        public static bool operator ==(InputCommand left, InputCommand right) => left.Equals(right);

        public static bool operator !=(InputCommand left, InputCommand right) => !left.Equals(right);

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
