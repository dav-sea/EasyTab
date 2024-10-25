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
        [Pure]
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
    }
}