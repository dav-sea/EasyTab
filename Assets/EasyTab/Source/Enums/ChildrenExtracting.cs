using UnityEngine;

namespace EasyTab
{
    public enum ChildrenExtracting 
    {
        [InspectorName("By transform children")]
        ByTransformChildren = 0,
        
        [InspectorName("Without children")]
        WithoutChildren = 1,
    }
}