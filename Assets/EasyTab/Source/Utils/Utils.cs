using System;
using UnityEngine.LowLevel;

namespace EasyTab.Internals
{
    internal static class Utils
    {
        internal const string LinkToReportIssues = "https://github.com/dav-sea/EasyTab/issues";
        internal const string LinkToRateAssetStore = "https://link.davsea.com/easy-tab/rate/assetStore";
        internal const string LinkToRateGithub = "https://link.davsea.com/easy-tab/rate/github";
        internal const string DeveloperEmail = "mail@davsea.com";
        
        internal const string PackageVersion = "1.3.1";
        
        internal static int GetSystemIndex(this in PlayerLoopSystem playerLoopSystem, Type type) 
            => Array.FindIndex(playerLoopSystem.subSystemList, e => e.type == type);

        internal static bool Has(this FallbackNavigationPolicy self, FallbackNavigationPolicy policy) 
            => (self & policy) == policy;
    }
}