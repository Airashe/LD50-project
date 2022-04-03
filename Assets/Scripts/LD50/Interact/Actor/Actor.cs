using Assets.Scripts.LD50.DialogueSystem.Interfaces;
using Assets.Scripts.LD50.DialogueSystem.Structs;
using Assets.Scripts.LD50.Interact.Items;
using LD50.Controllers.Interfaces;
using UnityEngine;
using LD50Application = LD50.Core.Application;

namespace LD50.Core.Interact
{
    public sealed class Actor : MonoBehaviour, IInteractable<Item>, IDialogueParticiant
    {
        public DialogueContext dialogueContext;
        public DialogueData dialogue;

        private static Actor emptyActor;
        public static Actor Empty()
        {
            if (emptyActor != null) return emptyActor;

            var go = new GameObject();
            emptyActor = go.AddComponent<Actor>();
            return emptyActor;
        }
        public GameObject GameObject => this.gameObject;
        [Header("Interactable")]
        [SerializeField]
        private float interationRadius = 1.0f;
        public float InteractionRadius => interationRadius;

        [SerializeField]
        private Vector2 interactionPivot;
        public Vector2 InteractionPivot => interactionPivot;

        [Header("Dialogues")]
        [SerializeField]
        private Vector2 dialogueBoxPivot;
        public Vector3 ParticiantWorldPosition
        {
            get
            {
                var selfPosition = gameObject.transform.position;
                selfPosition.x += dialogueBoxPivot.x;
                selfPosition.y += dialogueBoxPivot.y;
                return selfPosition;
            }
        }
        public InteractionResult BeginInteractionWith(Item interactionObject)
        {
            Debug.Log($"Interacted with item: {interactionObject}");
            return InteractionResult.Success;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(gameObject.transform.position + new Vector3(interactionPivot.x, interactionPivot.y, 0), InteractionRadius);
        }

        public void InteractionBeginInvoke()
        {
            var dialogueController =LD50Application.Instance.PlayerGO.GetComponent<IDialogueController>();
            if (dialogueController?.IsDialogueActive == false)
                dialogueController?.StartDialogue(dialogue, dialogueContext);
        }

        public void InteractionEndInvoke()
        {
            return;
        }
    }
}
