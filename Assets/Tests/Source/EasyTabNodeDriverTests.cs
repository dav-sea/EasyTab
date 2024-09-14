using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace EasyTab.Tests
{
    public class EasyTabNodeDriverTests
    {
        [UnityTest]
        public IEnumerator TestCase2() => Lifetime.DefineAsync(async (lifetime) =>
        {
            lifetime.Add(new GameObject("", typeof(EventSystem)));
            var window1 = lifetime.Add(new GameObject("1", typeof(Window)));
            var selectable1 = lifetime.Add(new GameObject("11", typeof(Button)));
            selectable1.transform.SetParent(window1.transform);
            var window2 = lifetime.Add(new GameObject("2", typeof(Window)));
            var selectable2 = lifetime.Add(new GameObject("22", typeof(Button)));
            selectable2.transform.SetParent(window2.transform);
            var window3 = lifetime.Add(new GameObject("3", typeof(Window)));
            var selectable3 = lifetime.Add(new GameObject("33", typeof(Button)));
            selectable3.transform.SetParent(window3.transform);

            window1.GetComponent<Window>().IsActive = true;
            window2.GetComponent<Window>().IsActive = false;
            window3.GetComponent<Window>().IsActive = true;

            var easyTabIntegration = new EasyTabIntegration();
            var conditions = easyTabIntegration.Solver.Drivers.Conditions.ConditionsForTraversingChildren;

            conditions.Add(t => !t.TryGetComponent<Window>(out var window) || window.IsActive);

            EventSystem.current.SetSelectedGameObject(selectable1);
            Assert.AreEqual(selectable1, EventSystem.current.currentSelectedGameObject);
            easyTabIntegration.Navigate();
            Assert.AreEqual(selectable3, EventSystem.current.currentSelectedGameObject);
            easyTabIntegration.Navigate();
            Assert.AreEqual(selectable1, EventSystem.current.currentSelectedGameObject);
        });

        [UnityTest]
        public IEnumerator TestCase3() => Lifetime.DefineAsync(async (lifetime) =>
        {
            lifetime.Add(new GameObject("", typeof(EventSystem)));

            var selectable1 = lifetime.Add(new GameObject("1", typeof(Button)));
            var selectable2 = lifetime.Add(new GameObject("2", typeof(Button), typeof(Image)));
            var selectable3 = lifetime.Add(new GameObject("3", typeof(Button), typeof(Image)));

            selectable2.GetComponent<Graphic>().color = Color.clear;

            var easyTabIntegration = new EasyTabIntegration();
            var conditions = easyTabIntegration.Solver.Drivers.Conditions.ConditionsForSelectable;
            conditions.Add(t => !t.TryGetComponent<Graphic>(out var graphic) || graphic.color.a > 0);

            EventSystem.current.SetSelectedGameObject(selectable1);
            Assert.AreEqual(selectable1, EventSystem.current.currentSelectedGameObject);
            easyTabIntegration.Navigate();
            Assert.AreEqual(selectable3, EventSystem.current.currentSelectedGameObject);
            easyTabIntegration.Navigate();
            Assert.AreEqual(selectable1, EventSystem.current.currentSelectedGameObject);
        });

        class Window : MonoBehaviour
        {
            public bool IsActive;
        }
    }
}