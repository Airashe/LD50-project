using Assets.Scripts.LD50.DialogueSystem.Structs;
using LD50.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.LD50.Core.Application.Structs
{
    public sealed class UnitySceneContext : MonoBehaviour
    {
        [Header("Common")]
        [SerializeField]
        private GameMode intitalGameMode;
        public GameMode IntitalGameMode => intitalGameMode;
        [SerializeField]
        private Unit playerUnit;
        public Unit PlayerUnit => playerUnit;

        [Header("VisualNovel Mode context")]
        [SerializeField]
        private DialogueData dialogue;
        public DialogueData Dialogue => dialogue;
        [SerializeField]
        private DialogueContext dialogueContext;
        public DialogueContext DialogueContext => dialogueContext;
    }
}
