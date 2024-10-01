using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace EasyTab
{
    [PublicAPI]
    public static class EasyTabUserExtensions
    {
        /// <summary>
        /// Checks if the specified <see cref="Target"/> is a <see cref="Transform"/> object.
        /// If it is, assigns the <see cref="Transform"/> to the output parameter and returns true.
        /// Otherwise, returns false and assigns the null value to the output parameter.
        /// </summary>
        /// <param name="target">The target object to check.</param>
        /// <param name="transform">The output parameter to store the <see cref="Transform"/> if the check is successful.</param>
        /// <returns>Returns true if the <see cref="Target"/> is a <see cref="Transform"/>, otherwise false.</returns>
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

        /// <summary>
        /// Creates a new <see cref="EasyTabNode"/> using the specified <see cref="Target"/> and the driver provided by the <see cref="IEasyTabDriverProvider"/>.
        /// </summary>
        /// <remarks><see cref="CreateNode"/>. accepts the <see cref="IEasyTabDriverProvider"/>. in order to use the entire chain of driver decorators.</remarks>
        /// <param name="self">An instance of <see cref="IEasyTabDriverProvider"/>.</param>
        /// <param name="target">The target object for the node creation.</param>
        /// <returns>Returns a new instance of <see cref="EasyTabNode"/> created with the specified <see cref="Target"/> and driver.</returns>
        /// <exception cref="NullReferenceException">Thrown if <paramref name="self"/> is null.</exception>
        [Pure]
        public static EasyTabNode CreateNode([NotNull] this IEasyTabDriverProvider self, Target target)
        {
            if (self == null) throw new NullReferenceException(nameof(self));
            var driver = self.GetDriver();
            return new EasyTabNode(target, driver);
        }

        /// <summary>
        /// Converts an <see cref="IEasyTabDriver"/> into a <see cref="DecoratableEasyTabDriver"/>, 
        /// allowing the addition of custom decorators.
        /// </summary>
        /// <param name="self">The instance of <see cref="IEasyTabDriver"/> to be converted.</param>
        /// <returns>Returns an instance of <see cref="DecoratableEasyTabDriver"/>.</returns>
        /// <exception cref="NullReferenceException">Thrown if <paramref name="self"/> is null.</exception>
        [Pure]
        public static DecoratableEasyTabDriver ToDecoratable(this IEasyTabDriver self)
        {
            if (self == null) throw new NullReferenceException(nameof(self));
            return new DecoratableEasyTabDriver(self);
        }

        /// <summary>
        /// Adds a border mode decorator to the <see cref="IEasyTabDriver"/>, 
        /// modifying its behavior for handling borders for target objects.
        /// </summary>
        /// <param name="self">The instance of <see cref="IEasyTabDriver"/> to decorate.</param>
        /// <param name="getBorderModeDecorator">The function to modify the <see cref="BorderMode"/> based on the target.</param>
        /// <returns>Returns the decorated <see cref="IEasyTabDriver"/> with a border mode decorator.</returns>
        /// <exception cref="NullReferenceException">Thrown if <paramref name="self"/> is null.</exception>
        [Pure]
        public static IEasyTabDriver DecorateBorderMode([NotNull] this IEasyTabDriver self,
            Func<IEasyTabDriver, Target, BorderMode> getBorderModeDecorator)
        {
            if (self == null) throw new NullReferenceException(nameof(self));

            var decoratable = self.ToDecoratable();
            decoratable.GetBorderModeDelegate = getBorderModeDecorator;
            return decoratable;
        }

        /// <summary>
        /// Adds a parent node retrieval decorator to the <see cref="IEasyTabDriver"/>, 
        /// allowing custom logic to define the parent of a node.
        /// </summary>
        /// <param name="self">The instance of <see cref="IEasyTabDriver"/> to decorate.</param>
        /// <param name="getParentDelegate">A function to determine the parent <see cref="EasyTabNode"/> based on the target.</param>
        /// <returns>Returns the decorated <see cref="IEasyTabDriver"/> with a parent node retrieval decorator.</returns>
        /// <exception cref="NullReferenceException">Thrown if <paramref name="self"/> is null.</exception>
        [Pure]
        public static IEasyTabDriver DecorateGetParent(this IEasyTabDriver self,
            Func<IEasyTabDriver, Target, EasyTabNode> getParentDelegate)
        {
            if (self == null) throw new NullReferenceException(nameof(self));

            var decoratable = self.ToDecoratable();
            decoratable.GetParentDelegate = getParentDelegate;
            return decoratable;
        }

        /// <summary>
        /// Adds a child node retrieval decorator to the <see cref="IEasyTabDriver"/>, 
        /// allowing custom logic to define how child nodes are retrieved based on the target and index.
        /// </summary>
        /// <param name="self">The instance of <see cref="IEasyTabDriver"/> to decorate.</param>
        /// <param name="getChildDelegate">A function to determine the child <see cref="EasyTabNode"/> based on the target and index.</param>
        /// <returns>Returns the decorated <see cref="IEasyTabDriver"/> with a child node retrieval decorator.</returns>
        /// <exception cref="NullReferenceException">Thrown if <paramref name="self"/> is null.</exception>
        [Pure]
        public static IEasyTabDriver DecorateGetChild(this IEasyTabDriver self,
            Func<IEasyTabDriver, Target, int, EasyTabNode> getChildDelegate)
        {
            if (self == null) throw new NullReferenceException(nameof(self));

            var decoratable = self.ToDecoratable();
            decoratable.GetChildDelegate = getChildDelegate;
            return decoratable;
        }

        /// <summary>
        /// Adds a children count decorator to the <see cref="IEasyTabDriver"/>, 
        /// allowing custom logic to determine the number of children for a target.
        /// </summary>
        /// <param name="self">The instance of <see cref="IEasyTabDriver"/> to decorate.</param>
        /// <param name="getChildrenCount">A function to determine the number of children for the target.</param>
        /// <returns>Returns the decorated <see cref="IEasyTabDriver"/> with a children count decorator.</returns>
        /// <exception cref="NullReferenceException">Thrown if <paramref name="self"/> is null.</exception>
        [Pure]
        public static IEasyTabDriver DecorateGetChildrenCount(this IEasyTabDriver self,
            Func<IEasyTabDriver, Target, int> getChildrenCount)
        {
            if (self == null) throw new NullReferenceException(nameof(self));

            var decoratable = self.ToDecoratable();
            decoratable.GetChildrenCountDelegate = getChildrenCount;
            return decoratable;
        }

        /// <summary>
        /// Adds a selectable state decorator to the <see cref="IEasyTabDriver"/>, 
        /// allowing custom logic to determine if a target is selectable.
        /// </summary>
        /// <param name="self">The instance of <see cref="IEasyTabDriver"/> to decorate.</param>
        /// <param name="isSelectableDelegate">A function to determine if the target is selectable.</param>
        /// <returns>Returns the decorated <see cref="IEasyTabDriver"/> with a selectable state decorator.</returns>
        /// <exception cref="NullReferenceException">Thrown if <paramref name="self"/> is null.</exception>
        [Pure]
        public static IEasyTabDriver DecorateIsSelectable(this IEasyTabDriver self,
            Func<IEasyTabDriver, Target, bool> isSelectableDelegate)
        {
            if (self == null) throw new NullReferenceException(nameof(self));

            var decoratable = self.ToDecoratable();
            decoratable.IsSelectableDelegate = isSelectableDelegate;
            return decoratable;
        }

        /// <summary>
        /// Adds a decorator that blocks selection of targets based on a predicate. 
        /// If the predicate returns true, the target will not be selectable.
        /// </summary>
        /// <param name="self">The instance of <see cref="IEasyTabDriver"/> to decorate.</param>
        /// <param name="blockingPredicate">A function that determines if the target should be blocked from being selectable.</param>
        /// <returns>Returns the decorated <see cref="IEasyTabDriver"/> with target selection blocking logic.</returns>
        [Pure]
        public static IEasyTabDriver WithTargetSelectableBlocking(this IEasyTabDriver self,
            Predicate<Target>  blockingPredicate)
            => self.DecorateIsSelectable((driver, target) => !blockingPredicate(target) && driver.IsSelectable(target));

        /// <summary>
        /// Adds a decorator that blocks child traversal based on a predicate. 
        /// If the predicate returns true, the target will not have any child nodes.
        /// </summary>
        /// <param name="self">The instance of <see cref="IEasyTabDriver"/> to decorate.</param>
        /// <param name="blockingPredicate">A function that determines if the target's child nodes should be blocked.</param>
        /// <returns>Returns the decorated <see cref="IEasyTabDriver"/> with child traversal blocking logic.</returns>
        [Pure]
        public static IEasyTabDriver WithTargetTraversingChildrenBlocking(this IEasyTabDriver self,
            Predicate<Target> blockingPredicate)
            => self.DecorateGetChildrenCount((driver, target) =>
                blockingPredicate(target) ? 0 : driver.GetChildrenCount(target));

        /// <summary>
        /// Adds a decorator that blocks selection of <see cref="Transform"/> targets based on a predicate. 
        /// If the predicate returns true, the <see cref="Transform"/> will not be selectable.
        /// </summary>
        /// <param name="self">The instance of <see cref="IEasyTabDriver"/> to decorate.</param>
        /// <param name="blockingPredicate">A function that determines if the <see cref="Transform"/> should be blocked from being selectable.</param>
        /// <returns>Returns the decorated <see cref="IEasyTabDriver"/> with <see cref="Transform"/> selection blocking logic.</returns>
        [Pure]
        public static IEasyTabDriver WithSelectableBlocking(this IEasyTabDriver self,
            Predicate<Transform> blockingPredicate)
            => self.DecorateIsSelectable((driver, target) =>
                target.IsTransform(out var transform) && !blockingPredicate(transform) && driver.IsSelectable(target));

        /// <summary>
        /// Adds a decorator that blocks child traversal for <see cref="Transform"/> targets based on a predicate. 
        /// If the predicate returns true, the <see cref="Transform"/> will not have any child nodes.
        /// </summary>
        /// <param name="self">The instance of <see cref="IEasyTabDriver"/> to decorate.</param>
        /// <param name="blockingPredicate">A function that determines if the <see cref="Transform"/> target's child nodes should be blocked.</param>
        /// <returns>Returns the decorated <see cref="IEasyTabDriver"/> with child traversal blocking logic for <see cref="Transform"/> targets.</returns>
        [Pure]
        public static IEasyTabDriver WithTraversingChildrenBlocking(this IEasyTabDriver self,
            Predicate<Transform> blockingPredicate)
            => self.DecorateGetChildrenCount((driver, target) =>
                target.IsTransform(out var transform) && blockingPredicate(transform)
                    ? 0
                    : driver.GetChildrenCount(target));
    }
}