using UnityEngine;

namespace EasyTab
{
    public sealed class GameObjectIsActiveCondition : IEasyTabCondition<Transform>
    {
        public bool IsMetFor(Transform transform) 
            => transform.gameObject.activeInHierarchy;
    }
}