using Assets.Scripts.LD50.Interact.Items;
using LD50.Controllers;
using LD50.Core.Interact;
using LD50.Interact.Items;
using System;
using UnityEngine;
using LD50Application = LD50.Core.Application;

namespace Assets.Scripts.LD50.Interact
{
    public class OnGroundItem : MonoBehaviour, IInteractable
    {
        [SerializeField]
        private ItemData itemData;

        public GameObject GameObject => this.gameObject;

        [SerializeField]
        private float interactionRadius;
        public float InteractionRadius => interactionRadius;

        [SerializeField]
        private Vector2 interactionPivot;
        public Vector2 InteractionPivot => interactionPivot;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(gameObject.transform.position + new Vector3(interactionPivot.x, interactionPivot.y, 0), InteractionRadius);
        }

        public void InteractionEndInvoke()
        {
            
        }

        public void InteractionBeginInvoke()
        {
            var uiController = LD50Application.Instance.PlayerGO.GetComponent<IUIController>();
            if (uiController == null) return;

            uiController.DragDropBeginInvoke(this);
        }

        public static implicit operator Item (OnGroundItem groundItem)
        {
            return new Item(groundItem.itemData, groundItem);
        }
    }
}
