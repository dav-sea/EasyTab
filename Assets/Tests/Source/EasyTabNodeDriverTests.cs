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
            var solver = easyTabIntegration.Solver;
            solver.Driver = solver.Driver
                .WithTraversingChildrenBlocking(t => t.TryGetComponent(out Window window) && !window.IsActive);

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
            var solver = easyTabIntegration.Solver;
            solver.Driver = solver.Driver
                .WithSelectableBlocking(t => t.TryGetComponent<Graphic>(out var graphic) && graphic.color.a <= 0);


            EventSystem.current.SetSelectedGameObject(selectable1);
            Assert.AreEqual(selectable1, EventSystem.current.currentSelectedGameObject);
            easyTabIntegration.Navigate();
            Assert.AreEqual(selectable3, EventSystem.current.currentSelectedGameObject);
            easyTabIntegration.Navigate();
            Assert.AreEqual(selectable1, EventSystem.current.currentSelectedGameObject);
        });
        
        [UnityTest]
        public IEnumerator TestCase4() => Lifetime.DefineAsync(async (lifetime) =>
        {
            lifetime.Add(new GameObject("", typeof(EventSystem)));
            var window1 = lifetime.Add(new GameObject("1", typeof(Window)));
            var selectable1 = lifetime.Add(new GameObject("11", typeof(Button)));
            selectable1.transform.SetParent(window1.transform);
            var window2 = lifetime.Add(new GameObject("2", typeof(Window)));
            var selectable22 = lifetime.Add(new GameObject("22", typeof(Button)));
            var selectable23 = lifetime.Add(new GameObject("23", typeof(Button)));
            var selectable24 = lifetime.Add(new GameObject("24", typeof(Button)));
            selectable22.transform.SetParent(window2.transform);
            selectable23.transform.SetParent(window2.transform);
            selectable24.transform.SetParent(window2.transform);
            var window3 = lifetime.Add(new GameObject("3", typeof(Window)));
            var selectable3 = lifetime.Add(new GameObject("33", typeof(Button)));
            selectable3.transform.SetParent(window3.transform);
            
            var easyTabIntegration = new EasyTabIntegration();
            var solver = easyTabIntegration.Solver;
            solver.Driver = solver.Driver.DecorateBorderMode((@base, target) =>
            {
                // if target is window
                if (target.IsTransform(out var targetTransform) && targetTransform.TryGetComponent<Window>(out _))
                    return BorderMode.Roll;

                return @base.GetBorderMode(target);
            });

            EventSystem.current.SetSelectedGameObject(selectable22);
            Assert.AreEqual(selectable22, EventSystem.current.currentSelectedGameObject);
            easyTabIntegration.Navigate();
            Assert.AreEqual(selectable23, EventSystem.current.currentSelectedGameObject);
            easyTabIntegration.Navigate();
            Assert.AreEqual(selectable24, EventSystem.current.currentSelectedGameObject);
            easyTabIntegration.Navigate();
            Assert.AreEqual(selectable22, EventSystem.current.currentSelectedGameObject);
        });

        class Window : MonoBehaviour
        {
            public bool IsActive;
        }
    }
}