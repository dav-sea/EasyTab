using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace EasyTab
{
    public class TransformDriver : IEasyTabNodeDriver
    {
        [NotNull] private readonly EasyTabNodeDriver _drivers;

        public TransformDriver([NotNull] EasyTabNodeDriver drivers)
        {
            _drivers = drivers ?? throw new ArgumentNullException(nameof(drivers));
        }

        public virtual EasyTabNode GetParent(TransformOrScene target)
        {
            var transform = target.AsTransform;
            var parent = transform.parent;
            if (parent)
            {
                return new EasyTabNode(parent, _drivers.TransformDriver);
            }

            return new EasyTabNode(transform.gameObject.scene, _drivers.SceneDriver);
        }

        public virtual EasyTabNode GetChild(TransformOrScene target, int childNumber)
        {
            var child = target.AsTransform.GetChild(childNumber);
            return new EasyTabNode(child, _drivers.TransformDriver);
        }

        public virtual int GetChildrenCount(TransformOrScene target)
        {
            var transform = target.AsTransform;
            if (transform.TryGetComponent<EasyTab>(out var easyTabComponent)
                && easyTabComponent.ChildrenExtracting == ChildrenExtracting.WithoutChildren)
                return 0;

            return _drivers.Conditions.CanTraversingChildren(transform)
                ? transform.childCount
                : 0;
        }

        public virtual bool IsSelectable(TransformOrScene target)
        {
            var transform = target.AsTransform;
            if (transform.TryGetComponent<EasyTab>(out var easyTabComponent)
                && easyTabComponent.SelectableRecognition == SelectableRecognition.AsNotSelectable)
                return false;

            return _drivers.Conditions.CanSelect(transform);
        }

        public virtual BorderMode GetBorderMode(TransformOrScene target)
        {
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
        }
    }
}