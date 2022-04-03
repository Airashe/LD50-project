using Airashe.UCore.Common.Behaviours;
using Airashe.UCore.Systems;
using Assets.Scripts.LD50.EventSystem.InteractionBehaviours.Common;
using Assets.Scripts.LD50.EventSystem.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.LD50.EventSystem.Manager
{
    public sealed class EventManager : UnitySingleton<EventManager>, ISystemManager
    {
        public bool Initialized => SingletonInitialized;

        public void StartEvent(GameObject source, InteractBehaviour @event)
        {
            if (@event == null) return;
            @event.InteractionBegin(source);
        }

        public void EndEvent(GameObject source, InteractBehaviour @event)
        {
            if (@event == null) return;
            @event.InteractionEnd(source);
        }

        public void InitializeManager()
        {
            return;
        }
    }
}
