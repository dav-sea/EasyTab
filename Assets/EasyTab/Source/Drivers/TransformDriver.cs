using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace EasyTab
{
    public class TransformDriver : IEasyTabNodeDriver<Transform>
    {
        [NotNull] private readonly EasyTabNodeDriver _drivers;

        public TransformDriver([NotNull] EasyTabNodeDriver drivers)
        {
            _drivers = drivers ?? throw new ArgumentNullException(nameof(drivers));
        }

        public virtual EasyTabNode GetParent(Transform target)
        {
            var parent = target.parent;
            if (parent)
            {
                if (parent.TryGetComponent<EasyTab>(out var easyTab))
                    return new EasyTabNode(easyTab, _drivers.EasyTabDriver);

                return new EasyTabNode(parent, _drivers.TransformDriver);
            }

            return new EasyTabNode(target.gameObject.scene, _drivers.SceneDriver);
        }

        public virtual EasyTabNode GetChild(Transform target, int childNumber)
        {
            var child = target.GetChild(childNumber);
            if (child.TryGetComponent<EasyTab>(out var easyTabChild))
                return EasyTabNode.ByDriver(easyTabChild, _drivers.EasyTabDriver);

            return EasyTabNode.ByDriver(child, _drivers.TransformDriver);
        }

        public virtual int GetChildrenCount(Transform target) 
            => _drivers.Conditions.CanTraversingChildren(target) 
                ? target.childCount 
                : 0;

        public virtual bool IsSelectable(Transform target) 
            => _drivers.Conditions.CanSelect(target);

        public virtual BorderMode GetBorderMode(Transform target)
        {
            return GetParent(target).IsNone ? BorderMode.Roll : BorderMode.Escape;
        }

        BorderMode IEasyTabNodeDriver.GetBorderMode(object target)
        {
            return GetBorderMode((Transform)target);
        }

        EasyTabNode IEasyTabNodeDriver.GetChild(object target, int childNumber)
        {
            return GetChild((Transform)target, childNumber);
        }

        int IEasyTabNodeDriver.GetChildrenCount(object target)
        {
            return GetChildrenCount((Transform)target);
        }

        bool IEasyTabNodeDriver.IsSelectable(object target)
        {
            return IsSelectable((Transform)target);
        }

        EasyTabNode IEasyTabNodeDriver.GetParent(object target)
        {
            return GetParent((Transform)target);
        }
    }
}