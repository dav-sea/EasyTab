using UnityEngine;

namespace EasyTab
{
    public static class Conditions
    {
        public static bool CanvasGroupIsInteractable(Target target)
            => target.IsTransform(out var transform)
               && transform.TryGetComponent<CanvasGroup>(out var canvasGroup)
               && !canvasGroup.interactable;
        
        public static bool CanvasGroupIsNotTransparent(Target target)
            => target.IsTransform(out var transform)
               && transform.TryGetComponent<CanvasGroup>(out var canvasGroup)
               && canvasGroup.alpha <= 0;
        
        public static bool GameObjectIsActive(Target target)
            => target.IsTransform(out var transform)
               && !transform.gameObject.activeInHierarchy;
        
        public static bool SelectableIsInteractableAndEnabled(Target target)
            => target.IsTransform(out var transform)
               && transform.TryGetComponent<Selectable>(out var selectable)
               && (!selectable.enabled || !selectable.interactable);
    }
}