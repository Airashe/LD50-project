using LD50.Interact.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.LD50.Interact.Items
{
    [Serializable]
    public class Item
    {
        [SerializeField]
        private ItemData itemData;
        public ItemData ItemData => itemData;

        public OnGroundItem OnGroundItem => onGroundItem;
        [SerializeField]
        private OnGroundItem onGroundItem;

        public Item(ItemData data, OnGroundItem onGroundItem)
        {
            this.itemData = data;
            this.onGroundItem = onGroundItem;
        }

        public void DestroyOnGroundItem()
        {
            if (onGroundItem != null)
                GameObject.Destroy(onGroundItem.gameObject);
        }

        public override string ToString()
        {
            return $"Item ({itemData?.Name ?? String.Empty})";
        }
    }
}
