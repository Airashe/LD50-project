using Assets.Scripts.LD50.EventSystem.Structs;
using LD50.Core.Interact;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.LD50.EventSystem.InteractionBehaviours.Common
{
    public sealed class ScriptableInteractBehaviourProxy : InteractBehaviour
    {
        [SerializeField]
        private ScriptableInteractBehaviour scriptable;
        private static GameObject proxy;

        public override InteractionResult InteractionBegin(GameObject behaviourSource)
        {
            var result = scriptable?.InteractionBegin(behaviourSource) ?? InteractionResult.NoInteractableItem;
            Destroy(proxy);
            return result;
        }

        public override void InteractionEnd(GameObject behaviourSource)
        {
            scriptable?.InteractionEnd(behaviourSource);
        }

        public static ScriptableInteractBehaviourProxy FromScriptable(ScriptableInteractBehaviour scriptable)
        {
            proxy = new GameObject();
            proxy.name = "Scriptable Interact Behaviour Proxy";
            var component = proxy.AddComponent<ScriptableInteractBehaviourProxy>();
            component.scriptable = scriptable;
            return component;
        }
    }
}
