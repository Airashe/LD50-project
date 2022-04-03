using Airashe.UCore.Common.Behaviours;
using Assets.Scripts.LD50.TickSystem.Managers;
using System;
using UnityEngine;

namespace Assets.Scripts.LD50.Core.Common
{
    [Serializable]
    public class EvaluationString : Airashe.UCore.Common.Behaviours.IObserver<int>
    {
        public static readonly EvaluationString Empty = new EvaluationString(null);
        [SerializeField]
        private string source;
        public int CurrentLength
        {
            get => currentLength;
            set
            {
                if (currentLength >= source.Length)
                {
                    currentLength = source.Length;
                    evaluated = true;
                    TickManager.Instance?.Unsubscribe(this);
                    return;
                }
                if (currentLength < 0)
                {
                    currentLength = 0;
                    TickManager.Instance?.Subscribe(this);
                    return;
                }
                currentLength = value;
            }
        }
        [SerializeField]
        private int currentLength;
        public int EvaluateSpeed { get; set; }
        [SerializeField]
        private int lastEvaluationTick;
        public bool Evaluating
        {
            get => evaluating;
            set
            {
                if (value)
                    TickManager.Instance?.Subscribe(this);
                else
                    TickManager.Instance?.Unsubscribe(this);
                evaluating = value;
            }
        }
        [SerializeField]
        private bool evaluating;

        public bool Evaluated
        {
            get => evaluated;
            set
            {
                if (value)
                    CurrentLength = source.Length;
                evaluated = value;
            }
        }
        private bool evaluated;

        public override string ToString() => source.Substring(0, CurrentLength);

        public void OnObservableNotification(int notificationData)
        {
            if (!Evaluating) return;

            var ticksEvaluated = notificationData - lastEvaluationTick;
            if (ticksEvaluated >= EvaluateSpeed)
            {
                lastEvaluationTick = notificationData;
                CurrentLength++;
            }
                
        }

        public EvaluationString(string source = null)
        {
            this.EvaluateSpeed = int.MaxValue;
            this.lastEvaluationTick = 0;
            this.source = source ?? string.Empty;
            this.currentLength = 0;
            this.evaluating = false;
        }

        public static implicit operator string (EvaluationString bcs)
        {
            if (bcs == null) return string.Empty;
            return bcs.ToString();
        }

        public static implicit operator EvaluationString(string s)
        {
            return new EvaluationString(s);
        }
    }
}
