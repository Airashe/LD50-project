using Airashe.UCore.Common.Behaviours;
using Assets.Scripts.LD50.DataBaseSystem.Manager;
using Assets.Scripts.LD50.DialogueSystem.Structs;
using Assets.Scripts.LD50.EventSystem.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LD50.DialogueSystem.Managers
{
    public sealed class DialoguesManager : UnitySingleton<DialoguesManager>, Airashe.UCore.Systems.ISystemManager
    {
        public bool Initialized => intialized && SingletonInitialized;
        private bool intialized;

        [SerializeField]
        private DialogueData activeDialogue;
        [SerializeField]
        private int currentItemIndex;
        public DialogueItem CurrentItem => activeDialogue?.items[currentItemIndex] ?? DialogueItem.EndItem;
        public DialogueItem LastQuoteItem => lastQuoteItem ?? DialogueItem.EndItem;
        private DialogueItem lastQuoteItem;

        private DataBaseManager dataBaseManager => DataBaseManager.Instance;

        public void InitializeManager()
        {
            intialized = true;
        }

        public void ActivateDialogue(int dialogueId, DialogueContext context)
        {
            ActivateDialogue(dataBaseManager.GetDialogueDataById(dialogueId), context);
        }

        public void ActivateDialogue(DialogueData dialogue, DialogueContext context)
        {
            if (dialogue == null)
                activeDialogue = new DialogueData();

            activeDialogue = dataBaseManager.GetDialogueData(dialogue);
            activeDialogue.dialogueContext = context;
            foreach (var item in activeDialogue.items)
                item.DialogueData = activeDialogue;
            currentItemIndex = -1;
        }

        public DialogueItem ChoseAnswer(GameObject source, int answerIndex)
        {
            if (activeDialogue == null) return DialogueItem.EndItem;
            if (currentItemIndex >= activeDialogue.Length) return DialogueItem.EndItem;
            var currentItem = activeDialogue[currentItemIndex];
            if (answerIndex >= currentItem.Answers.Length) return GetDialogueNextItem();
            return ChoseAnswer(source, currentItem.Answers[answerIndex]);
        }

        public DialogueItem ChoseAnswer(GameObject source, DialogueQuote answer)
        {
            if (activeDialogue == null) return DialogueItem.EndItem;
            if (currentItemIndex >= activeDialogue.Length) return GetDialogueNextItem();

            if (activeDialogue.items[currentItemIndex].Answers.Contains(answer))
                StartAnswerEvent(source, answer);
            if (answer.nextDialogue != null)
            {
                ActivateDialogue(answer.nextDialogue, activeDialogue.dialogueContext);
            }
            return GetDialogueNextItem();
        }
        private void StartAnswerEvent(GameObject source, DialogueQuote answer)
        {
            if (answer == null) return;
            if (currentItemIndex >= activeDialogue.Length) return;
            var currentItemEvent = answer.scriptableEvent;

            EventManager.Instance.StartEvent(source, currentItemEvent);
        }

        public DialogueItem GetDialogueNextItem()
        {
            currentItemIndex++;
            if (currentItemIndex >= activeDialogue.Length) return DialogueItem.EndItem;

            StartCurrentItemEvent();
            var item = activeDialogue[currentItemIndex];
            if (item.Type == Assets.Scripts.LD50.DialogueSystem.Enums.DialogueItemType.Quote)
                lastQuoteItem = item;

            if (item.Type != Assets.Scripts.LD50.DialogueSystem.Enums.DialogueItemType.Answer && item.Quote.nextDialogue != null)
            {
                ActivateDialogue(item.Quote.nextDialogue, activeDialogue.dialogueContext);
                return GetDialogueNextItem();
            }
                

            return item;
        }

        public void StartCurrentItemEvent(GameObject source = null)
        {
            if (currentItemIndex >= activeDialogue.Length) return;

            StartAnswerEvent(source, activeDialogue[currentItemIndex].Quote);
        }
    }
}
