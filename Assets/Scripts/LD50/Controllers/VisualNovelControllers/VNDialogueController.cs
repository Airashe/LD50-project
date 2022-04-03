using Assets.Scripts.LD50.DialogueSystem.Structs;
using LD50.Controllers.Interfaces;
using LD50.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.LD50.Controllers.VisualNovelControllers
{
    internal class VNDialogueController : MonoBehaviour, IInitializable, IDialogueController
    {
        public bool IsDialogueActive => throw new NotImplementedException();

        public DialogueItem CurrentDialogueItem => throw new NotImplementedException();

        public DialogueContext DialogueContext => throw new NotImplementedException();

        public float LastNextQuoteRequest => throw new NotImplementedException();

        public bool IsInitialized => isInitialized;

        public string CurrentLine => throw new NotImplementedException();

        private bool isInitialized;

        public void EndDialogue()
        {
            throw new NotImplementedException();
        }

        public void Initialize() => isInitialized = true;

        public void NextDialogueItem()
        {
            throw new NotImplementedException();
        }

        public void StartDialogue(int dialogueId, DialogueContext dialogueContext)
        {
            throw new NotImplementedException();
        }

        public void StartDialogue(DialogueData dialogueData, DialogueContext dialogueContext)
        {
            throw new NotImplementedException();
        }
    }
}
