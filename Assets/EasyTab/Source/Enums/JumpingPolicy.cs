using UnityEngine;

namespace EasyTab
{
    public enum JumpingPolicy
    {
        [InspectorName("Dont use jumping")]
        DontUseJumps,
        [InspectorName("Use only jumping")]
        UseOnlyJumps,
        [InspectorName("Use jumping or hierarchy")]
        UseJumpsOrHierarchy,
        [InspectorName("Use jumping or next of jump")]
        UseJumpsOrTheirNext
    }
}