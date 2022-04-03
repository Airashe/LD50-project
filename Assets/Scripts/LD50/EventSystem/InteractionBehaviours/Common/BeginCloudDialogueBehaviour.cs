using Assets.Scripts.LD50.DialogueSystem.Structs;
using Assets.Scripts.LD50.EventSystem.Structs;
using LD50.Core.Interact;
using LD50Application = LD50.Core.GameApplication;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LD50.Controllers.Interfaces;

public class BeginCloudDialogueBehaviour : InteractBehaviour
{
    public DialogueContext dialogueContext;
    public DialogueData dialogue;

    public override InteractionResult InteractionBegin(GameObject source)
    {
        var currentDialogueController = LD50Application.Instance.PlayerGO?.GetComponent<IDialogueController>();
        if (currentDialogueController == null) return InteractionResult.NotInUseRange;

        currentDialogueController.StartDialogue(dialogue, dialogueContext);
        return InteractionResult.Success;
    }
}
