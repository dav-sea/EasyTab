using UnityEngine;

namespace EasyTab
{
    public sealed class CanvasGroupIsInteractableCondition  : IEasyTabCondition<Transform> 
    {
        public bool IsMetFor(Transform transform)
        {
            if (!transform.TryGetComponent<CanvasGroup>(out var canvasGroup))
                return true;

            return canvasGroup.interactable;
        }
    }
}