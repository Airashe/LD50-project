using Assets.Scripts.LD50.Core.Common;
using Assets.Scripts.LD50.EventSystem.InteractionBehaviours.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.LD50.DialogueSystem.Structs
{
    [Serializable]
    public class DialogueQuote
    {
        public static readonly DialogueQuote Empty = new DialogueQuote(string.Empty);

        [TextArea(5, 20)]
        public string data;
        public float quoteTime;
        public ScriptableInteractBehaviour scriptableEvent;
        public override string ToString()
        {
            return data;
        }
        public EvaluationString ToEvaluationString(int evaluationSpeed = int.MaxValue)
        {
            var es = new EvaluationString(data);
            es.EvaluateSpeed = evaluationSpeed;
            return es;
        }

        public DialogueQuote(string data = "", float quoteTime = 0.0f)
        {
            this.data = data;
            this.quoteTime = quoteTime;
        }
    }
}
