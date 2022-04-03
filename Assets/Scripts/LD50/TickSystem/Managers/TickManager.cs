using Airashe.UCore.Common.Behaviours;
using Airashe.UCore.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.LD50.TickSystem.Managers
{
    public class TickManager : UnitySingleton<TickManager>, ISystemManager, Airashe.UCore.Common.Behaviours.IObservable<int>
    {
        private const float secondPerTick = 1;
        private int tick;
        private float tickTimer;
        private List<Airashe.UCore.Common.Behaviours.IObserver<int>> observers;

        private void Update()
        {
            tickTimer += Time.deltaTime;
            if (tickTimer >= secondPerTick)
            {
                tickTimer += secondPerTick;
                tick++;
                NotifySubscribers();
            }
        }

        public bool Initialized => SingletonInitialized;

        public void InitializeManager()
        {
            tick = 0;
            observers = new List<Airashe.UCore.Common.Behaviours.IObserver<int>>();
            return;
        }

        public void NotifySubscribers()
        {
            var tmpObservers = new List<Airashe.UCore.Common.Behaviours.IObserver<int>>(observers);
            foreach (var observer in tmpObservers)
                observer.OnObservableNotification(tick);
        }

        public void Subscribe(Airashe.UCore.Common.Behaviours.IObserver<int> observer)
        {
            observers.Add(observer);
        }

        public void Unsubscribe(Airashe.UCore.Common.Behaviours.IObserver<int> observer)
        {
            observers.Remove(observer);
        }
    }
}
