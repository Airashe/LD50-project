using Airashe.UCore.Systems;
using UnityEngine;

namespace LD50.Core.Managers
{
    public sealed class ManagerFactory
    {
        public static T InitializeManager<T>() where T : MonoBehaviour, ISystemManager
        {
            var managerGO = Application.Instance.gameObject;
            managerGO.transform.position = Vector3.zero;
            managerGO.AddComponent<T>();
            
            var manager = managerGO.GetComponent<T>();
            manager.InitializeManager();
            return manager;
        }
    }
}
