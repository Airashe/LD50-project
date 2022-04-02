using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Airashe.UCore.Common.Behaviours
{
    /// <summary>
    /// Добавляет к объектам <see cref="MonoBehaviour"/> поведение синглтона. 
    /// Позволяя единовременно находится только одному экземпляру типа <typeparamref name="T"/> 
    /// в игре.
    /// </summary>
    /// <typeparam name="T">Тип, для которого добавляется поведение синглтона.</typeparam>
    public abstract class UnitySingleton<T> : MonoBehaviour where T : UnitySingleton<T>
    {
        /// <summary>
        /// Экзмепляр <typeparamref name="T"/>.
        /// </summary>
        public static T Instance => instance;

        [Header("Singleton")]
        private static T instance;

        protected bool SingletonInitialized => singletonInitialized;
        private bool singletonInitialized;

        private void Start()
        {
            if (instance != null && instance != this)
                Destroy(gameObject);

            DontDestroyOnLoad(this);
            instance = (T)this;

            singletonInitialized = true;
            SingletonIntialized();
        }

        /// <summary>
        /// Метод вызываемый в <see cref="Start"/>, после того как синглтон был проинициализирован.
        /// </summary>
        protected virtual void SingletonIntialized() { }
    }

}