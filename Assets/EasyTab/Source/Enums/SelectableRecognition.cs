using UnityEngine;

namespace EasyTab
{
    public enum SelectableRecognition
    {
        [InspectorName("Auto (by Driver)")]
        ByDriver = 0,
        [InspectorName("As not selectable")]
        AsNotSelectable = 1,
    }
}