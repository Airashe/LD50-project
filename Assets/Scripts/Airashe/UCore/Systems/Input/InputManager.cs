using System;
using System.Collections.Generic;
using System.Threading;
using Airashe.UCore.Common.Behaviours;
using Airashe.UCore.Systems.Input.Commands;
using UnityEngine;
using UnityInput = UnityEngine.Input;

namespace Airashe.UCore.Systems.Input
{
    /// <summary>
    /// Менеджер считывающий ввод игрока и преобразующий его в команды.
    /// </summary>
    [Serializable]
    public class InputManager : UnitySingleton<InputManager>, 
                                ISystemManager, 
                                Common.Behaviours.IObservable<InputCommandStateChanged>
    {
        public bool Initialized
        {
            get => initialized;
            private set => initialized = value;
        }
        private bool initialized;
        /// <summary>
        /// Текущий глобальный индекс команды.
        /// </summary>
        public int CommandGlobalIndex => commands.Count;
        /// <summary>
        /// Текущие команды, доступные пользователю.
        /// </summary>
        public List<InputCommand> commands;
        /// <summary>
        /// Событие происходящее когда пользователь изменил статус команды ввода.
        /// </summary>
        private event Action<InputCommandStateChanged> commandStateChanged;

        private void Update()
        {
            if (!Initialized) return;

            ReadUserInput();
        }

        public void InitializeManager()
        {
            GetAllInputCommands();
            Initialized = true;
        }

        /// <summary>
        /// Подписать <paramref name="observer"/> на уведомления менеджера ввода.
        /// </summary>
        /// <param name="observer">Слушатель ввода.</param>
        public void Subscribe(Common.Behaviours.IObserver<InputCommandStateChanged> observer)
        {
            if (observer == null) return;

            commandStateChanged += observer.OnObservableNotification;
        }

        /// <summary>
        /// Отписать <paramref name="observer"/> от уведомлений менеджера ввода.
        /// </summary>
        /// <param name="observer">Слушатель ввода.</param>
        public void Unsubscribe(Common.Behaviours.IObserver<InputCommandStateChanged> observer)
        {
            if (observer == null) return;

            commandStateChanged -= observer.OnObservableNotification;
        }

        /// <summary>
        /// Получить следующий глобальный индекс команды.
        /// <para>Так же увеличивает значение глобального индекса.</para>
        /// </summary>
        /// <returns>
        /// Возвращает следующий уникальный идентификатор команды.
        /// </returns>
        protected int NextCommandGlobalIndex() => CommandGlobalIndex + 1;

        /// <summary>
        /// Получить все объекты команд, которые есть в текущей сборке.
        /// </summary>
        private void GetAllInputCommands()
        {
            commands = new List<InputCommand>();
            commands.Add(new NoneCommand(NextCommandGlobalIndex()));

            var assemblyTypes = this.GetType().Assembly.GetTypes();
            foreach(var type in assemblyTypes)
            {
                if (type.BaseType == typeof(InputCommand))
                {
                    int nextIndex = NextCommandGlobalIndex();
                    try
                    {
                        var command = (InputCommand) Activator.CreateInstance(type, nextIndex);
                        commands.Add(command);
                    }
                    catch(Exception ex)
                    {
                        Debug.LogError($"InputManager: Command intialization error, {ex.Message}");
                    }
                }
            }
        }

        /// <summary>
        /// Считать пользовательский ввод.
        /// </summary>
        private void ReadUserInput()
        {
            foreach(var command in commands)
            {
                ReadKeyCodeInput(command);
            }
        }

        /// <summary>
        /// Считать команду при помощи свойства <see cref="InputCommand.Key"/>.
        /// </summary>
        /// <param name="command">Команда.</param>
        private void ReadKeyCodeInput(InputCommand command)
        {
            if (command.Key == KeyCode.None) return;

            if (UnityInput.GetKeyDown(command.Key))
            {
                ChangeCommandState(command, true);
                return;
            }
            if (UnityInput.GetKeyUp(command.Key))
            {
                ChangeCommandState(command, false);
                return;
            }
        }
        
        /// <summary>
        /// Изменить состояние команды ввода.
        /// </summary>
        /// <param name="commandId">Идентификатор команды.</param>
        /// <param name="isActive">Новый статус команды.</param>
        private void ChangeCommandState(InputCommand command, bool isActive)
        {
            command.IsActive = isActive;
            OnCommandStateChanged(command);
        }

        /// <summary>
        /// Уведомить подписчиков о изменении статуса команды.
        /// </summary>
        /// <param name="commandId">Идентификатор команды.</param>
        private void OnCommandStateChanged(InputCommand command)
        {
            var notificationData = new InputCommandStateChanged(command.TypeIndex, command.IsActive);

            var observersSnapshot = commandStateChanged;
            if (observersSnapshot != null) observersSnapshot(notificationData);
        }
    }
}
