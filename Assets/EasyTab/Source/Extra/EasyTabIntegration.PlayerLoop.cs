using System;
using System.Linq;
using EasyTab.Internals;
using UnityEngine;
using UnityEngine.LowLevel;

namespace EasyTab
{
    public sealed partial class EasyTabIntegration
    {
        private static bool _globallyEnabled;
        private static EasyTabIntegration _globally;

        public static EasyTabIntegration Globally
        {
            get
            {
#if UNITY_EDITOR
                if (!Application.isPlaying)
                    throw new InvalidOperationException("Cant access to property in edit-mode");
#endif

                return _globally;
            }
        }

        public static bool GloballyEnabled
        {
            get
            {
#if UNITY_EDITOR
                if (!Application.isPlaying)
                    throw new InvalidOperationException("Cant access to property in edit-mode");
#endif

                return _globallyEnabled;
            }

            set
            {
#if UNITY_EDITOR
                if (!Application.isPlaying)
                    throw new InvalidOperationException("Cant access to property in edit-mode");
#endif

                _globallyEnabled = value;
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetGlobally()
        {
            _globally = new EasyTabIntegration();
            _globallyEnabled = true;
        }

        [RuntimeInitializeOnLoadMethod]
        private static void RuntimeInitializeOnLoadMethod()
        {
            try
            {
                InjectGloballyEasyTabIntegrationToPlayerLoop();
            }
            catch (Exception e)
            {
                Debug.LogError("Inject EasyTabIntegration globally failed");
                Debug.LogException(e);
            }
        }

        private static void RemoveGloballyEasyTabIntegrationFromPlayerLoop()
        {
            var playerLoop = PlayerLoop.GetCurrentPlayerLoop();

            var updateSystemType = typeof(UnityEngine.PlayerLoop.Update);
            var updateSystemIndex = playerLoop.GetSystemIndex(updateSystemType);
            var updateSystem = playerLoop.subSystemList[updateSystemIndex];
            var updatesSubSystems = updateSystem.subSystemList.ToList();

            updatesSubSystems.RemoveAll(e => e.type == typeof(EasyTabIntegration));
            updateSystem.subSystemList = updatesSubSystems.ToArray();
            playerLoop.subSystemList[updateSystemIndex] = updateSystem;

            PlayerLoop.SetPlayerLoop(playerLoop);
        }

        private static void InjectGloballyEasyTabIntegrationToPlayerLoop()
        {
#if UNITY_EDITOR // When closing the application (Unity Player), we can not waste time removing an element from the PlayerLoop
            Application.quitting += RemoveGloballyEasyTabIntegrationFromPlayerLoop;
#endif
            var playerLoop = PlayerLoop.GetCurrentPlayerLoop();

            var easyTabSystem = new PlayerLoopSystem
            {
                type = typeof(EasyTabIntegration),
                subSystemList = Array.Empty<PlayerLoopSystem>(),
                updateDelegate = DoUpdate
            };

            var updateSystemType = typeof(UnityEngine.PlayerLoop.Update);
            var updateSystemIndex = playerLoop.GetSystemIndex(updateSystemType);
            var updateSystem = playerLoop.subSystemList[updateSystemIndex];

            var scriptRunBehaviourUpdateType = typeof(UnityEngine.PlayerLoop.Update.ScriptRunBehaviourUpdate);
            var scripUpdateIndex = updateSystem.GetSystemIndex(scriptRunBehaviourUpdateType);
            var updatesSubSystems = updateSystem.subSystemList.ToList();
            updatesSubSystems.Insert(scripUpdateIndex, easyTabSystem);
            updateSystem.subSystemList = updatesSubSystems.ToArray();
            playerLoop.subSystemList[updateSystemIndex] = updateSystem;

            PlayerLoop.SetPlayerLoop(playerLoop);
        }

        private static void DoUpdate()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                return;
#endif

            if (!GloballyEnabled)
                return;

            var easyTabSupport = Globally;
            easyTabSupport.UpdateAll();
        }
    }
}