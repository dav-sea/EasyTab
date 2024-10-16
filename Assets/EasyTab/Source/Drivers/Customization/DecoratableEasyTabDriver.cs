using System;
using JetBrains.Annotations;

namespace EasyTab
{
    /// <summary>
    /// A decorator class that extends the functionality of an <see cref="IEasyTabDriver"/> by allowing the addition of custom behaviors 
    /// through delegates. It enables dynamic modification of key operations such as determining the border mode, 
    /// parent and child nodes, children count, and selectability of targets.
    /// </summary>
    [PublicAPI]
    public sealed class DecoratableEasyTabDriver : IEasyTabDriver
    {
        public readonly IEasyTabDriver Decorated;

        public Func<IEasyTabDriver, Target, BorderMode> GetBorderModeDelegate;
        public Func<IEasyTabDriver, Target, EasyTabNode> GetParentDelegate;
        public Func<IEasyTabDriver, Target, int, EasyTabNode> GetChildDelegate;
        public Func<IEasyTabDriver, Target, int> GetChildrenCountDelegate;
        public Func<IEasyTabDriver, Target, bool> IsSelectableDelegate;

        public DecoratableEasyTabDriver([NotNull] IEasyTabDriver decorated)
            => Decorated = decorated ?? throw new ArgumentNullException(nameof(decorated));

        public BorderMode GetBorderMode(Target target)
        {
            if (GetBorderModeDelegate == null)
                return Decorated.GetBorderMode(target);

            return GetBorderModeDelegate(Decorated, target);
        }

        public EasyTabNode GetParent(Target target)
        {
            if (GetParentDelegate == null)
                return Decorated.GetParent(target);

            return GetParentDelegate(Decorated, target);
        }

        public EasyTabNode GetChild(Target target, int childNumber)
        {
            if (GetChildDelegate == null)
                return Decorated.GetChild(target, childNumber);

            return GetChildDelegate(Decorated, target, childNumber);
        }

        public int GetChildrenCount(Target target)
        {
            if (GetChildrenCountDelegate == null)
                return Decorated.GetChildrenCount(target);

            return GetChildrenCountDelegate(Decorated, target);
        }

        public bool IsSelectable(Target target)
        {
            if (IsSelectableDelegate == null)
                return Decorated.IsSelectable(target);

            return IsSelectableDelegate(Decorated, target);
        }
    }
}