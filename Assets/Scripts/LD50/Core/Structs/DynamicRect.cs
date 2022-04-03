using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.LD50.Core.Structs
{
    [Serializable]
    public struct DynamicRect
    {
        public float X
        {
            get => position.x;
            set => position.x = value;
        }
        public float Y
        {
            get => position.y;
            set => position.y = value;
        }
        public float Width
        {
            get => size.x;
            set => size.x = value;
        }
        public float Height
        {
            get => size.y;
            set => size.y = value;
        }

        public float AbsoluteX => Screen.width * X / 100;
        public float AbsoluteY => Screen.height * Y / 100;
        public float AbsoluteWidth => Screen.width * (Width / 100);
        public float AbsoluteHeight => Screen.width * (Height / 100);

        public float LeftCornerAbsouleX => AbsoluteX - AbsoluteWidth / 2;
        public float LeftCornerAbsouleY => AbsoluteY - AbsoluteHeight / 2;


        [SerializeField]
        private Vector2 position;
        [SerializeField]
        private Vector2 size;

        public DynamicRect(float x, float y, float width, float height) : this(new Vector2(x, y), new Vector2(width, height))
        {
            
        }

        public DynamicRect(Vector2 position, Vector2 size)
        {
            this.position = position;
            this.size = size;
        }

        public static implicit operator Rect (DynamicRect drect)
        {
            var size = new Vector2(drect.AbsoluteWidth, drect.AbsoluteHeight);
            var position = new Vector2(drect.LeftCornerAbsouleX, drect.LeftCornerAbsouleY);
            return new Rect(position, size);
        }

        public bool Contains(Vector2 position)
        {
            return position.x >= LeftCornerAbsouleX && position.x <= LeftCornerAbsouleX + AbsoluteWidth &&
                   position.y >= LeftCornerAbsouleY && position.y <= LeftCornerAbsouleY + AbsoluteHeight;
        }
    }
}
