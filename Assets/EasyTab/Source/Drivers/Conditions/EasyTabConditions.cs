using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace EasyTab
{
    [PublicAPI]
    public sealed class EasyTabConditions
    {
        [PublicAPI]
        public readonly List<IEasyTabCondition<Transform>> ConditionsForTraversingChildren;
        
        [PublicAPI]
        public readonly List<IEasyTabCondition<Transform>> ConditionsForSelectable;

        public EasyTabConditions()
        {
            var selectableCondition = new SelectableIsInteractableAndEnabledCondition();
            var isActiveGameObjectCondition = new GameObjectIsActiveCondition();
            var canvasGroupInteractableCondition = new CanvasGroupIsInteractableCondition();
            var canvasGroupNotTransparentCondition = new CanvasGroupIsNotTransparentCondition();

            ConditionsForTraversingChildren = new List<IEasyTabCondition<Transform>>(4)
            {
                isActiveGameObjectCondition,
                canvasGroupInteractableCondition,
                canvasGroupNotTransparentCondition
            };
            
            ConditionsForSelectable = new List<IEasyTabCondition<Transform>>(3)
            {
                isActiveGameObjectCondition,
                selectableCondition
            };
        }

        public bool CanTraversingChildren(Transform transform)
            => ConditionsIsMet(ConditionsForTraversingChildren, transform);

        public bool CanSelect(Transform transform)
            => ConditionsIsMet(ConditionsForSelectable, transform);

        private bool ConditionsIsMet<T>(List<IEasyTabCondition<T>> conditions, T obj)
        {
            foreach (var condition in conditions)
                if (!condition.IsMetFor(obj))
                    return false;

            return true;
        }
    }
}