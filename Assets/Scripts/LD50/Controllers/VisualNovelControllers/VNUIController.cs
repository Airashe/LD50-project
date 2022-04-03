using Airashe.UCore.Systems.Input;
using Assets.Scripts.LD50.Core.Structs;
using Assets.Scripts.LD50.DialogueSystem.Structs;
using Assets.Scripts.LD50.Interact.Items;
using LD50.Controllers;
using LD50.Controllers.Interfaces;
using LD50.Core;
using LD50.Core.Interact;
using LD50.DialogueSystem.Managers;
using LD50.Input.Commands;
using LD50.Input.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityInput = UnityEngine.Input;
using LD50Application = LD50.Core.GameApplication;
using Assets.Scripts.LD50.VisualNovelSystem.Managers;
using Assets.Scripts.LD50.Core.Common;

namespace Assets.Scripts.LD50.Controllers.VisualNovelControllers
{
    public class VNUIController : MonoBehaviour, IInitializable, IUIController, Airashe.UCore.Common.Behaviours.IObserver<InputCommandStateChanged>
    {
        private ILogicController logicController;
        private Camera mainCamera;

        private Item dragDropItem;
        private bool dragDropHaveItem;

        [SerializeField]
        private DynamicRect inventoryAreaRect = new DynamicRect(50, 92.5f, 73.68f, 15);
        [SerializeField]
        private DynamicRect dialogueQouteRect = new DynamicRect(50, 92.5f, 73.68f, 15);

        private DynamicRect dialogeAnswerButtonRect = new DynamicRect(50, 50, 10, 5);
        

        private Rect[] inventoryItemsRects;
        private Rect[] answersRects;
        private Rect answersAreaRect;

        private bool inventoryIsActive = false;
        private bool interactionActive = false;
        private EvaluationString lanstQouteLine = EvaluationString.Empty;

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
            if (inventoryIsActive)
            {
                return;
            }
            
            DrawDialogueBox();
            DrawDialogueChoise();
        }

        private void DrawDialogueChoise()
        {
            if (DialoguesManager.Instance.CurrentItem.Type != DialogueSystem.Enums.DialogueItemType.Answer)
                return;

            var currentItem = DialoguesManager.Instance.CurrentItem;
            GUI.Box(answersAreaRect, "");
            for(int i = 0; i < DialoguesManager.Instance.CurrentItem.Answers.Length; i++)
            {
                GUI.Button(answersRects[i], DialoguesManager.Instance.CurrentItem.Answers[i].ToString());
            }
        }

        private void DrawDialogueBox()
        {
            var lastQuoteItem = DialoguesManager.Instance.CurrentItem.Type == DialogueSystem.Enums.DialogueItemType.Quote ? DialoguesManager.Instance.CurrentItem : DialoguesManager.Instance.LastQuoteItem;
            if (lastQuoteItem == null) return;

            Rect currentRect = dialogueQouteRect;
            GUI.Box(currentRect, "");
            var nameRect = currentRect;
            nameRect.position += new Vector2(10, 10);
            nameRect.size = new Vector2(currentRect.width - 20, 30);
            GUI.Label(nameRect, lastQuoteItem.Interactor.ActorName);
            currentRect.position += new Vector2(15, 25);
            currentRect.size -= new Vector2(30, 50);
            GUI.Label(currentRect, lanstQouteLine.ToString());
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
            var answering = DialoguesManager.Instance.CurrentItem.Type == DialogueSystem.Enums.DialogueItemType.Answer;
            if (MouseOverAnswerArea())
            {
                ProcessClickOnAnswerButton();
                return;
            }
            if (DragDropOverInventoryUI())
            {
                var selectedInventoryItemIndex = DragDropOverItemUI();
                if (selectedInventoryItemIndex != -1)
                {
                    DragDropBeginInvoke(logicController?.ControlledUnit?.Inventory[selectedInventoryItemIndex]);
                }
                return;
            }
            if (!answering)
            {
                NextFrame();
            }
        }

        private void ProcessClickOnAnswerButton()
        {
            var mousePosition = new Vector2(Screen.width, Screen.height - Input.mousePosition.y);
            for(int i = 0; i < answersRects.Length; i++)
            {
                if (answersRects[i].Contains(mousePosition))
                {
                    DialoguesManager.Instance.ChoseAnswer(gameObject, i);
                    break;
                }
            }
        }

