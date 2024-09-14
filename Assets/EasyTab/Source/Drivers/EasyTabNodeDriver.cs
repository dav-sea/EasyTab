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
        public IEasyTabNodeDriver TransformDriver { private set; get; }
        internal IEasyTabNodeDriver SceneDriver {private set;  get; }
        internal IEasyTabNodeDriver RootDriver {private set;  get; }

        public EasyTabNodeDriver()
        {
            Conditions = new EasyTabConditions();
            TransformDriver = new TransformDriver(this);
            SceneDriver = new SceneDriver(this);
            RootDriver = new RootDriver(SceneDriver);
        }

        internal EasyTabNode CreateNode(Transform transform)
        {
            var targetNodeCandidate = new EasyTabNode(transform, TransformDriver);
            return targetNodeCandidate;
        }
    }
}