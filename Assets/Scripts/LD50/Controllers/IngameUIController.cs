using LD50.Core;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LD50.Controllers;
using LD50.Interact.Items;
using LD50.Controllers.Interfaces;
using Assets.Scripts.LD50.Core.Structs;
using UnityInput = UnityEngine.Input;
using Airashe.UCore.Systems.Input;
using LD50.Input.Structs;
using LD50.Input.Commands;
using Assets.Scripts.LD50.Interact.Items;
using LD50Application = LD50.Core.Application;
using LD50.Core.Interact;

namespace Assets.Scripts.LD50.Controllers
{
    public sealed class IngameUIController : MonoBehaviour, IInitializable, IUIController, Airashe.UCore.Common.Behaviours.IObserver<InputCommandStateChanged>
    {
        public ILogicController LogicController => logicController;
        private ILogicController logicController;

        [SerializeField]
        private Vector2 dialogueCloudSizes = new Vector2(180, 40);
        private Item dragDropItem;
        private bool dragDropHaveItem;
        [SerializeField]
        private DynamicRect inventoryAreaRect = new DynamicRect(50, 92.5f, 73.68f, 15);
        [SerializeField]
        private int ItemsInInventoryCount => logicController?.ControlledUnit?.Inventory?.Count ?? 0;
        private int previousRenderedItemsCount;
        private Rect[] inventoryItemsRects;

        private bool inventoryIsActive = true;
        private bool interactionActive = false;

        private Camera mainCamera;

        private void OnPlayerInput(InputCommandStateData inputData)
        {
            if (inputData.CommandType == InputCommandType.None) return;

            if (inputData.CommandType == InputCommandType.Interact)
            {
                ProcessInteraction(inputData.IsActive);
                return;
            }
        }

        #region Render
        private void OnGUI()
        {
            RenderInventory();
            RenderDialogue();
            RenderDragDropItem();
        }

        private void RenderDragDropItem()
        {
            if (dragDropItem?.ItemData?.Texture == null) return;

            var texture = dragDropItem.ItemData.Texture;
            var rect = new Rect(UnityInput.mousePosition.x, Screen.height - UnityInput.mousePosition.y, LD50Application.Instance.applicationConfiguration.dragDropIconSize.x, LD50Application.Instance.applicationConfiguration.dragDropIconSize.x);
            GUI.DrawTexture(rect, texture);
        }

        private void RenderInventory()
        {
            if (inventoryIsActive)
            {
                GUI.Box(inventoryAreaRect, "");

                if (previousRenderedItemsCount != ItemsInInventoryCount)
                {
                    inventoryItemsRects = new Rect[ItemsInInventoryCount];
                }

                var rectStartPosition = new Vector2(inventoryAreaRect.LeftCornerAbsouleX + 5, inventoryAreaRect.LeftCornerAbsouleY + 5);
                var unityInventory = logicController?.ControlledUnit?.Inventory ?? new List<Item>();
                for (var i = 0; i < ItemsInInventoryCount; i++)
                {
                    var itemIcon = unityInventory[i]?.ItemData?.Texture;
                    var itemButtonSize = new Vector2(40, 40);
                    inventoryItemsRects[i] = new Rect(rectStartPosition, itemButtonSize);
                    inventoryItemsRects[i].x += (itemButtonSize.x + 10) * i;
                    GUI.Button(inventoryItemsRects[i], "");

                    var iconPosition = inventoryItemsRects[i].position;
                    iconPosition.x += 2.5f;
                    iconPosition.y += 2.5f;
                    GUI.DrawTexture(new Rect(iconPosition, new Vector2(35, 35)), itemIcon);
                }
            }
        }
        #endregion

        #region DialogueIngame

        private void RenderDialogue()
        {
            if (logicController?.DialogueController?.IsDialogueActive != true)
                return;

            RenderDialogueItem();
        }

