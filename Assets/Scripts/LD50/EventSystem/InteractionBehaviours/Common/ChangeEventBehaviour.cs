using Assets.Scripts.LD50.EventSystem.Structs;
using LD50.Core.Interact;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeEventBehaviour : InteractBehaviour
{
    public InteractBehaviour newBehaviour;

    public override InteractionResult InteractionBegin(GameObject behaviourSource)
    {
        if (behaviourSource == null) return InteractionResult.NoInteractableItem;

        var prevBehaviours = behaviourSource.GetComponents<InteractBehaviour>();
        foreach (var prevBehaviour in prevBehaviours)
        {
            if (prevBehaviours != null)
                Destroy(prevBehaviour);
        }

        if (newBehaviour == null) return InteractionResult.Success;
        behaviourSource.GetComponent<Actor>().InteractBehaviour = newBehaviour;
        return InteractionResult.Success;
    }
}
