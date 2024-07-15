using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EasyTab
{
    public class EasyTabNodeDriver
    {
        public EasyTabConditions Conditions { private set; get; }
        public IEasyTabNodeDriver<Transform> TransformDriver { private set; get; }
        
        // todo: remove
        public IEasyTabNodeDriver<EasyTab> EasyTabDriver { private set; get; }
        public IEasyTabNodeDriver<Scene> SceneDriver {private set;  get; }
        public IEasyTabNodeDriver RootDriver {private set;  get; }
        
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
            EasyTabDriver = new EasyTabDriver(this);
            SceneDriver = new SceneDriver(this);
            RootDriver = new RootDriver(SceneDriver);
        }

        internal EasyTabNode CreateNode(Transform target)
        {
            if (target.TryGetComponent<EasyTab>(out var easyTabTarget))
                return EasyTabNode.ByDriver(easyTabTarget, EasyTabDriver);

            var targetNodeCandidate = EasyTabNode.ByDriver(target, TransformDriver);

            return targetNodeCandidate;
        }
    }
}