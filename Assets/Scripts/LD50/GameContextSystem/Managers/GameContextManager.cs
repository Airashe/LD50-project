using Airashe.UCore.Common.Behaviours;
using Airashe.UCore.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.LD50.GameContextSystem.Managers
{
    public class GameContextManager : UnitySingleton<GameContextManager>, ISystemManager
    {
        public bool Initialized => SingletonInitialized && initialized;
        private bool initialized;

        private Dictionary<string, int> intContext;
        private Dictionary<string, object> classesContext;

        public void Put(string key, int value)
        {
            if (!this.intContext.ContainsKey(key))
                intContext.Add(key, value);
            intContext[key] = value;
        }
        public void Put<T>(string key, T value) where T : class
        {
            if (!this.classesContext.ContainsKey(key))
                classesContext.Add(key, value);
            classesContext[key] = value;
        }

        public void InitializeManager()
        {
            classesContext = new Dictionary<string, object>();
            intContext = new Dictionary<string, int>();
            initialized = true;
        }
    }
}
