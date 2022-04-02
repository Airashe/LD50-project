using Airashe.UCore.Common.Behaviours;
using Airashe.UCore.Systems.Input;
using LD50.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour, IObserver<InputCommandStateChanged>, IInitializable
{
    private void OnPlayerInput(InputCommandStateChanged inputData)
    {
        Debug.Log(inputData);
    }

    void IObserver<InputCommandStateChanged>.OnObservableNotification(InputCommandStateChanged notificationData) => OnPlayerInput(notificationData);

    void IInitializable.Initialize()
    {
        InputManager.Instance.Subscribe(this);
    }
}
