using Assets.Scripts.LD50.Interact;
using System;
using UnityEngine;

namespace LD50.Interact.Items
{
    [CreateAssetMenu(fileName = "Item", menuName = "Core/Item", order = 1)]
    public class ItemData : ScriptableObject
    {
        public string Name;
        public Sprite Sprite;
        public Texture2D Texture
        {
            get
            {
                if (Sprite == null) return null;

                if (Sprite.rect.width != Sprite.texture.width)
                {
                    Texture2D newText = new Texture2D((int)Sprite.rect.width, (int)Sprite.rect.height);
                    Color[] newColors = Sprite.texture.GetPixels((int)Sprite.textureRect.x,
                                                                 (int)Sprite.textureRect.y,
                                                                 (int)Sprite.textureRect.width,
                                                                 (int)Sprite.textureRect.height);
                    newText.SetPixels(newColors);
                    newText.Apply();
                    newText.filterMode = FilterMode.Point;
                    newText.Compress(true);
                    return newText;
                }
                else
                    return Sprite.texture;
            }
        }
    }
}
