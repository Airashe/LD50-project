using Airashe.UCore.Common.Behaviours;
using Airashe.UCore.Systems.Input;
using Assets.Scripts.LD50.Controllers;
using LD50.Controllers;
using LD50.Controllers.Interfaces;
using LD50.Core.Extensions;
using LD50.Core.Interact;
using LD50.Input.Commands;
using LD50.Input.Structs;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityInput = UnityEngine.Input;

namespace LD50.Core.Controllers
{
    [Serializable]
    public class IngameLogicController : MonoBehaviour, Airashe.UCore.Common.Behaviours.IObserver<InputCommandStateChanged>, IInitializable, ILogicController
    {
        public IUIController UiController
        {
            get
            {
                if (uiController == null)
                {
                    uiController = gameObject.IntializeComponent<IngameUIController>();
                }
                return uiController;
            }
        }
        private IUIController uiController;

        public IDialogueController DialogueController
        {
            get
            {
                if (uiController == null)
                {
                    dialogueController = gameObject.IntializeComponent<IngameDialogueController>();
                }
                return dialogueController;
            }
        }
        private IDialogueController dialogueController;

        public Unit ControlledUnit
        {
            get => controlledUnit;
            set => controlledUnit = value;
        }

        [SerializeField]
        private Unit controlledUnit;

        private bool interactionActive;
        private IInteractable currentInteraction;

        private void Update()
        {
            if (ControlledUnit == null)
                return;

            FollowControlledUnit();
        }

        private void FixedUpdate()
        {
            if (ControlledUnit == null)
                return;

            MoveConrolledUnit();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(ControlledUnit?.transform.position ?? Vector3.zero, ControlledUnit?.InteractionMaxDistance ?? 0);
        }

        private void OnPlayerInput(InputCommandStateData inputData)
        {
            if (ControlledUnit == null || inputData.CommandType == InputCommandType.None) return;
            if ((inputData.CommandType & InputCommandType.MovementMask) == inputData.CommandType)
            {
                MoveControlledUnitUnit(inputData.CommandType, inputData.IsActive);
                return;
            }
            
            if (inputData.CommandType == InputCommandType.Interact)
            {
                ProcessInteraction(inputData.IsActive);
                return;
            }
        }

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

        #region InteractionsWithWorld

        private void BeginInteraction()
        {
            var ray = Camera.main.ScreenPointToRay(UnityInput.mousePosition);
            var hitResult = Physics2D.GetRayIntersection(ray, Mathf.Infinity);
            var interactableComponent = hitResult.collider?.transform.GetComponent<IInteractable>();
            if (!InteractableInRange(interactableComponent)) return;

            currentInteraction = interactableComponent;
            currentInteraction?.InteractionBeginInvoke();
        }

        public InteractionResult UseItemOnWorld<T>(T item)
        {
            if (item == null) return InteractionResult.None;

            var ray = Camera.main.ScreenPointToRay(UnityInput.mousePosition);
            var hitResult = Physics2D.GetRayIntersection(ray, Mathf.Infinity);
            var interactableComponent = hitResult.collider?.transform.GetComponent<IInteractable<T>>();
            if (!InteractableInRange(interactableComponent)) return InteractionResult.NotInUseRange;

            return interactableComponent?.BeginInteractionWith(item) ?? InteractionResult.NoInteractableItem;
        }

        private void EndInteraction()
        {
            currentInteraction?.InteractionEndInvoke();
        }

        private bool InteractableInRange(IInteractable target)
        {
            if (target?.GameObject == null) return false;
            var targetPosition = new Vector2(target.GameObject.transform.position.x, target.GameObject.transform.position.y);
            var distance = Vector2.Distance(transform.position, targetPosition + target.InteractionPivot);
            return distance <= (ControlledUnit?.InteractionMaxDistance ?? 0) + target.InteractionRadius;
        }

        #endregion

        #region MovementControl
        private void FollowControlledUnit() => this.transform.position = controlledUnit.transform.position;

        private void MoveConrolledUnit() => ControlledUnit.Rigidbody.velocity = ControlledUnit.MovementDirection * ControlledUnit.WalkSpeed;

        private void MoveControlledUnitUnit(InputCommandType inputCommand, bool isActive)
        {
            var moveCommands = new List<int>() {
                (int)InputCommandType.MoveForward,
                (int)InputCommandType.MoveBackward,
                (int)InputCommandType.MoveLeft,
                (int)InputCommandType.MoveRight, 
            };
            if (isActive)
                moveCommands.Remove((int)inputCommand);

            var activeCommand = isActive ? inputCommand : InputCommandType.None;
            if (activeCommand == InputCommandType.None)
            {
                activeCommand = (InputCommandType)InputManager.Instance.IsAnyActive(moveCommands);
                isActive = activeCommand != InputCommandType.None;
            }

            var movementDirection = Vector3.zero;
            switch (activeCommand)
            {
                case InputCommandType.MoveForward:
                    movementDirection.y = isActive ? 1 : 0;
                    break;
                case InputCommandType.MoveBackward:
                    movementDirection.y = isActive ? -1 : 0;
                    break;
                case InputCommandType.MoveLeft:
                    movementDirection.x = isActive ? -1 : 0;
                    break;
                case InputCommandType.MoveRight:
                    movementDirection.x = isActive ? 1 : 0;
                    break;
            }

            ControlledUnit.MovementDirection = movementDirection;
        }
        #endregion 

        #region Interfaces realisation
        void Airashe.UCore.Common.Behaviours.IObserver<InputCommandStateChanged>.OnObservableNotification(InputCommandStateChanged notificationData) => OnPlayerInput(notificationData);

        bool IInitializable.IsInitialized => isInitialized;

        private bool isInitialized;
        void IInitializable.Initialize()
        {
            InputManager.Instance.Subscribe(this);
            uiController = gameObject.IntializeComponent<IngameUIController>();
            dialogueController = gameObject.IntializeComponent<IngameDialogueController>();

            isInitialized = true;
        }
        #endregion
    }

}