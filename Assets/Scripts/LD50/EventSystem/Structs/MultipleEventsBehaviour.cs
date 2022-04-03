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
        public ItemInteractBehaviour[] eventsByItem;
        public InteractBehaviour[] events;

        private Item interactionItem;
        public override InteractionResult InteractionWith(GameObject source, Item item)
        {
            foreach (var e in eventsByItem)
            {
                if (e != null && !(e is ItemInteractBehaviour))
                    e.InteractionBegin(source);
            }
            return eventsByItem.Length == 0 ? InteractionResult.NotValidTarget : InteractionResult.Success;
        }

        public override InteractionResult InteractionBegin(GameObject source)
        {
            foreach (var e in events)
            {
                if (e != null && !(e is ItemInteractBehaviour))
                    e.InteractionBegin(source);
            }
            return InteractionResult.Success;
        }
    }
}
