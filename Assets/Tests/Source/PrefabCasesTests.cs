using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace EasyTab.Tests
{
    public class PrefabCasesTests
    {
        private const float DelayInSecondsBetweenTabs = 0.01f;

        [UnityTest]
        [TestCaseSource(nameof(TestSource))]
        public IEnumerator TestCase(string prefabName, bool reverse) => Lifetime.DefineAsync(async (lifetime) =>
        {
            var testCase = EasyTabTestCasePrefab.Instantiate(prefabName);
            lifetime.Add(testCase.gameObject);
            var easyTabSupport = new EasyTabIntegration();
            easyTabSupport.Solver.WhenCurrentIsNotSelectable = testCase.WhenCurrentIsNotSelectable;
            easyTabSupport.Solver.WhenCurrentIsNotSet = testCase.WhenCurrentIsNotSet;

            var seq = reverse
                ? testCase.ExcpectSelectableSequence.AsEnumerable().Reverse()
                : testCase.ExcpectSelectableSequence;

            if (testCase.NavigateFirst)
            {
                EventSystem.current.SetSelectedGameObject(testCase.FirstSelectable.gameObject);
                easyTabSupport.UpdateLastSelected();
                easyTabSupport.Navigate(reverse);
                await UniTask.Delay(TimeSpan.FromSeconds(DelayInSecondsBetweenTabs));
            }
            else
            {
                easyTabSupport.UpdateLastSelected();
                EventSystem.current.SetSelectedGameObject(seq.First().gameObject);
            }

            foreach (var selectable in seq)
            {
                var current = EventSystem.current.currentSelectedGameObject;
                Assert.AreEqual(current, selectable.gameObject);
                easyTabSupport.Navigate(reverse);
                await UniTask.Delay(TimeSpan.FromSeconds(DelayInSecondsBetweenTabs));
            }
        });


        [Test]
        public void TestTwoRootBranches()
        {
            using (var context = new Lifetime())
            {
                context.Add(new GameObject("", typeof(EventSystem)));
                var selectable1Go = context.Add(new GameObject("1", typeof(Button)));
                var selectable2Go = context.Add(new GameObject("2", typeof(Button)));
                
                var easyTabSupport = new EasyTabIntegration();

                selectable1Go.GetComponent<Selectable>().Select();
                Assert.AreEqual(selectable1Go, EventSystem.current.currentSelectedGameObject);
                easyTabSupport.Navigate();
                Assert.AreEqual(selectable2Go, EventSystem.current.currentSelectedGameObject);
                easyTabSupport.Navigate();
                Assert.AreEqual(selectable1Go, EventSystem.current.currentSelectedGameObject);
            }
        }


        private static IEnumerable TestSource
        {
            get
            {
                yield return new TestCaseData("Common", false).Returns(null);
                yield return new TestCaseData("Common", true).Returns(null);
                yield return new TestCaseData("Case1", false).Returns(null);
                yield return new TestCaseData("Case1", true).Returns(null);
                yield return new TestCaseData("ClampTestCase1", false).Returns(null);
                yield return new TestCaseData("ClampTestCase1", true).Returns(null);
                yield return new TestCaseData("ExcludeChildrenCase1", true).Returns(null);
                yield return new TestCaseData("ExcludeChildrenCase1", false).Returns(null);
                yield return new TestCaseData("ExcludeChildrenCase2", false).Returns(null);
                yield return new TestCaseData("ExcludeChildrenCase2", true).Returns(null);
                yield return new TestCaseData("ExcludeChildrenCase3", false).Returns(null);
                yield return new TestCaseData("ExcludeChildrenCase3", true).Returns(null);
                yield return new TestCaseData("NavigationLockOnMultilineInputFieldCase1", false).Returns(null);
                yield return new TestCaseData("NavigationLockCase1", false).Returns(null);
                yield return new TestCaseData("NavigationForceUnlockOnMultilineInputFieldCase1", false).Returns(null);
                yield return new TestCaseData("NavigationForceUnlockOnMultilineInputFieldCase1", true).Returns(null);
                yield return new TestCaseData("DisabledTransformShouldBeIgnored", true).Returns(null);
                yield return new TestCaseData("DisabledTransformShouldBeIgnored", false).Returns(null);
                yield return new TestCaseData("DisabledSelectableComponentShouldBeIgnored", false).Returns(null);
                yield return new TestCaseData("DisabledSelectableComponentShouldBeIgnored", true).Returns(null);
                yield return new TestCaseData("NotInteractableComponentShouldBeIgnored", true).Returns(null);
                yield return new TestCaseData("NotInteractableComponentShouldBeIgnored", false).Returns(null);
                yield return new TestCaseData("CanvasGroupTestCase2", false).Returns(null);
                yield return new TestCaseData("CanvasGroupTestCase2", true).Returns(null);
                yield return new TestCaseData("CanvasGroupTestCase1", true).Returns(null);
                yield return new TestCaseData("CanvasGroupTestCase4", true).Returns(null);
                yield return new TestCaseData("CanvasGroupTestCase4", false).Returns(null);
                yield return new TestCaseData("ReverseWithChildrenCase", false).Returns(null);
                yield return new TestCaseData("ReverseWithChildrenCase", true).Returns(null);
                yield return new TestCaseData("SimpleJumpingTestCase", true).Returns(null);
                yield return new TestCaseData("SimpleJumpingTestCase", false).Returns(null);
                yield return new TestCaseData("UseOnlyJumpsWithMissingNext", false).Returns(null);
                yield return new TestCaseData("UseJumpsOrSelfChildrenTestCase", false).Returns(null);
                yield return new TestCaseData("UseJumpsOrTheirNextTestCase1", false).Returns(null);
                yield return new TestCaseData("UseJumpsOrTheirNextTestCase2", false).Returns(null);
                yield return new TestCaseData("UseJumpsOrTheirNextTestCase3", false).Returns(null);
                yield return new TestCaseData("UseJumpsOrTheirNextTestCase4", false).Returns(null);
                yield return new TestCaseData("UseJumpsOrTheirNextTestCase5", false).Returns(null);
                yield return new TestCaseData("NavigationLockOnMultilineTMPInputFieldCase1", false).Returns(null);
                yield return new TestCaseData("NavigationForceUnlockOnMultilineTMPInputFieldCase1", true).Returns(null);
                yield return new TestCaseData("NavigationForceUnlockOnMultilineTMPInputFieldCase1", false).Returns(null);
            }
        }
    }
}