using UnityEngine;

namespace LD50.Core.Extensions
{
    public static class GameObjectExtensions
    {
        public static T AddComponentIfNotExists<T>(this GameObject gameObject) where T : Component
        {
            if (gameObject == null)
                return null;

            var component = gameObject.GetComponent<T>();
            if (component == null)
            {
                component = gameObject.AddComponent<T>();
            }
            return component;
        }

        public static T IntializeComponent<T>(this GameObject gameObject) where T : Component, IInitializable
        {
            if (gameObject == null)
                return null;

            var component = gameObject.GetComponent<T>();
            if (component == null)
            {
                component = gameObject.AddComponent<T>();
            }

            if (!(component).IsInitialized)
            {
                (component).Initialize();
            }
            return component;
        }
    }
}
