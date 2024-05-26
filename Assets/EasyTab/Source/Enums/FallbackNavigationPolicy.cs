using System;

namespace EasyTab
{
    [Flags]
    public enum FallbackNavigationPolicy
    {
        Nothing,
        AllowNavigateToEntryPoint = 1 << 0,
        AllowNavigateToClosest = 1 << 1,
        AllowNavigateToLastSelected = 1 << 2,
        AllowAll = ~Nothing
    }
}