using Airashe.UCore.Systems;
using UnityEngine;

namespace LD50.Core.Managers
{
    public sealed class ManagerFactory
    {
        public static T InitializeManager<T>() where T : MonoBehaviour, ISystemManager
        {
            var managerGO = GameApplication.Instance.gameObject;
            managerGO.transform.position = Vector3.zero;
            
            var manager = managerGO.GetComponent<T>();
            if (manager == null)
                manager = managerGO.AddComponent<T>();

            manager.InitializeManager();
            return manager;
        }
    }
}
