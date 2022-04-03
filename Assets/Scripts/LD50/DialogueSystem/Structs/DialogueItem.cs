using Assets.Scripts.LD50.DialogueSystem.Enums;
using Assets.Scripts.LD50.DialogueSystem.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.LD50.DialogueSystem.Structs
{
    [Serializable]
    public class DialogueItem
    {
        public static readonly DialogueItem EndItem = new DialogueItem(DialogueItemType.End);

#if UNITY_EDITOR
        [SerializeField]
        private string name;
#endif

        [SerializeField]
        private int evaluationSpeed;
        public int EvalutaionSpeed => evaluationSpeed;

        [SerializeField]
        private DialogueItemType type;
        public DialogueItemType Type => type;

        [SerializeField]
        private int interactorId;
        public int InteractorId => interactorId;

        public IDialogueParticiant Interactor
        {
            get
            {
                if (dialogueData?.dialogueContext == null || dialogueData.dialogueContext.DialogueParticiants.Count() < interactorId)
                    return null;
                return dialogueData.dialogueContext.DialogueParticiants[interactorId];
            }
        }
        public DialogueData DialogueData
        {
            get => dialogueData;
            set => dialogueData = value;
        }
        private DialogueData dialogueData;

        [SerializeField]
        private DialogueQuote[] quotes;
        public DialogueQuote Quote
        {
            get
            {
                if (quotes != null && quotes.Length > 0)
                    return quotes[0];
                return DialogueQuote.Empty;
            }
        }

        public DialogueQuote[] Answers
        {
            get
            {
                if (quotes != null && quotes.Length > 0)
                    return quotes;
                return new DialogueQuote[0];
            }
        }

        public DialogueItem()
        {

        }

        public DialogueItem(DialogueItemType type)
        {
            this.type = type;
        }
    }
}
