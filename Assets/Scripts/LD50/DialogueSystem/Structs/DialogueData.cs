using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.LD50.DialogueSystem.Structs
{
    [Serializable]
    [CreateAssetMenu(fileName = "Dialogue", menuName = "LD50/Dialogue", order = 1)]
    public class DialogueData : ScriptableObject, IEquatable<DialogueData>
    {
        [SerializeField]
        private int dialogueIndex = -1;
        [SerializeField]
        public List<DialogueItem> items;
        public DialogueContext dialogueContext;

        public DialogueItem this[int index]

        {
            get
            {
                if (items == null) return DialogueItem.EndItem;
                return items[index];
            }
        }

        public int Length
        {
            get
            {
                if (items == null)
                    return 0;
                return items.Count;
            }
        }

        public bool Equals(DialogueData other)
        {
            if (other == null)
                return false;
            if (other.dialogueIndex == -1 || dialogueIndex == -1)
                return false;
            return other.dialogueIndex == dialogueIndex;
        }

        public override bool Equals(object other)
        {
            var data = other as DialogueData;
            if (other == null)
                return false;
            return Equals(data);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }
}
