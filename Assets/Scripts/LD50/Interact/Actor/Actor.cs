using Assets.Scripts.LD50.Interact.Items;
using UnityEngine;

namespace LD50.Core.Interact
{
    public sealed class Actor : MonoBehaviour, IInteractable<Item>
    {
        public GameObject GameObject => this.gameObject;
        [Header("Interactable")]
        [SerializeField]
        private float interationRadius = 1.0f;
        public float InteractionRadius => interationRadius;

        [SerializeField]
        private Vector2 interactionPivot;
        public Vector2 InteractionPivot => interactionPivot;

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
            throw new System.NotImplementedException();
        }

        public void InteractionEndInvoke()
        {
            throw new System.NotImplementedException();
        }
    }
}
