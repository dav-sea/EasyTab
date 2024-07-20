using UnityEngine;
using UnityEngine.UI;

namespace EasyTab
{
    public sealed class SelectableIsInteractableAndEnabledCondition : IEasyTabCondition<Transform>
    {
        public bool IsMetFor(Transform transform)
        {
            if (!transform.TryGetComponent<Selectable>(out var selectable))
                return false;

            return selectable.enabled && selectable.interactable;
        }
    }
}