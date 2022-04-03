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
    public class MultipleEventsBehaviour : ItemInteractBehaviour
    {
        public InteractBehaviour[] events;

        private Item interactionItem;
        public override InteractionResult InteractionWith(GameObject source, Item item)
        {
            interactionItem = item;
            InteractionBegin(source);
            return InteractionResult.Success;
        }

        public override InteractionResult InteractionBegin(GameObject source)
        {
            foreach (var e in events)
            {
                var itemE = e as ItemInteractBehaviour;
                if(itemE != null)
                {
                    itemE.InteractionWith(source, interactionItem);
                    continue;
                }
                e.InteractionBegin(source);
            }
            return InteractionResult.Success;
        }
    }
}
