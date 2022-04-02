using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Airashe.UCore.Common.Behaviours
{
    /// <summary>
    /// Интерфейс, предоставляющий функции наблюдателя за типом реализующим <see cref="IObservable{T}"/>.
    /// </summary>
    /// <typeparam name="T">Тип с данными уведомления.</typeparam>
    public interface IObserver<T>
    {
        /// <summary>
        /// Метод, вызываемый <see cref="IObservable{T}"/>, когда происходит уведомление.
        /// </summary>
        /// <param name="notificationData">Данные уведомления.</param>
        public void OnObservableNotification(T notificationData);
    }
}