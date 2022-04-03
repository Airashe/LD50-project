using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.LD50.Core.Extensions
{
    public static class LayerMaskExtensions
    {
        public static int GetLayerIndex (this LayerMask mask)
        {
            int layerNumber = 0;
            int layer = mask.value;
            while (layer > 1)
            {
                layer = layer >> 1;
                layerNumber++;
            }
            return layerNumber;
        }
    }
}
