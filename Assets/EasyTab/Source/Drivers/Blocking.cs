using UnityEngine;
using UnityEngine.UI;

namespace EasyTab
{
    public static class Blocking
    {
        public static bool CanvasGroupIsNotInteractable(Target target)
            => target.IsTransform(out var transform)
               && transform.TryGetComponent<CanvasGroup>(out var canvasGroup)
               && !canvasGroup.interactable;
        
        public static bool CanvasGroupIsNotTransparent(Target target)
            => target.IsTransform(out var transform)
               && transform.TryGetComponent<CanvasGroup>(out var canvasGroup)
               && canvasGroup.alpha <= 0;
        
        public static bool GameObjectIsNotActive(Target target)
            => target.IsTransform(out var transform)
               && !transform.gameObject.activeInHierarchy;
    }
}