using Assets.Scripts.LD50.VisualNovelSystem.Managers;
using LD50.Controllers;
using LD50.Core;
using LD50.Core.Interact;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.LD50.EventSystem.InteractionBehaviours.Common.VisualNovel
{
    [CreateAssetMenu(fileName = "Item", menuName = "LD50/Events/Common/Visual Novel mode/Change Backround", order = 1)]
    public class ChangeBackroundScriptableBehaviour : ScriptableInteractBehaviour
    {
        public Sprite newBackground;

        public override InteractionResult InteractionBegin(GameObject behaviourSource)
        {
            VisualNovelManager.Instance.CurrentBackround = newBackground;

            return InteractionResult.Success;
        }
    }
}
