using JetBrains.Annotations;

namespace EasyTab
{
    /// <summary>
    /// This structure is an element of the hierarchy of objects in Unity.
    /// Provides a uniform API for working with hierarchy, encapsulating a driver that can work with either IsTransform, AsScene, or root
    /// </summary>
    public struct EasyTabNode
    {
        private readonly Target _target;

        [NotNull] private readonly IEasyTabDriver _targetDriver;

        public bool IsSelectable => _targetDriver.IsSelectable(_target);
        public EasyTabNode Parent => _targetDriver.GetParent(_target);
        public BorderMode BorderMode => _targetDriver.GetBorderMode(_target);

        public int ChildrenCount => _targetDriver.GetChildrenCount(_target);

        public bool IsNone => _target.IsNone;

        public static EasyTabNode None => default;

        public Target Target => _target;

        internal EasyTabNode(Target target, [NotNull] IEasyTabDriver easyTabTargetDriver)
        {
            _target = target;
            _targetDriver = easyTabTargetDriver;
        }

        public EasyTabNode GetChild(int childNumber)
        {
            return _targetDriver.GetChild(_target, childNumber);
        }

        public int GetChildNumberOf(EasyTabNode tab)
        {
            for (int i = 0; i < ChildrenCount; i++)
                if (tab._target.Equals(GetChild(i)._target))
                    return i;

            return -1;
        }
    }
}