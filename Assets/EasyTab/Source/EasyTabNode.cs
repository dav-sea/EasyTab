using JetBrains.Annotations;

namespace EasyTab
{
    /// <summary>
    /// This structure is an element of the hierarchy of objects in Unity.
    /// Provides a uniform API for working with hierarchy, encapsulating a driver that can work with either AsTransform, AsScene, or root
    /// </summary>
    public struct EasyTabNode
    {
        private readonly TransformOrScene _target;

        [NotNull] private readonly IEasyTabNodeDriver _nodeDriver;

        public bool IsSelectable => _nodeDriver.IsSelectable(_target);
        public EasyTabNode Parent => _nodeDriver.GetParent(_target);
        public BorderMode BorderMode => _nodeDriver.GetBorderMode(_target);

        public int ChildrenCount => _nodeDriver.GetChildrenCount(_target);

        public bool IsNone => _nodeDriver is null;

        public static EasyTabNode None => new EasyTabNode();

        internal EasyTabNode(TransformOrScene target, [NotNull] IEasyTabNodeDriver easyTabNodeDriver)
        {
            _target = target;
            _nodeDriver = easyTabNodeDriver;
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

        public TransformOrScene GetTarget2() => _target;
    }
}