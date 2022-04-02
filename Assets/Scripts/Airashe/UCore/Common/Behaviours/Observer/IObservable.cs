using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Airashe.UCore.Common.Behaviours
{
    /// <summary>
    /// Интерфейс, предоставляющий функции наблюдаемого объекта в паттерне "Наблюдатель".
    /// </summary>
    /// <typeparam name="T">Тип с данными уведомления.</typeparam>
    public interface IObservable<T>
    {
        /// <summary>
        /// Подписать <paramref name="observer"/> на уведомления.
        /// </summary>
        /// <param name="observer">Экземпляр, подписывающийся на уведомления.</param>
        public void Subscribe(IObserver<T> observer);

        /// <summary>
        /// Отписать <paramref name="observer"/> от уведомлений.
        /// </summary>
        /// <param name="observer">Экземпляр, подписывающийся на уведомления.</param>
        public void Unsubscribe(IObserver<T> observer);
    }
}