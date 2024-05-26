using UnityEngine;

namespace EasyTab
{
    public enum ChildrenExtracting 
    {
        [InspectorName("By transform children")]
        ByTransformChildren,
        
        [InspectorName("Without children")]
        WithoutChildren,
    }
}