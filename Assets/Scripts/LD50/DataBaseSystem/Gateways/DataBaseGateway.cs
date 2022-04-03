using Assets.Scripts.LD50.DialogueSystem.Structs;
using System;
using UnityEngine;

namespace Assets.Scripts.LD50.DataBaseSystem.Gateways
{
    [Serializable]
    [CreateAssetMenu(fileName = "Dialogue", menuName = "LD50/Data Base Gateway", order = 1)]
    public sealed class DataBaseGateway : ScriptableObject
    {
        public DialogueData[] dialoguesDatas;

        public DialogueData GetDialogueDataById(int id)
        {
            if (dialoguesDatas.Length <= id)
                return null;
            return dialoguesDatas[id];
        }
    }
}