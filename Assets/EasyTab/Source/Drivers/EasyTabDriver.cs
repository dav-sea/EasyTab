using System;
using EasyTab.Internals;
using UnityEngine;

namespace EasyTab
{
    public class EasyTabDriver : IEasyTabNodeDriver<EasyTab>
    {
        private readonly EasyTabNodeDriver _drivers;

        public EasyTabDriver(EasyTabNodeDriver drivers)
        {
            _drivers = drivers;
        }

        public BorderMode GetBorderMode(EasyTab target)
        {
            return target.BorderMode;
        }
        
        public bool IsSelectable(EasyTab target)
        {
            switch (target.SelectableRecognition)
            {
                case SelectableRecognition.ByDriver:
                    return _drivers.TransformDriver.IsSelectable(target.transform);

                default:
                case SelectableRecognition.AsNotSelectable:
                    return false;
            }
        }

        public EasyTabNode GetParent(EasyTab target)
        {
            return _drivers.TransformDriver.GetParent(target.transform);
        }

        public EasyTabNode GetChild(EasyTab target, int childNumber)
        {
            switch (target.ChildrenExtracting)
            {
                case ChildrenExtracting.ByTransformChildren:
                    return _drivers.TransformDriver.GetChild(target.transform, childNumber);
                
                default:
                case ChildrenExtracting.WithoutChildren:
                    throw new InvalidOperationException();
            }
        }

        public int GetChildrenCount(EasyTab target)
        {
            switch (target.ChildrenExtracting)
            {
                case ChildrenExtracting.ByTransformChildren:
                    return _drivers.TransformDriver.GetChildrenCount(target.transform);
                
                default:
                case ChildrenExtracting.WithoutChildren:
                    return 0;
            }
        }
        
        BorderMode IEasyTabNodeDriver.GetBorderMode(object target)
        {
            return GetBorderMode((EasyTab)target);
        }

        EasyTabNode IEasyTabNodeDriver.GetChild(object target, int childNumber)
        {
            return GetChild((EasyTab)target, childNumber);
        }

        int IEasyTabNodeDriver.GetChildrenCount(object target)
        {
            return GetChildrenCount((EasyTab)target);
        }

        bool IEasyTabNodeDriver.IsSelectable(object target)
        {
            return IsSelectable((EasyTab)target);
        }

        EasyTabNode IEasyTabNodeDriver.GetParent(object target)
        {
            return GetParent((EasyTab)target);
        }
    }
}