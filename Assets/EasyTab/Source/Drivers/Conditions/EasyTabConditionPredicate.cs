using System;
using JetBrains.Annotations;

namespace EasyTab
{
    public sealed class EasyTabConditionPredicate<T> : IEasyTabCondition<T>
    {
        [NotNull] private readonly Predicate<T> _predicate;

        public EasyTabConditionPredicate([NotNull] Predicate<T> predicate) 
            => _predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));

        public bool IsMetFor(T obj)
            => _predicate(obj);
    }
}