using Assets.Scripts.LD50.Interact.Items;
using LD50.Controllers;
using LD50.Core.Interact;
using LD50.Interact.Items;
using System;
using UnityEditor;
using UnityEngine;
using LD50Application = LD50.Core.GameApplication;

namespace Assets.Scripts.LD50.Interact
{
    public class OnGroundItem : MonoBehaviour, IInteractable
    {
        [SerializeField]
        private ItemData itemData;

        public GameObject GameObject => this.gameObject;

        [Header("Interaction")]
        [SerializeField]
        private float interactionRadius;
        public float InteractionRadius => interactionRadius;

        [SerializeField]
        private Vector2 interactionPivot;
        public Vector2 InteractionPivot => interactionPivot;

        public bool Destroyed => (gameObject?.activeSelf ?? true) != true;
        private bool IsItemVisible
        {
            get => this?.GetComponent<SpriteRenderer>()?.enabled ?? false;
            set
            {
                if (this?.gameObject?.GetComponent<SpriteRenderer>() != null) this.GetComponent<SpriteRenderer>().enabled = value;
            }
        }

        private void Start()
        {
            var render = this.gameObject.GetComponent<SpriteRenderer>();
            if (render != null && itemData != null)
                render.sprite = itemData.Sprite;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(gameObject.transform.position + new Vector3(interactionPivot.x, interactionPivot.y, 0), InteractionRadius);
        }

        public void InteractionEndInvoke()
        {
            IsItemVisible = true;
        }

        public void InteractionBeginInvoke()
        {
            var uiController = LD50Application.Instance.PlayerGO.GetComponent<IUIController>();
            if (uiController == null) return;

            uiController.DragDropBeginInvoke(this);
            IsItemVisible = false;
        }

        public static implicit operator Item (OnGroundItem groundItem)
        {
            return new Item(groundItem.itemData, groundItem);
        }
    }
}
