using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace EasyTab
{
    public static class EasyTabUserExtensions
    {
        public static bool IsTransform(in this Target target, out Transform transform)
        {
            if (target.IsTransform)
            {
                transform = target.AsTransform;
                return true;
            }

            transform = default;
            return false;
        }
        
        public static EasyTabNode CreateNode([NotNull] this IEasyTabDriverProvider self, Target target)
        {
            if (self == null) throw new NullReferenceException(nameof(self));
            var driver = self.GetDriver();
            return new EasyTabNode(target, driver);
        }
        
        public static DecoratableEasyTabDriver ToDecoratable(this IEasyTabDriver self)
        {
            if (self == null) throw new NullReferenceException(nameof(self));
            return new DecoratableEasyTabDriver(self);
        }

        public static IEasyTabDriver DecorateBorderMode([NotNull] this IEasyTabDriver self,
            Func<IEasyTabDriver, Target, BorderMode> getBorderModeDecorator)
        {
            if (self == null) throw new NullReferenceException(nameof(self));

            var decoratable = self.ToDecoratable();
            decoratable.GetBorderModeDelegate = getBorderModeDecorator;
            return decoratable;
        }

        public static IEasyTabDriver DecorateGetParent(this IEasyTabDriver self,
            Func<IEasyTabDriver, Target, EasyTabNode> getParentDelegate)
        {
            if (self == null) throw new NullReferenceException(nameof(self));

            var decoratable = self.ToDecoratable();
            decoratable.GetParentDelegate = getParentDelegate;
            return decoratable;
        }

        public static IEasyTabDriver DecorateGetChild(this IEasyTabDriver self,
            Func<IEasyTabDriver, Target, int, EasyTabNode> getChildDelegate)
        {
            if (self == null) throw new NullReferenceException(nameof(self));

            var decoratable = self.ToDecoratable();
            decoratable.GetChildDelegate = getChildDelegate;
            return decoratable;
        }

        public static IEasyTabDriver DecorateGetChildrenCount(this IEasyTabDriver self,
            Func<IEasyTabDriver, Target, int> getChildrenCount)
        {
            if (self == null) throw new NullReferenceException(nameof(self));

            var decoratable = self.ToDecoratable();
            decoratable.GetChildrenCountDelegate = getChildrenCount;
            return decoratable;
        }

        public static IEasyTabDriver DecorateIsSelectable(this IEasyTabDriver self,
            Func<IEasyTabDriver, Target, bool> isSelectableDelegate)
        {
            if (self == null) throw new NullReferenceException(nameof(self));

            var decoratable = self.ToDecoratable();
            decoratable.IsSelectableDelegate = isSelectableDelegate;
            return decoratable;
        }
        
        public static IEasyTabDriver WithSelectableBlocking(this IEasyTabDriver self,
            Func<Target, bool> blockingPredicate)
            => self.DecorateIsSelectable((driver, target) => !blockingPredicate(target) && driver.IsSelectable(target));
        
        public static IEasyTabDriver WithSelectableBlocking(this IEasyTabDriver self,
            Func<Transform, bool> blockingPredicate)
            => self.DecorateIsSelectable((driver, target) => !blockingPredicate(target) && driver.IsSelectable(target));

        public static IEasyTabDriver WithTraversingChildrenBlocking(this IEasyTabDriver self,
            Func<Target, bool> blockingPredicate)
            => self.DecorateGetChildrenCount((driver, target) => blockingPredicate(target) ? 0 : driver.GetChildrenCount(target));
        
        
    }
}