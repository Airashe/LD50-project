using Assets.Scripts.LD50.Core.Common;
using Assets.Scripts.LD50.DialogueSystem.Interfaces;
using Assets.Scripts.LD50.DialogueSystem.Structs;
using LD50.Controllers.Interfaces;
using LD50.Core;
using LD50.Core.Interact;
using LD50.DialogueSystem.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.LD50.Controllers
{
    public class IngameDialogueController : MonoBehaviour, IInitializable, IDialogueController
    {
        private bool isIntialized;
        public bool IsInitialized => isIntialized;

        public bool IsDialogueActive => isDialogueActive;
        private bool isDialogueActive;

        public DialogueContext DialogueContext => context;
        private DialogueContext context;

        public DialogueItem CurrentDialogueItem => currentDialueItem;

        private float lastNextQuouteRequest;
        public float LastNextQuoteRequest => lastNextQuouteRequest;

        public string CurrentLine => currentLine ?? string.Empty;
        private DialogueItem currentDialueItem;
        private EvaluationString currentLine;

        private void Update() => AutoProgressDialogue();

        private void AutoProgressDialogue()
        {
            if (isDialogueActive && currentDialueItem != null)
            {
                if (currentLine?.Evaluated != true)
                    lastNextQuouteRequest = Time.time;

                var timePassedSinceLastRequest = Time.time - lastNextQuouteRequest;
                if (timePassedSinceLastRequest >= currentDialueItem.Quote.quoteTime)
                    NextDialogueItem();
                if (currentDialueItem.Type == DialogueSystem.Enums.DialogueItemType.End)
                    isDialogueActive = false;
            }
        }

        public void StartDialogue(int dialogueId, DialogueContext dialogueContext)
        {
            if (isDialogueActive) EndDialogue();
            DialoguesManager.Instance.ActivateDialogue(dialogueId, dialogueContext);
            InitializeDialogue(dialogueContext);
        }
        public void StartDialogue(DialogueData dialogueData, DialogueContext dialogueContext)
        {
            if (isDialogueActive) EndDialogue();
            DialoguesManager.Instance.ActivateDialogue(dialogueData, dialogueContext);
            InitializeDialogue(dialogueContext);
        }

        private void InitializeDialogue(DialogueContext dialogueContext)
        {
            isDialogueActive = true;
            context = dialogueContext ?? DialogueContext.Empty;
            currentDialueItem = DialoguesManager.Instance.GetDialogueNextItem();
            currentLine = currentDialueItem?.Quote.ToEvaluationString(currentDialueItem?.EvalutaionSpeed ?? 0);
            currentLine.Evaluating = true;
            if (currentDialueItem.Type == DialogueSystem.Enums.DialogueItemType.End)
                EndDialogue();
        }

        public void NextDialogueItem()
        {
            lastNextQuouteRequest = Time.time;
            currentDialueItem = DialoguesManager.Instance.GetDialogueNextItem();
            currentLine = currentDialueItem?.Quote.ToEvaluationString(currentDialueItem?.EvalutaionSpeed ?? 0);
            currentLine.Evaluating = true;
        }

        public void EndDialogue()
        {
            context = DialogueContext.Empty;
            isDialogueActive = false;
        }

        public void Initialize()
        {
            isIntialized = true;
        }
    }
}
