using System;
using JetBrains.Annotations;
using UnityEngine.SceneManagement;

namespace EasyTab
{
    public sealed class RootDriver : IEasyTabNodeDriver
    {
        private readonly IEasyTabNodeDriver<Scene> _sceneDriver;

        public RootDriver([NotNull] IEasyTabNodeDriver<Scene> sceneDriver)
        {
            _sceneDriver = sceneDriver ?? throw new ArgumentNullException(nameof(sceneDriver));
        }

        public BorderMode GetBorderMode(object _)
        {
            return BorderMode.Roll;
        }

        public EasyTabNode GetParent(object _)
        {
            return EasyTabNode.None;
        }

        public EasyTabNode GetChild(object _, int childNumber)
        {
            var scene = SceneManager.GetSceneAt(childNumber);
            
            return EasyTabNode.ByDriver(scene, _sceneDriver);
        }

        public int GetChildrenCount(object _)
        {
            return SceneManager.sceneCount;
        }

        public bool IsSelectable(object _)
        {
            return false;
        }
    }
}