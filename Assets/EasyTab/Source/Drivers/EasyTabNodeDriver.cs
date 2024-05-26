using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EasyTab
{
    public class EasyTabNodeDriver
    {
        public IEasyTabNodeDriver<Transform> TransformDriver { private set; get; }
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
            TransformDriver = new TransformDriver(this);
            EasyTabDriver = new EasyTabDriver(this);
            SceneDriver = new SceneDriver(this);
            RootDriver = new RootDriver(SceneDriver);
        }

        public bool ParentsIsCorrect(Transform target) 
            => TryGetValidParent(target, out var result) && target.parent == result;

        public EasyTabNode FindStartNodeRelativeBy(Transform target)
        {
            if (!TryGetValidParent(target, out var result)) 
                target = result;

            if (target.TryGetComponent<EasyTab>(out var easyTabTarget))
                return EasyTabNode.ByDriver(easyTabTarget, EasyTabDriver);

            var targetNodeCandidate = EasyTabNode.ByDriver(target, TransformDriver);

            return targetNodeCandidate;
        }

        public EasyTabNode CreateNode(Transform target)
        {
            if (target.TryGetComponent<EasyTab>(out var easyTabTarget))
                return EasyTabNode.ByDriver(easyTabTarget, EasyTabDriver);

            var targetNodeCandidate = EasyTabNode.ByDriver(target, TransformDriver);

            return targetNodeCandidate;
        }
        
        private bool TryGetValidParent(Transform current, out Transform resultParent)
        {
            resultParent = null;
            
            var parent = current.parent;
            if (!parent)
                return true;
            
            if (TryGetValidParent(parent, out var upperParent))
            {
                var parentNode = CreateNode(parent);
                if (parentNode.ChildrenCount == 0)
                {
                    resultParent = parent;
                    return false;
                }

                resultParent = parent;
                return true;
            }

            resultParent = upperParent;
            return false;
        }
    }
}