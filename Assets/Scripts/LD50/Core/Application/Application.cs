using Airashe.UCore.Common.Behaviours;
using Airashe.UCore.Systems;
using Airashe.UCore.Systems.Input;
using LD50.Core.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LD50.Core
{
    public sealed class Application : UnitySingleton<Application>
    {
        public ISystemManager InputManager => inputManager;
        private ISystemManager inputManager;

        private void Start() => InitializeApplication();

        private void InitializeApplication()
        {
            IntializeManagers();
        }

        private void IntializeManagers()
        {
            inputManager = ManagerFactory.InitializeManager<InputManager>();
        }
    }
}
