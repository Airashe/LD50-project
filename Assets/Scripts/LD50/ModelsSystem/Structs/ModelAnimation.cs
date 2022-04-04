using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.LD50.ModelsSystem.Structs
{
    [Serializable]
    public struct ModelAnimation
    {
        public static readonly ModelAnimation Empty = new ModelAnimation();
        public string name;
        [SerializeField]
        private float playSpeedFront;
        public Sprite[] frontKeys;
        [SerializeField]
        private float playSpeedBack;
        public Sprite[] backKeys;
        [SerializeField]
        private float playSpeedLeft;
        public Sprite[] leftKeys;
        [SerializeField]
        private float playSpeedRight;
        public Sprite[] rightKeys;

        public Sprite[] this[ModelDirection direction]
        {
            get
            {
                switch(direction)
                {
                    case ModelDirection.Front:
                        return frontKeys ?? new Sprite[0];
                    case ModelDirection.Back:
                        return backKeys ?? new Sprite[0];
                    case ModelDirection.Left:
                        return leftKeys ?? new Sprite[0];
                    case ModelDirection.Right:
                        return rightKeys ?? new Sprite[0];
                }
                return new Sprite[0];
            }
        }

        public float GetPlaySpeed(ModelDirection direction)
        {
            switch (direction)
            {
                case ModelDirection.Front:
                    return playSpeedFront;
                case ModelDirection.Back:
                    return playSpeedBack;
                case ModelDirection.Left:
                    return playSpeedLeft;
                case ModelDirection.Right:
                    return playSpeedRight;
            }
            return 0;
        }
    }
}
