using System;
using JetBrains.Annotations;
using UnityEngine.SceneManagement;

namespace EasyTab
{
    public sealed class RootDriver : IEasyTabNodeDriver
    {
        private readonly IEasyTabNodeDriver _sceneDriver;

        public RootDriver([NotNull] IEasyTabNodeDriver sceneDriver)
        {
            _sceneDriver = sceneDriver ?? throw new ArgumentNullException(nameof(sceneDriver));
        }

        public BorderMode GetBorderMode(TransformOrScene _)
        {
            return BorderMode.Roll;
        }

        public EasyTabNode GetParent(TransformOrScene _)
        {
            return EasyTabNode.None;
        }

        public EasyTabNode GetChild(TransformOrScene _, int childNumber)
        {
            var scene = SceneManager.GetSceneAt(childNumber);
            return new EasyTabNode(scene, _sceneDriver);
        }

        public int GetChildrenCount(TransformOrScene _)
        {
            return SceneManager.sceneCount;
        }

        public bool IsSelectable(TransformOrScene _)
        {
            return false;
        }
    }
}