using Assets.Scripts.LD50.DialogueSystem.Interfaces;
using LD50.Core.Interact;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.LD50.DialogueSystem.Structs
{
    [Serializable]
    public class DialogueContext
    {
        public static readonly DialogueContext Empty = new DialogueContext(null);

        [SerializeField]
        private Actor[] dialogueParticiants;
        public Actor[] DialogueParticiants => dialogueParticiants;

        public DialogueContext(IEnumerable<Actor> particiants)
        {
            this.dialogueParticiants = particiants?.ToArray() ?? Array.Empty<Actor>();
        }
    }
}
