using System;
using UnityEngine.LowLevel;

namespace EasyTab.Internals
{
    internal static class Utils
    {
        internal const string LinkToReportIssues = "https://github.com/dav-sea/EasyTab/issues";
        
        internal static int GetSystemIndex(this in PlayerLoopSystem playerLoopSystem, Type type) 
            => Array.FindIndex(playerLoopSystem.subSystemList, e => e.type == type);

        internal static bool Has(this FallbackNavigationPolicy self, FallbackNavigationPolicy policy) 
            => (self & policy) == policy;
    }
}