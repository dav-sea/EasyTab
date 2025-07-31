#if UNITY_EDITOR
using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace EasyTab.Internals
{
    internal static class EasyTabEditorEntryPoint
    {
        private const string PathToLoggedVersionFile = "./Library/EasyTabLoggedVersion.txt";

        private static bool TryGetLoggedVersion(out string lastLoggedVersion)
        {
            lastLoggedVersion = Utils.PackageVersion;
            if (!File.Exists(PathToLoggedVersionFile))
                return false;

            lastLoggedVersion = File.ReadAllText(PathToLoggedVersionFile);
            return true;
        }

        private static void SetLastLoggedNewVersion(string lastLoggedVersion)
        {
            File.WriteAllText(PathToLoggedVersionFile, lastLoggedVersion);
        }

        [InitializeOnLoadMethod]
        private static void Init()
        {
            try
            {
                PrintMessageIfNeeded();
            }
            catch (Exception e)
            {
#if EASYTAB_DEVELOPMENT
                Debug.LogException(e);
#endif
            }
            finally
            {
            }
        }
        
        private static void PrintMessageIfNeeded()
        {
            if (TryGetLoggedVersion(out var lastLoggedVersion) && lastLoggedVersion == Utils.PackageVersion) 
                return;
            
            PrintLogAboutPackage();
            SetLastLoggedNewVersion(Utils.PackageVersion);
        }

        private static void PrintLogAboutPackage()
        {
            var message =
                $"<b><color=#0077FF>Easy</color>Tab</b>. The project has a package that implements the Tab key navigation functionality.\n" +
                "\n" +
                "Thanks for using EasyTab, you can rate the package on the AssetStore or GitHub.\n" +
                "\n" +
                $"<b>GitHub</b>:  {Utils.LinkToRateGithub}\n" +
                $"<b>AssetStore</b>:  {Utils.LinkToRateAssetStore}\n" +
                "\n" +
                $"Developer's email address: {Utils.DeveloperEmail}\n" +
                "\n" +
                $"Package version: {Utils.PackageVersion}\n";

            Debug.LogFormat(LogType.Log, LogOption.NoStacktrace, null, message);
        }
    }
}
#endif