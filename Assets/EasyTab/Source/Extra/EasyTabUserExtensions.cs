using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace EasyTab
{
    public static class EasyTabUserExtensions
    {
        public static IEasyTabCondition<T> Add<T>([NotNull] this List<IEasyTabCondition<T>> conditionsList,
            [NotNull] Predicate<T> predicate)
        {
            if (conditionsList == null) throw new NullReferenceException();
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            
            var condition = new EasyTabConditionPredicate<T>(predicate);
            conditionsList.Add(condition);

            return condition;
        }
    }
}