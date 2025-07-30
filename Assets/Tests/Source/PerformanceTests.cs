using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Profiling;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace EasyTab.Tests
{
    public class PerformanceTests
    {
        [UnityTest]
        public IEnumerator TimeSpentForTraversingBigTreeMustNotExceedQuota() => Lifetime.DefineAsync(async (lifetime) =>
        {
            lifetime.Add(new GameObject(string.Empty, typeof(EventSystem)));

            var easyTabSupport = new EasyTabIntegration();
            var buttonContainer = new GameObject("buttonContainer");
            var button = lifetime.Add(new GameObject("1", typeof(Button)));
            button.transform.SetParent(buttonContainer.transform);

            EventSystem.current.SetSelectedGameObject(button);
            Assert.AreEqual(button, EventSystem.current.currentSelectedGameObject);

            // buttonContainer.AddComponent<EasyTab>().BorderMode = BorderMode.Roll;
            
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 5000; j++)
                    lifetime.Add(new GameObject(string.Empty));
                
                await UniTask.NextFrame();
            }
            
            // cold call
            easyTabSupport.Navigate();
            Assert.AreEqual(button, EventSystem.current.currentSelectedGameObject);
            
            var timeBeforeNavigate = DateTime.UtcNow;
            easyTabSupport.Navigate();
            var timeInNavigation = DateTime.UtcNow - timeBeforeNavigate;
            
            Debug.Log($"time for navigation: {timeInNavigation.TotalMilliseconds:F4} ms");
            Assert.Less(timeInNavigation.TotalMilliseconds, 1000);
        });
    }
}