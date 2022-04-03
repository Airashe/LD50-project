using Airashe.UCore.Common.Behaviours;
using Assets.Scripts.LD50.Core.Extensions;
using Assets.Scripts.LD50.DialogueSystem.Structs;
using LD50.Core;
using LD50.Core.Enums;
using LD50.DialogueSystem.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.LD50.VisualNovelSystem.Managers
{
    internal class VisualNovelManager : UnitySingleton<VisualNovelManager>, Airashe.UCore.Systems.ISystemManager
    {
        public bool Initialized => initialzed;
        private bool initialzed;

        public Sprite CurrentBackround
        {
            get => SpriteRenderer.sprite;
            set => SpriteRenderer.sprite = value;
        }
        
        public SpriteRenderer SpriteRenderer
        {
            get
            {
                if (spriteRenderer == null)
                {
                    var spriteGO = new GameObject();
                    spriteGO.name = "VisualNovel_Mode_SpriteRenderer";
                    DontDestroyOnLoad(spriteGO);
                    spriteGO.transform.SetParent(transform);
                    spriteGO.layer = GameApplication.Instance.applicationConfiguration.VisualNovelLayer.GetLayerIndex();
                    spriteRenderer = spriteGO.AddComponent<SpriteRenderer>();
                }
                return spriteRenderer;
            }
        }
        private SpriteRenderer spriteRenderer;

        private GameMode CurrentGameMode => GameApplication.Instance.GameMode;

        private void FixedUpdate()
        {
            if (CurrentGameMode == GameMode.VisualNovel)
                MakeSpriteGOTrackPlayer();
        }

        private void MakeSpriteGOTrackPlayer()
        {
            var goPlayer = GameApplication.Instance.PlayerGO;
            if (goPlayer == null)
                return;

            var goTransform = SpriteRenderer.transform;
            var newPosition = goPlayer.transform.position;
            newPosition.z += 10;
            goTransform.position = newPosition;
            SpriteRenderer.transform.localScale = Vector3.one * GetSpriteSize();
        }

        private Vector2 GetSpriteSize()
        {
            Vector2 topRightCorner = new Vector2(1, 1);
            Vector2 edgeVectorHeight = Camera.main.ViewportToWorldPoint(topRightCorner);
            var height = edgeVectorHeight.y * 2;

            Vector2 edgeVectorWidth = Camera.main.ViewportToWorldPoint(topRightCorner);
            var width = edgeVectorWidth.x * 2;

            return new Vector2(width, height);
        }

        public void StartLightNovelDialogueScript(DialogueData dialogue, DialogueContext context)
        {
            if (dialogue == null)
                return;

            DialoguesManager.Instance.ActivateDialogue(dialogue, context);
            DialoguesManager.Instance.GetDialogueNextItem();
        }

        public void InitializeManager()
        {
            initialzed = true;
        }
    }
}