        private void RenderDialogueItem()
        {
            var currentItem = logicController?.DialogueController?.CurrentDialogueItem;
            var currentContext = logicController?.DialogueController?.DialogueContext;
            if (currentItem == null)
                return;

            var participantIndex = currentItem.InteractorId;
            var quoteParent = participantIndex >= currentContext.DialogueParticiants.Length ? Actor.Empty() : currentContext.DialogueParticiants[participantIndex] ?? Actor.Empty();

            Vector3 qouteBorderPosition = mainCamera.WorldToScreenPoint(quoteParent.ParticiantWorldPosition);

            var borderRect = new Rect(qouteBorderPosition.x - dialogueCloudSizes.x / 2, Screen.height - qouteBorderPosition.y - dialogueCloudSizes.y / 2, dialogueCloudSizes.x, dialogueCloudSizes.y);

            if (borderRect.x < 0) borderRect.x = 0;
            if (borderRect.y < 0) borderRect.y = 0;
            if (borderRect.x > Screen.width - dialogueCloudSizes.x) borderRect.x = Screen.width - dialogueCloudSizes.x;
            if (borderRect.y > Screen.height - dialogueCloudSizes.y) borderRect.y = Screen.height - dialogueCloudSizes.y;

            GUI.Box(borderRect, currentItem.Quote.ToString());

        }

        #endregion

        #region InteractionsWithUI
        private void ProcessInteraction(bool isInteractCommandActive)
        {
            interactionActive = isInteractCommandActive;
            if (interactionActive)
            {
                BeginInteraction();
                return;
            }
            EndInteraction();
        }

        private void BeginInteraction()
        {
            if (DragDropOverInventoryUI())
            {
                var selectedInventoryItemIndex = DragDropOverItemUI();
                if (selectedInventoryItemIndex != -1)
                {
                    DragDropBeginInvoke(logicController?.ControlledUnit?.Inventory[selectedInventoryItemIndex]);
                }
            }
        }
        
        private void EndInteraction() => DragDropEndInvoke();
        #endregion

        #region DragDropLogic
        public void DragDropBeginInvoke(Item item)
        {
            if (item == null) return;

            if (dragDropItem != null)
                DragDropEndInvoke();

            dragDropItem = item;
            dragDropHaveItem = true;


            logicController?.ControlledUnit?.RemoveItemFromInventory(dragDropItem);
        }

        public void DragDropEndInvoke()
        {
            if (dragDropItem == null) return;

            if (DragDropOverInventoryUI())
            {
                logicController?.ControlledUnit?.AddItemToInteventory(dragDropItem);
                dragDropItem.DestroyOnGroundItem();
            }
            else
            {
                if (!dragDropHaveItem)
                    return;
                logicController?.ControlledUnit?.RemoveItemFromInventory(dragDropItem);
                var useResult = logicController?.UseItemOnWorld(dragDropItem) ?? InteractionResult.None;
                switch(useResult)
                {
                    case InteractionResult.NoInteractableItem when dragDropItem.OnGroundItem == null || dragDropItem.OnGroundItem.Destroyed:
                        logicController?.ControlledUnit?.AddItemToInteventory(dragDropItem);
                        break;
                    case InteractionResult.NotInUseRange when dragDropItem.OnGroundItem == null || dragDropItem.OnGroundItem.Destroyed:
                        logicController?.ControlledUnit?.AddItemToInteventory(dragDropItem);
                        break;
                    case InteractionResult.NotValidTarget:
                        logicController?.ControlledUnit?.AddItemToInteventory(dragDropItem);
                        dragDropItem.DestroyOnGroundItem();
                        break;
                    case InteractionResult.Success:
                        dragDropItem.DestroyOnGroundItem();
                        break;
                }
            }

            dragDropItem = null;
            dragDropHaveItem = false;
        }

        private int DragDropOverItemUI()
        {
            var mousePosition = new Vector2(UnityInput.mousePosition.x, Screen.height - UnityInput.mousePosition.y);
            for (int i = 0; i < inventoryItemsRects.Length; i++)
            {
                if (inventoryItemsRects[i].Contains(mousePosition))
                {
                    return i;
                }
            }
            return -1;
        }

        private bool DragDropOverInventoryUI()
        {
            var mousePosition = new Vector2(UnityInput.mousePosition.x, Screen.height - UnityInput.mousePosition.y);
            return inventoryAreaRect.Contains(mousePosition);
        }
        #endregion

        #region Interfaces realisation

        bool IInitializable.IsInitialized => isInitialized;

        private bool isInitialized;
        void IInitializable.Initialize()
        {
            inventoryItemsRects = new Rect[0];
            logicController = this.GetComponent<ILogicController>();
            InputManager.Instance.Subscribe(this);
            mainCamera = this.gameObject.GetComponentInChildren<Camera>();
            isInitialized = true;
            dragDropHaveItem = false;
        }

        void Airashe.UCore.Common.Behaviours.IObserver<InputCommandStateChanged>.OnObservableNotification(InputCommandStateChanged notificationData) => OnPlayerInput(notificationData);

        #endregion
    }
}
