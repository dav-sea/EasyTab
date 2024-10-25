using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace EasyTab
{
    public sealed class EasyTabDriver : IEasyTabDriver
    {
        [NotNull] private readonly IEasyTabDriverProvider _driverProvider;

        // list for temporary storage of the root objects of the scene. it is needed to avoid frequent allocations
        // The initial capacity is 0, so GetRootGameObjects will set the best capacity before filling
        private readonly List<GameObject> _listOfGameObjects = new List<GameObject>(0);

        public EasyTabDriver([NotNull] IEasyTabDriverProvider driverProvider)
        {
            _driverProvider = driverProvider ?? throw new ArgumentNullException(nameof(driverProvider));
        }

        public EasyTabNode GetParent(Target target)
        {
            switch (target.TargetType)
            {
                case TargetType.Transform:
                    var transform = target.AsTransform;
                    var transformParent = transform.parent;

                    Target parentTarget = transformParent
                        ? (Target) transformParent
                        : (Target) transform.gameObject.scene;

                    return _driverProvider.CreateNode(parentTarget);

                case TargetType.Scene:
                    return _driverProvider.CreateNode(Target.Root);

                case TargetType.Root:
                    return EasyTabNode.None;

                default:
                    return Fail_InvalidOperationForTargetType<EasyTabNode>(target.TargetType);
            }
        }

        public EasyTabNode GetChild(Target target, int childNumber)
        {
            switch (target.TargetType)
            {
                case TargetType.Root:
                    var scene = SceneManager.GetSceneAt(childNumber);
                    return _driverProvider.CreateNode(scene);

                case TargetType.Transform:
                    var childTransform = target.AsTransform.GetChild(childNumber);
                    return _driverProvider.CreateNode(childTransform);

                case TargetType.Scene:
                    // Method overloading with List<> is used to avoid allocations
                    target.AsScene.GetRootGameObjects(_listOfGameObjects);

                    var childGameObject = _listOfGameObjects[childNumber];

                    // It is necessary to clean the list so as not to store links and not to disrupt the GC
                    _listOfGameObjects.Clear();

                    return _driverProvider.CreateNode(childGameObject.transform);

                default:
                    return Fail_InvalidOperationForTargetType<EasyTabNode>(target.TargetType);
            }
        }

        public int GetChildrenCount(Target target)
        {
            switch (target.TargetType)
            {
                case TargetType.Root:
                    return SceneManager.sceneCount;

                case TargetType.Transform:
                    var transform = target.AsTransform;

                    if (!transform.gameObject.activeSelf)
                        return 0;

                    if (transform.TryGetComponent(out CanvasGroup canvasGroup))
                        if (!canvasGroup.interactable || canvasGroup.alpha == 0)
                            return 0;

                    if (transform.TryGetComponent<EasyTab>(out var easyTabComponent)
                        && easyTabComponent.ChildrenExtracting == ChildrenExtracting.WithoutChildren)
                        return 0;

                    return transform.childCount;

                case TargetType.Scene:
                    return target.AsScene.rootCount;

                default:
                    return Fail_InvalidOperationForTargetType<int>(target.TargetType);
            }
        }

        public bool IsSelectable(Target target)
        {
            switch (target.TargetType)
            {
                case TargetType.Root:
                    return false;

                case TargetType.Transform:
                    var transform = target.AsTransform;
                    if (!transform.gameObject.activeSelf)
                        return false;

                    if (transform.TryGetComponent<EasyTab>(out var easyTabComponent)
                        && easyTabComponent.SelectableRecognition == SelectableRecognition.AsNotSelectable)
                        return false;

                    return transform.TryGetComponent(out Selectable selectable)
                           && selectable.enabled
                           && selectable.interactable;

                case TargetType.Scene:
                    return false;

                default:
                    return Fail_InvalidOperationForTargetType<bool>(target.TargetType);
            }
        }

        public BorderMode GetBorderMode(Target target)
        {
            switch (target.TargetType)
            {
                case TargetType.Root:
                    return BorderMode.Roll;

                case TargetType.Transform:
                    var transform = target.AsTransform;
                    if (transform.TryGetComponent<EasyTab>(out var easyTabComponent))
                        return easyTabComponent.BorderMode;

#if TMP_PRESENT
                    if (transform.TryGetComponent<TMPro.TMP_Dropdown>(out _))
                        return BorderMode.Roll;
#endif

                    if (transform.TryGetComponent<Dropdown>(out _))
                        return BorderMode.Roll;

                    return GetParent(target).IsNone ? BorderMode.Roll : BorderMode.Escape;

                case TargetType.Scene:
                    return BorderMode.Escape;

                default:
                    return Fail_InvalidOperationForTargetType<BorderMode>(target.TargetType);
            }
        }

        private _ Fail_InvalidOperationForTargetType<_>(TargetType targetType)
            => throw new InvalidOperationException($"This operation cannot be performed for {targetType} target");
    }
}