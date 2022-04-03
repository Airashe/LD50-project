using Airashe.UCore.Common.Behaviours;
using Assets.Scripts.LD50.DataBaseSystem.Manager;
using Assets.Scripts.LD50.DialogueSystem.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LD50.DialogueSystem.Managers
{
    public sealed class DialoguesManager : UnitySingleton<DialoguesManager>, Airashe.UCore.Systems.ISystemManager
    {
        public bool Initialized => intialized && SingletonInitialized;
        private bool intialized;

        [SerializeField]
        private DialogueData activeDialogue;
        [SerializeField]
        private int currentItemIndex;

        private DataBaseManager dataBaseManager => DataBaseManager.Instance;

        public void InitializeManager()
        {
            intialized = true;
        }

        public void ActivateDialogue(int dialogueId)
        {
            ActivateDialogue(dataBaseManager.GetDialogueDataById(dialogueId));
        }

        public void ActivateDialogue(DialogueData dialogue)
        {
            if (dialogue == null)
                activeDialogue = new DialogueData();

            activeDialogue = dataBaseManager.GetDialogueData(dialogue);
            currentItemIndex = -1;
        }

        public DialogueItem GetDialogueNextItem()
        {
            currentItemIndex++;
            if (currentItemIndex >= activeDialogue.Length) return DialogueItem.EndItem;

            return activeDialogue[currentItemIndex];
        }
    }
}
