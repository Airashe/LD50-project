using Airashe.UCore.Systems.Input;
using Assets.Scripts.LD50.DialogueSystem.Structs;
using LD50.Controllers;
using LD50.Controllers.Interfaces;
using LD50.Core;
using LD50.Core.Extensions;
using LD50.Core.Interact;
using LD50.Input.Commands;
using LD50.Input.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityInput = UnityEngine.Input;

namespace Assets.Scripts.LD50.Controllers.VisualNovelControllers
{
    public sealed class VNLogicController : MonoBehaviour, Airashe.UCore.Common.Behaviours.IObserver<InputCommandStateChanged>, IInitializable, ILogicController
    {
        public IUIController UiController
        {
            get
            {
                if (uiController == null)
                {
                    uiController = gameObject.IntializeComponent<VNUIController>();
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
                    throw new NotImplementedException();
                }
                return dialogueController;
            }
        }
        private IDialogueController dialogueController;

        private bool interactionActive;
        private IInteractable currentInteraction;

        public Unit ControlledUnit
        {
            get => controlledUnit;
            set => controlledUnit = value;
        }
        [SerializeField]
        private Unit controlledUnit;

        private void OnPlayerInput(InputCommandStateData inputData)
        {
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

        private void BeginInteraction()
        {
            var ray = Camera.main.ScreenPointToRay(UnityInput.mousePosition);
            var hitResult = Physics2D.GetRayIntersection(ray, Mathf.Infinity);
            var interactableComponent = hitResult.collider?.transform.GetComponent<IInteractable>();

            currentInteraction = interactableComponent;
            currentInteraction?.InteractionBeginInvoke();
        }

        private void EndInteraction()
        {
            currentInteraction?.InteractionEndInvoke();
        }

        public InteractionResult UseItemOnWorld<T>(T item)
        {
            return InteractionResult.NoInteractableItem;
        }

        #region Interfaces realisation
        void Airashe.UCore.Common.Behaviours.IObserver<InputCommandStateChanged>.OnObservableNotification(InputCommandStateChanged notificationData) => OnPlayerInput(notificationData);

        bool IInitializable.IsInitialized => isInitialized;

        private bool isInitialized;
        void IInitializable.Initialize()
        {
            InputManager.Instance.Subscribe(this);
            uiController = gameObject.IntializeComponent<VNUIController>();
            isInitialized = true;
        }
        #endregion
    }
}
