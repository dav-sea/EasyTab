using System;
using JetBrains.Annotations;

namespace EasyTab
{
    public sealed class DecoratableEasyTabDriver : IEasyTabDriver
    {
        public readonly IEasyTabDriver Decorated;

        public  Func<IEasyTabDriver, Target, BorderMode> GetBorderModeDelegate;
        public  Func<IEasyTabDriver, Target, EasyTabNode> GetParentDelegate;
        public  Func<IEasyTabDriver, Target, int, EasyTabNode> GetChildDelegate;
        public  Func<IEasyTabDriver, Target, int> GetChildrenCountDelegate;
        public  Func<IEasyTabDriver, Target, bool> IsSelectableDelegate;
        
        public DecoratableEasyTabDriver([NotNull] IEasyTabDriver decorated)
            => Decorated = decorated ?? throw new ArgumentNullException(nameof(decorated));

        public BorderMode GetBorderMode(Target target)
        {
            if(GetBorderModeDelegate == null)
                return Decorated.GetBorderMode(target);

            return GetBorderModeDelegate(Decorated, target);
        }

        public EasyTabNode GetParent(Target target)
        {
            if(GetParentDelegate == null)
                return Decorated.GetParent(target);

            return GetParentDelegate(Decorated, target);
        }

        public EasyTabNode GetChild(Target target, int childNumber)
        {
            if(GetChildDelegate == null)
                return Decorated.GetChild(target, childNumber);

            return GetChildDelegate(Decorated, target, childNumber);
        }

        public int GetChildrenCount(Target target)
        {
            if(GetChildrenCountDelegate == null)
                return Decorated.GetChildrenCount(target);

            return GetChildrenCountDelegate(Decorated, target);
        }

        public bool IsSelectable(Target target)
        {
            if(IsSelectableDelegate == null)
                return Decorated.IsSelectable(target);

            return IsSelectableDelegate(Decorated, target);
        }
    }

    public abstract class TargetDriverDecorator : IEasyTabDriver
    {
        public readonly IEasyTabDriver Decorated;

        public TargetDriverDecorator([NotNull] IEasyTabDriver decorated)
            => Decorated = decorated ?? throw new ArgumentNullException(nameof(decorated));

        public virtual BorderMode GetBorderMode(Target target) => Decorated.GetBorderMode(target);
        public virtual EasyTabNode GetParent(Target target) => Decorated.GetParent(target);
        public virtual EasyTabNode GetChild(Target target, int childNumber) => Decorated.GetChild(target, childNumber);
        public virtual int GetChildrenCount(Target target) => Decorated.GetChildrenCount(target);
        public virtual bool IsSelectable(Target target) => Decorated.IsSelectable(target);
    }
}