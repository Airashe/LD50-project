using Assets.Scripts.LD50.DialogueSystem.Interfaces;
using Assets.Scripts.LD50.DialogueSystem.Structs;
using Assets.Scripts.LD50.EventSystem.Structs;
using Assets.Scripts.LD50.Interact.Items;
using LD50.Controllers.Interfaces;
using UnityEngine;
using LD50Application = LD50.Core.Application;

namespace LD50.Core.Interact
{
    public sealed class Actor : MonoBehaviour, IInteractable<Item>, IDialogueParticiant
    {
        public ItemInteractBehaviour ItemInteractBehaviour
        {
            get
            {
                if (itemInteractBehaviour == null)
                    itemInteractBehaviour = GetComponent<ItemInteractBehaviour>();
                return itemInteractBehaviour;
            }
        }
        public InteractBehaviour InteractBehaviour
        {
            get
            {
                if (interactBehaviour == null)
                    interactBehaviour = GetComponent<InteractBehaviour>();
                return interactBehaviour;
            }
        }
        private ItemInteractBehaviour itemInteractBehaviour;
        [SerializeField]
        private InteractBehaviour interactBehaviour;

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
            if (ItemInteractBehaviour == null || interactionObject == null)
                return InteractionResult.NoInteractableItem;

            return ItemInteractBehaviour.InteractionWith(gameObject, interactionObject);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(gameObject.transform.position + new Vector3(interactionPivot.x, interactionPivot.y, 0), InteractionRadius);
        }

        public void InteractionBeginInvoke()
        {
            if (InteractBehaviour == null) return;
            InteractBehaviour.InteractionBegin(gameObject);
        }

        public void InteractionEndInvoke()
        {
            if (InteractBehaviour == null) return;
            InteractBehaviour.InteractionEnd(gameObject);
        }
    }
}
