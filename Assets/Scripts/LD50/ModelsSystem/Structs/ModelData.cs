using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.LD50.ModelsSystem.Structs
{
    [Serializable]
    [CreateAssetMenu(fileName = "ModelData", menuName = "LD50/Model", order = 1)]
    public class ModelData : ScriptableObject
    {
        [SerializeField]
        private string currentAnimationName;
        public string CurrentAnimationName
        {
            get => currentAnimationName;
            set => currentAnimationName = value;
        }
        [SerializeField]
        private List<ModelAnimation> animations;

        [SerializeField]
        private ModelDirection currentDirection;
        public ModelDirection CurrentDirection
        {
            get => currentDirection;
            set => currentDirection = value;
        }

        public ModelAnimation CurrentAnimation
        {
            get
            {
                var activeAnimation = new ModelAnimation();
                foreach(var animation in animations)
                {
                    if(animation.name == currentAnimationName)
                    {
                        activeAnimation = animation;
                        break;
                    }
                }

                return activeAnimation;
            }
        }
    }
}
