using Airashe.UCore.Common.Behaviours;
using Airashe.UCore.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.LD50.EventSystem.Manager
{
    public sealed class EventManager : UnitySingleton<EventManager>, ISystemManager
    {
        public bool Initialized => SingletonInitialized;
        
        public void InitializeManager()
        {
            return;
        }
    }
}
