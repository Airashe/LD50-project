using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.LD50.DialogueSystem.Interfaces
{
    public interface IDialogueParticiant
    {
        public string ActorName { get; }
        public Vector3 ParticiantWorldPosition { get; }
    }
}
