using Assets.Scripts.LD50.Interact.Items;
using LD50.Core.Interact;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.LD50.EventSystem.Structs
{
    public abstract class InteractBehaviour : MonoBehaviour
    {

        public virtual InteractionResult InteractionBegin(GameObject behaviourSource)
        {
            return InteractionResult.NoInteractableItem;
        }

        public virtual void InteractionEnd(GameObject behaviourSource)
        { }
    }
}
