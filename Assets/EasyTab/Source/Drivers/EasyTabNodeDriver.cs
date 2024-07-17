using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EasyTab
{
    [PublicAPI]
    public sealed class EasyTabNodeDriver
    {
        [PublicAPI]
        public EasyTabConditions Conditions { private set; get; }
        [PublicAPI]
        public IEasyTabNodeDriver<Transform> TransformDriver { private set; get; }
        internal IEasyTabNodeDriver<Scene> SceneDriver {private set;  get; }
        internal IEasyTabNodeDriver RootDriver {private set;  get; }
        
        [PublicAPI, Obsolete("Use Conditions for limiting of traversing and selectable")]
        public void SetTransformDriver([NotNull] IEasyTabNodeDriver<Transform> transformDriver)
        {
            if (transformDriver == null)
                throw new ArgumentNullException(nameof(transformDriver));

            TransformDriver = transformDriver;
        }

        public EasyTabNodeDriver()
        {
            Conditions = new EasyTabConditions();
            TransformDriver = new TransformDriver(this);
            SceneDriver = new SceneDriver(this);
            RootDriver = new RootDriver(SceneDriver);
        }

        internal EasyTabNode CreateNode(Transform target)
        {
            var targetNodeCandidate = EasyTabNode.ByDriver(target, TransformDriver);

            return targetNodeCandidate;
        }
    }
}