        private bool MouseOverAnswerArea()
        {
            var answering = DialoguesManager.Instance.CurrentItem.Type == DialogueSystem.Enums.DialogueItemType.Answer;
            var mousePosition = new Vector2(Screen.width, Screen.height - Input.mousePosition.y);
            return answering && answersAreaRect.Contains(mousePosition);
        }

        private void EndInteraction() => DragDropEndInvoke();
        #endregion

        #region DialogueStepsLogic

        private void NextFrame()
        {
            if (!lanstQouteLine.Evaluated)
            {
                lanstQouteLine.Evaluated = true;
                return;
            }

            DialoguesManager.Instance.GetDialogueNextItem();
            if (DialoguesManager.Instance.CurrentItem.Type == DialogueSystem.Enums.DialogueItemType.Answer)
            {
                var itemsCount = DialoguesManager.Instance.CurrentItem.Answers.Length;
                answersRects = new Rect[itemsCount];
                answersAreaRect = new DynamicRect(50, 50, dialogeAnswerButtonRect.Width + 2, dialogeAnswerButtonRect.Height * itemsCount + 2);

                Rect currentAreaRect = answersAreaRect;
                Rect buttonRect = dialogeAnswerButtonRect;
                buttonRect.position = currentAreaRect.position;

                for (int i = 0; i < answersRects.Length; i++)
                {
                    buttonRect.position = new Vector2(currentAreaRect.x + Screen.width / 100, currentAreaRect.position.y + (buttonRect.height + Screen.height/100) * i);
                    buttonRect.y += Screen.height / 100;
                    answersRects[i] = buttonRect;
                }
            }
            GetCurrentLine();
        }

        private void GetCurrentLine()
        {
            var lastQuoteDialogueItem = DialoguesManager.Instance.CurrentItem;
            if (lastQuoteDialogueItem.Type == DialogueSystem.Enums.DialogueItemType.Answer)
            {
                lastQuoteDialogueItem = DialoguesManager.Instance.LastQuoteItem;
            }

            lanstQouteLine = lastQuoteDialogueItem?.Quote?.ToEvaluationString(lastQuoteDialogueItem?.EvalutaionSpeed ?? 0) ?? string.Empty;
            lanstQouteLine.Evaluating = true;
            lanstQouteLine.Evaluated = DialoguesManager.Instance.CurrentItem.Type == DialogueSystem.Enums.DialogueItemType.Answer;
        }
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
                var inventoryItem = dragDropItem.OnGroundItem == null || dragDropItem.OnGroundItem.Destroyed;
                var groundItem = dragDropItem.OnGroundItem != null && !dragDropItem.OnGroundItem.Destroyed;
                switch (useResult)
                {
                    case InteractionResult.NotValidTarget when groundItem:
                        logicController?.ControlledUnit?.AddItemToInteventory(dragDropItem);
                        dragDropItem.DestroyOnGroundItem();
                        break;

                    case InteractionResult.NotInUseRange when groundItem:
                        dragDropItem.ReturnOnGroundItem();
                        break;
                    case InteractionResult.NoInteractableItem when groundItem:
                        dragDropItem.ReturnOnGroundItem();
                        break;

                    case InteractionResult.NoInteractableItem when inventoryItem:
                        logicController?.ControlledUnit?.AddItemToInteventory(dragDropItem);
                        break;
                    case InteractionResult.NotInUseRange when inventoryItem:
                        logicController?.ControlledUnit?.AddItemToInteventory(dragDropItem);
                        break;
                    case InteractionResult.NotValidTarget when inventoryItem:
                        logicController?.ControlledUnit?.AddItemToInteventory(dragDropItem);
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
            return inventoryIsActive && inventoryAreaRect.Contains(mousePosition);
        }
        #endregion

        #region Interfaces realisation

        bool IInitializable.IsInitialized => isInitialized;

        public ILogicController LogicController => logicController;

        private bool isInitialized;
        void IInitializable.Initialize()
        {
            inventoryItemsRects = new Rect[0];
            logicController = this.GetComponent<ILogicController>();
            InputManager.Instance.Subscribe(this);
            mainCamera = this.gameObject.GetComponentInChildren<Camera>();
            isInitialized = true;
            dragDropHaveItem = false;
            VisualNovelManager.Instance.StartLightNovelDialogueScript(LD50Application.Instance.CurrentSceneContext.Dialogue, LD50Application.Instance.CurrentSceneContext.DialogueContext);
            GetCurrentLine();
        }

        void Airashe.UCore.Common.Behaviours.IObserver<InputCommandStateChanged>.OnObservableNotification(InputCommandStateChanged notificationData) => OnPlayerInput(notificationData);

        #endregion
    }
}
