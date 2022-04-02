using Airashe.UCore.Common.Behaviours;
using Airashe.UCore.Systems.Input;
using LD50.Input.Commands;
using LD50.Input.Structs;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD50.Core.Controllers
{
    [Serializable]
    public class IngameController : MonoBehaviour, Airashe.UCore.Common.Behaviours.IObserver<InputCommandStateChanged>, IInitializable
    {
        public Unit ControlledUnit
        {
            get => controlledUnit;
            private set => controlledUnit = value;
        }

        [SerializeField]
        private Unit controlledUnit;

        private void Update()
        {
            if (ControlledUnit == null)
                return;

            FollowControlledUnit();
        }

        private void FixedUpdate()
        {
            if (ControlledUnit == null)
                return;

            MoveConrolledUnit();
        }

        private void FollowControlledUnit()
        {
            this.transform.position = controlledUnit.transform.position;
        }

        private void MoveConrolledUnit()
        {
            ControlledUnit.Rigidbody.velocity = ControlledUnit.MovementDirection * ControlledUnit.WalkSpeed;
        }

        private void OnPlayerInput(InputCommandStateData inputData)
        {
            if (ControlledUnit == null) return;
            if ((inputData.CommandType & InputCommandType.MovementMask) == inputData.CommandType)
                MoveControlledUnitUnit(inputData.CommandType, inputData.IsActive);
        }

        private void MoveControlledUnitUnit(InputCommandType inputCommand, bool isActive)
        {
            var moveCommands = new List<int>() {
                (int)InputCommandType.MoveForward,
                (int)InputCommandType.MoveBackward,
                (int)InputCommandType.MoveLeft,
                (int)InputCommandType.MoveRight, 
            };
            if (isActive)
                moveCommands.Remove((int)inputCommand);

            var activeCommand = isActive ? inputCommand : InputCommandType.None;
            if (activeCommand == InputCommandType.None)
            {
                activeCommand = (InputCommandType)InputManager.Instance.IsAnyActive(moveCommands);
                isActive = activeCommand != InputCommandType.None;
            }

            var movementDirection = Vector3.zero;
            switch (activeCommand)
            {
                case InputCommandType.MoveForward:
                    movementDirection.y = isActive ? 1 : 0;
                    break;
                case InputCommandType.MoveBackward:
                    movementDirection.y = isActive ? -1 : 0;
                    break;
                case InputCommandType.MoveLeft:
                    movementDirection.x = isActive ? -1 : 0;
                    break;
                case InputCommandType.MoveRight:
                    movementDirection.x = isActive ? 1 : 0;
                    break;
            }

            ControlledUnit.MovementDirection = movementDirection;
        }

        void Airashe.UCore.Common.Behaviours.IObserver<InputCommandStateChanged>.OnObservableNotification(InputCommandStateChanged notificationData) => OnPlayerInput(notificationData);

        bool IInitializable.IsInitialized => isInitialized;
        private bool isInitialized;
        void IInitializable.Initialize()
        {
            InputManager.Instance.Subscribe(this);
            isInitialized = true;
        }
    }

}