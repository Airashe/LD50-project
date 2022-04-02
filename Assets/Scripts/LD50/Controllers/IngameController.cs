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

        private void OnPlayerInput(InputCommandStateData inputData)
        {
            if (ControlledUnit == null) return;
            if ((inputData.CommandType & InputCommandType.MovementMask) == inputData.CommandType && inputData.IsActive)
                MoveControlledUnitUnit(inputData.CommandType);
        }

        public void MoveControlledUnitUnit(InputCommandType inputCommand)
        {
            Debug.Log(inputCommand);
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