using UnityEngine;

namespace LD50.Core.Interact
{
    public interface IInteractable
    {
        public GameObject GameObject { get; }
        public float InteractionRadius { get; }
        public Vector2 InteractionPivot { get; }

        public void InteractionBeginInvoke();
        public void InteractionEndInvoke();
    }

    public interface IInteractable<TAwaitingType> : IInteractable
    {
        public InteractionResult BeginInteractionWith(TAwaitingType interactionObject);
    }
}
