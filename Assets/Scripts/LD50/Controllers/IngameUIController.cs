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
using LD50.Core.Interact;

namespace Assets.Scripts.LD50.Controllers
{
    public sealed class IngameUIController : MonoBehaviour, IInitializable, IUIController, Airashe.UCore.Common.Behaviours.IObserver<InputCommandStateChanged>
    {
        public ILogicController LogicController => logicController;
        private ILogicController logicController;

        private Item dragDropItem;
        [SerializeField]
        private DynamicRect inventoryAreaRect = new DynamicRect(50, 92.5f, 73.68f, 15);
        [SerializeField]
        private int ItemsInInventoryCount => logicController?.ControlledUnit?.Inventory?.Count ?? 0;
        private int previousRenderedItemsCount;
        private Rect[] inventoryItemsRects;

        private bool inventoryIsActive = true;
        private bool interactionActive = false;

        private void OnPlayerInput(InputCommandStateData inputData)
        {
            if (inputData.CommandType == InputCommandType.None) return;

            if (inputData.CommandType == InputCommandType.Interact)
            {
                ProcessInteraction(inputData.IsActive);
                return;
            }
        }

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

        #region Render
        private void OnGUI()
        {
            RenderInventory();
            RenderDragDropItem();
        }

        private void RenderDragDropItem()
        {
            if (dragDropItem?.ItemData?.Texture == null) return;

            var texture = dragDropItem.ItemData.Texture;
            var rect = new Rect(UnityInput.mousePosition.x, Screen.height - UnityInput.mousePosition.y, texture.width, texture.height);
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
                    inventoryItemsRects[i] = new Rect(rectStartPosition, new Vector2(40, 40));
                    GUI.Button(inventoryItemsRects[i], "");

                    rectStartPosition.x += 2.5f;
                    rectStartPosition.y += 2.5f;
                    GUI.DrawTexture(new Rect(rectStartPosition, new Vector2(35, 35)), itemIcon);
                }
            }
        }
        #endregion

        #region DragDropLogic
        public void DragDropBeginInvoke(Item item)
        {
            if (item == null) return;

            if (dragDropItem != null)
                DragDropEndInvoke();

            dragDropItem = item;

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
                logicController?.ControlledUnit?.RemoveItemFromInventory(dragDropItem);
                var useResult = logicController?.UseItemOnWorld(dragDropItem) ?? InteractionResult.None;
                switch(useResult)
                {
                    case InteractionResult.NoInteractableItem when dragDropItem.OnGroundItem == null:
                        logicController?.ControlledUnit?.AddItemToInteventory(dragDropItem);
                        break;
                    case InteractionResult.NotInUseRange when dragDropItem.OnGroundItem == null:
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
            logicController = this.GetComponent<ILogicController>();
            InputManager.Instance.Subscribe(this);
            isInitialized = true;
        }

        void Airashe.UCore.Common.Behaviours.IObserver<InputCommandStateChanged>.OnObservableNotification(InputCommandStateChanged notificationData) => OnPlayerInput(notificationData);

        #endregion
    }
}
