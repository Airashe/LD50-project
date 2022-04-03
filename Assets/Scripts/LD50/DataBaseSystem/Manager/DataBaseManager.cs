using Airashe.UCore.Common.Behaviours;
using Assets.Scripts.LD50.DataBaseSystem.Gateways;
using Assets.Scripts.LD50.DialogueSystem.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.LD50.DataBaseSystem.Manager
{
    public sealed class DataBaseManager : UnitySingleton<DataBaseManager>, Airashe.UCore.Systems.ISystemManager
    {
        [SerializeField]
        private DataBaseGateway databaseGateway;

        public bool Initialized => databaseGateway != null && SingletonInitialized;

        public DialogueData GetDialogueDataById(int id)
        {
            if (databaseGateway == null)
                throw new ApplicationException("Database gateway instance is null.");
            return databaseGateway.GetDialogueDataById(id);
        }

        public DialogueData GetDialogueData(DialogueData dialogueData)
        {
            if (databaseGateway == null)
                return dialogueData == null ? new DialogueData() : dialogueData;
            
            foreach (var dialogueInGateway in databaseGateway.dialoguesDatas)
            {
                if (dialogueInGateway.Equals(dialogueData))
                    return dialogueData;
            }
            return dialogueData ?? ScriptableObject.CreateInstance<DialogueData>();
        }

        public void InitializeManager()
        {
            return;
        }
    }
}
