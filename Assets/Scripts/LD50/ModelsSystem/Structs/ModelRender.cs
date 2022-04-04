using Airashe.UCore.Common.Structures;
using Assets.Scripts.LD50.TickSystem.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.LD50.ModelsSystem.Structs
{
    public class ModelRender : MonoBehaviour, Airashe.UCore.Common.Behaviours.IObserver<int>
    {
        [SerializeField]
        private ModelData model;
        public ModelDirection ModelDirection
        {
            get => model?.CurrentDirection ?? ModelDirection.Front;
            set
            {
                if (model != null)
                    model.CurrentDirection = value;
            }
        }
        [SerializeField]
        private float defaultPlaySpeed;
        private float playSpeed;
        private int lastTickFrameChage;
        [SerializeField]
        private SpriteRenderer spriteRenderer;

        private Sprite[] currentFramesList;
        private ModelAnimation currentModelAnimation;
        private BorderedValue<int> currentFrameIndex;

        private bool connectedToTickManager = false;

        private void Start()
        {
            PlayAnimation(model?.CurrentAnimationName ?? "Idle");

            if (TickManager.Instance == null)
                return;
            TickManager.Instance.Subscribe(this);

            if (spriteRenderer == null)
                spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        }

        public void PlayAnimation(string name, float? playSpeed = null)
        {
            if (model == null) return;
            if (string.IsNullOrEmpty(name)) return;
            model.CurrentAnimationName = name;
            currentModelAnimation = model.CurrentAnimation;

            if (playSpeed == null)
            {
                if (currentModelAnimation.GetPlaySpeed(ModelDirection) < 1)
                    playSpeed = defaultPlaySpeed;
                else
                    playSpeed = currentModelAnimation.GetPlaySpeed(ModelDirection);
            }

            this.playSpeed = (float)playSpeed;            
            currentFramesList = currentModelAnimation[model.CurrentDirection];
            currentFrameIndex = new BorderedValue<int>(currentFrameIndex.Value, 0, currentFramesList.Length - 1, true);
        }

        public void PlayOneShotAnimation(string name, float? playSpeed = null)
        {

        }

        private void Update()
        {
            if (model == null || (currentFramesList?.Length ?? 0) == 0)
                return;

            if (!connectedToTickManager)
            {
                if (TickManager.Instance == null)
                    return;

                TickManager.Instance.Subscribe(this);
                connectedToTickManager = true;
            }
        }

        public void OnObservableNotification(int currentTicks)
        {
            var canChageFrame = (currentTicks - lastTickFrameChage) >= playSpeed;
            if (canChageFrame)
                ChangeFrameIndex(currentTicks);
        }

        private void ChangeFrameIndex(int currentTicks)
        {
            lastTickFrameChage = currentTicks;
            currentFrameIndex.Value += 1;
            spriteRenderer.sprite = currentFramesList[currentFrameIndex];
        }
    }

}
