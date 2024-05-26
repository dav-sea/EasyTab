using System;
using JetBrains.Annotations;

namespace EasyTab
{
    public struct EasyTabNode
    {
        [CanBeNull] private readonly object _target;

        [NotNull] private readonly IEasyTabNodeDriver _nodeDriver;

        public bool IsSelectable => _nodeDriver.IsSelectable(_target);
        public EasyTabNode Parent => _nodeDriver.GetParent(_target);
        public BorderMode BorderMode => _nodeDriver.GetBorderMode(_target);

        public int ChildrenCount => _nodeDriver.GetChildrenCount(_target);

        public bool IsNone => _nodeDriver is null;

        public static EasyTabNode None => new EasyTabNode();

        internal EasyTabNode([CanBeNull] object target, [NotNull] IEasyTabNodeDriver easyTabNodeDriver)
        {
            _target = target;
            _nodeDriver = easyTabNodeDriver;
        }

        public static EasyTabNode ByDriver<T>([NotNull] T target, [NotNull] IEasyTabNodeDriver<T> driver)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));
            if (driver == null) throw new ArgumentNullException(nameof(driver));
            
            return new EasyTabNode(target, driver);
        }

        public EasyTabNode GetChild(int childNumber)
        {
            return _nodeDriver.GetChild(_target, childNumber);
        }

        public int GetChildNumberOf(EasyTabNode tab)
        {
            for (int i = 0; i < ChildrenCount; i++)
                // todo potential NRE
                if (tab._target.Equals(GetChild(i)._target))
                    return i;

            return -1;
        }

        public object GetTarget() => _target;
    }
}