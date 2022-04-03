using Assets.Scripts.LD50.EventSystem.Structs;
using LD50.Core.Interact;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.LD50.EventSystem.InteractionBehaviours.Common
{
    public abstract class ScriptableInteractBehaviour : ScriptableObject
    {
        public virtual InteractionResult InteractionBegin(GameObject behaviourSource)
        {
            return InteractionResult.NoInteractableItem;
        }
        public virtual void InteractionEnd(GameObject behaviourSource)
        {
            
        }


        public static implicit operator InteractBehaviour (ScriptableInteractBehaviour scriptableInteractBehaviour)
        {
            return ScriptableInteractBehaviourProxy.FromScriptable(scriptableInteractBehaviour);
        }
    }
}
