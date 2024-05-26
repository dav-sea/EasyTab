using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace EasyTab.Tests
{
    public class TestsWithMultipleScenes
    {
        [UnityTest]
        public IEnumerator NavigateShouldBeWorkWithTwoLoadedScenes() => Lifetime.DefineAsync(async (lifetime) =>
        {
            lifetime.Add(new GameObject("", typeof(EventSystem)));

            var firstScene = SceneManager.CreateScene("1");
            var secondScene = SceneManager.CreateScene("2");

            try
            {
                var firstGo = lifetime.Add(new GameObject("1", typeof(Button)));
                var secondGo = lifetime.Add(new GameObject("2", typeof(Button)));

                SceneManager.MoveGameObjectToScene(firstGo, firstScene);
                SceneManager.MoveGameObjectToScene(secondGo, secondScene);

                Assert.AreEqual(firstGo.scene, firstScene);
                Assert.AreEqual(secondGo.scene, secondScene);

                EventSystem.current.SetSelectedGameObject(firstGo);

                EasyTabIntegration.Globally.Navigate();

                Assert.AreEqual(secondGo, EventSystem.current.currentSelectedGameObject);
            }
            finally
            {
                await SceneManager.UnloadSceneAsync(firstScene).ToUniTask();
                await SceneManager.UnloadSceneAsync(secondScene).ToUniTask();
            }
        });
    }

    public class TestsWithEnterKey
    {
        [Test]
        public void EnterPressNotShouldNavigateTestCase1() => Lifetime.Define(lifetime =>
        {
            lifetime.Add(new GameObject("", typeof(EventSystem)));
            lifetime.Add(new GameObject("1", typeof(Button)));
            lifetime.Add(new GameObject("2", typeof(Button)));

            Assert.AreEqual(null, EventSystem.current.currentSelectedGameObject);
            var easyTabSupport = EasyTabIntegration.Globally;
            easyTabSupport.Navigate(isEnter: true);
            Assert.AreEqual(null, EventSystem.current.currentSelectedGameObject);
        });

        [Test]
        public void EnterPressNotShouldNavigateTestCase2() => Lifetime.Define(lifetime =>
        {
            lifetime.Add(new GameObject("", typeof(EventSystem)));
            var selectable1 = lifetime.Add(new GameObject("1", typeof(Button)));
            lifetime.Add(new GameObject("2", typeof(Button)));

            var easyTabSupport = new EasyTabIntegration();
            Assert.AreEqual(null, EventSystem.current.currentSelectedGameObject);
            EventSystem.current.SetSelectedGameObject(selectable1);
            Assert.AreEqual(selectable1, EventSystem.current.currentSelectedGameObject);
            easyTabSupport.Navigate(isEnter: true);
            Assert.AreEqual(selectable1, EventSystem.current.currentSelectedGameObject);
        });

        [Test]
        public void EnterPressNotShouldNavigateTestCase3() => Lifetime.Define(lifetime =>
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
            easyTabSupport.Navigate(isEnter: true);
            Assert.AreEqual(null, EventSystem.current.currentSelectedGameObject);
        });
    }
}