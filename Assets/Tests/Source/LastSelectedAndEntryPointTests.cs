using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace EasyTab.Tests
{
    public class LastSelectedAndEntryPointTests
    {
        [Test]
        public void IgnoreSelectableShouldBeUsedInLastSelectedFallbackTestCase() => Lifetime.Define((lifetime) =>
        {
            lifetime.Add(new GameObject("", typeof(EventSystem)));

            var easyTabSupport = new EasyTabIntegration();
            var firstButton = lifetime.Add(new GameObject("1", typeof(Button)));
            lifetime.Add(new GameObject("2", typeof(Button)));

            EventSystem.current.SetSelectedGameObject(firstButton);
            Assert.AreEqual(firstButton, EventSystem.current.currentSelectedGameObject);

            easyTabSupport.UpdateLastSelected();
            EventSystem.current.SetSelectedGameObject(null);

            easyTabSupport.Navigate();
            Assert.AreEqual(firstButton, EventSystem.current.currentSelectedGameObject);
            EventSystem.current.SetSelectedGameObject(null);

            firstButton.AddComponent<EasyTab>().SelectableRecognition = SelectableRecognition.AsNotSelectable;
            easyTabSupport.Navigate();

            Assert.AreEqual(null, EventSystem.current.currentSelectedGameObject);
        });

        [Test]
        public void ExcludeChildrenShouldBeUsedInFallbackTestCase() => Lifetime.Define((lifetime) =>
        {
            lifetime.Add(new GameObject("", typeof(EventSystem)));

            var easyTabSupport = new EasyTabIntegration();
            var parentForFirstButton = lifetime.Add(new GameObject("parent"));
            var firstButton = lifetime.Add(new GameObject("1", typeof(Button)));
            firstButton.transform.SetParent(parentForFirstButton.transform);

            lifetime.Add(new GameObject("2", typeof(Button)));

            EventSystem.current.SetSelectedGameObject(firstButton);
            Assert.AreEqual(firstButton, EventSystem.current.currentSelectedGameObject);
            easyTabSupport.UpdateLastSelected();

            EventSystem.current.SetSelectedGameObject(null);
            easyTabSupport.Navigate();
            Assert.AreEqual(firstButton, EventSystem.current.currentSelectedGameObject);

            EventSystem.current.SetSelectedGameObject(null);
            parentForFirstButton.AddComponent<EasyTab>().ChildrenExtracting = ChildrenExtracting.WithoutChildren;
            easyTabSupport.Navigate();
            Assert.AreEqual(null, EventSystem.current.currentSelectedGameObject);
        });

        [Test]
        public void LastSelectedIsEasyTabTestCase() => Lifetime.Define((lifetime) =>
        {
            lifetime.Add(new GameObject("", typeof(EventSystem)));

            var easyTabSupport = new EasyTabIntegration();
            var button = lifetime.Add(new GameObject("1", typeof(Button), typeof(EasyTab)));

            EventSystem.current.SetSelectedGameObject(button);
            Assert.AreEqual(button, EventSystem.current.currentSelectedGameObject);
            easyTabSupport.UpdateLastSelected();
            EventSystem.current.SetSelectedGameObject(null);

            easyTabSupport.Navigate();
            Assert.AreEqual(button, EventSystem.current.currentSelectedGameObject);
        });

        [Test]
        public void LastSelectedShouldBeUseTestCase1() => Lifetime.Define((lifetime) =>
        {
            lifetime.Add(new GameObject("", typeof(EventSystem)));
            var selectable1 = lifetime.Add(new GameObject("1", typeof(Button)));
            lifetime.Add(new GameObject("2", typeof(Button)));

            var easyTabSupport = new EasyTabIntegration();
            Assert.AreEqual(null, EventSystem.current.currentSelectedGameObject);
            EventSystem.current.SetSelectedGameObject(selectable1);
            Assert.AreEqual(selectable1, EventSystem.current.currentSelectedGameObject);
            easyTabSupport.UpdateLastSelected();
            EventSystem.current.SetSelectedGameObject(null);
            Assert.AreEqual(null, EventSystem.current.currentSelectedGameObject);
            easyTabSupport.Navigate();
            Assert.AreEqual(selectable1, EventSystem.current.currentSelectedGameObject);
        });

        [Test]
        public void LastSelectedShouldNotBeUsedTestCase1() => Lifetime.Define((lifetime) =>
        {
            lifetime.Add(new GameObject("", typeof(EventSystem)));
            var selectable1 = lifetime.Add(new GameObject("1", typeof(Button)));
            lifetime.Add(new GameObject("2", typeof(Button)));

            var easyTabSupport = new EasyTabIntegration();
            easyTabSupport.Solver.WhenCurrentIsNotSet = FallbackNavigationPolicy.Nothing;

            Assert.AreEqual(null, EventSystem.current.currentSelectedGameObject);
            EventSystem.current.SetSelectedGameObject(selectable1);
            Assert.AreEqual(selectable1, EventSystem.current.currentSelectedGameObject);
            easyTabSupport.UpdateLastSelected();
            EventSystem.current.SetSelectedGameObject(null);
            Assert.AreEqual(null, EventSystem.current.currentSelectedGameObject);
            easyTabSupport.Navigate();
            Assert.AreEqual(null, EventSystem.current.currentSelectedGameObject);
        });

        [Test]
        public void EntryPointShouldNotBeUsedTestCase1() => Lifetime.Define((lifetime) =>
        {
            lifetime.Add(new GameObject("", typeof(EventSystem)));
            var selectable1 = lifetime.Add(new GameObject("1", typeof(Button)));
            lifetime.Add(new GameObject("2", typeof(Button)));

            var easyTabSupport = new EasyTabIntegration();
            easyTabSupport.Solver.WhenCurrentIsNotSet = FallbackNavigationPolicy.Nothing;

            Assert.AreEqual(null, EventSystem.current.currentSelectedGameObject);
            EventSystem.current.SetSelectedGameObject(selectable1);
            Assert.AreEqual(selectable1, EventSystem.current.currentSelectedGameObject);
            easyTabSupport.UpdateLastSelected();
            EventSystem.current.SetSelectedGameObject(null);
            Assert.AreEqual(null, EventSystem.current.currentSelectedGameObject);
            easyTabSupport.Navigate();
            Assert.AreEqual(null, EventSystem.current.currentSelectedGameObject);
        });

        [Test]
        public void EntryPointShouldBeUsedTestCase1() => Lifetime.Define(lifetime =>
        {
            lifetime.Add(new GameObject("", typeof(EventSystem)));
            var selectable1 = lifetime.Add(new GameObject("1", typeof(Button)));
            var selectable2 = lifetime.Add(new GameObject("2", typeof(Button)));

            var easyTabSupport = new EasyTabIntegration();
            easyTabSupport.Solver.WhenCurrentIsNotSet = FallbackNavigationPolicy.AllowNavigateToEntryPoint;
            easyTabSupport.Solver.EntryPoint = selectable2;

            Assert.AreEqual(null, EventSystem.current.currentSelectedGameObject);
            EventSystem.current.SetSelectedGameObject(selectable1);
            Assert.AreEqual(selectable1, EventSystem.current.currentSelectedGameObject);
            easyTabSupport.UpdateLastSelected();
            EventSystem.current.SetSelectedGameObject(null);
            Assert.AreEqual(null, EventSystem.current.currentSelectedGameObject);
            easyTabSupport.Navigate();
            Assert.AreEqual(selectable2, EventSystem.current.currentSelectedGameObject);
        });
    }
}