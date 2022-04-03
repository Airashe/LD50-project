using Assets.Scripts.LD50.DialogueSystem.Interfaces;
using Assets.Scripts.LD50.DialogueSystem.Structs;

namespace LD50.Controllers.Interfaces
{
    public interface IDialogueController
    {
        public string CurrentLine { get; }
        public bool IsDialogueActive { get; }
        public DialogueItem CurrentDialogueItem { get; }
        public DialogueContext DialogueContext { get; }

        public void StartDialogue(int dialogueId, DialogueContext dialogueContext);
        public void StartDialogue(DialogueData dialogueData, DialogueContext dialogueContext);
        public void EndDialogue();

        public void NextDialogueItem();

        public float LastNextQuoteRequest { get; }
    }
}