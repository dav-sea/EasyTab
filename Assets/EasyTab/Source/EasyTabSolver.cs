using System;
using System.Collections.Generic;
using EasyTab.Internals;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace EasyTab
{
    /// <summary>
    /// The solver's task is to determine the next selectable object relative to the game object.
    /// </summary>
    public sealed class EasyTabSolver : IEasyTabDriverProvider
    {
        public FallbackNavigationPolicy WhenCurrentIsNotSet { set; get; } =
            FallbackNavigationPolicy.AllowNavigateToLastSelected | FallbackNavigationPolicy.AllowNavigateToEntryPoint;

        public FallbackNavigationPolicy WhenCurrentIsNotSelectable { set; get; } = FallbackNavigationPolicy.AllowAll;

        public GameObject EntryPoint { set; get; }
        public GameObject LastSelected { set; get; }

        /// <summary>
        /// A collection of visited objects during jumps. 
        /// It is needed to detect recursive jumps and prevent an uncaught StackOverflowException and application crash
        /// </summary>
        private readonly HashSet<GameObject> _visitedGameObjects = new HashSet<GameObject>();
        private readonly EasyTabHierarchySolver _hierarchySolver = new EasyTabHierarchySolver();
        private IEasyTabDriver _driver;
        
        
        public EasyTabSolver() => _driver = new EasyTabDriver(this);
        
        [NotNull]
        public IEasyTabDriver Driver
        {
            get => _driver;
            set => _driver = value ?? throw new ArgumentNullException(nameof(value));
        }

        public GameObject GetNext(GameObject current, bool reverse, bool isEnterKey = false)
        {
            try
            {
                var next = GetNextImpl(current, reverse, isEnterKey);
                return next.Target.IsTransform(out var nextTransform) ? nextTransform.gameObject : null;
            }
            finally // ..but if a real StackOverflowException is thrown, the application will crash
            {
                _visitedGameObjects.Clear();
            }
        }
        
        public GameObject GetNextWithoutPolicy(GameObject current, bool reverse)
        {
            try
            {
                var currentNode = this.CreateNode(current.transform);
                var next = GetNextWithoutPolicyImpl(currentNode, reverse);
                return next.Target.IsTransform(out var nextTransform) ? nextTransform.gameObject : null;
            }
            finally // ..but if a real StackOverflowException is thrown, the application will crash
            {
                _visitedGameObjects.Clear();
            }
        }
        
        bool IsCorrectFallback(GameObject fallback)
        {
            if (!fallback)
                return false;

            var fallbackTransform = fallback.transform;
            if (!TryGetValidParent(fallbackTransform, out var result)
                || fallbackTransform.parent != result)
                return false;

            var fallbackNode = this.CreateNode(fallbackTransform);
            return fallbackNode.IsSelectable;
        }

        private EasyTabNode GetNextWithoutPolicyImpl(EasyTabNode currentNode, bool reverse = false)
        {
            if (!NeedUseJumping(currentNode, out var policy, out var nextJump, out var reverseJump))
                return GetNextByHierarchy(currentNode, reverse);

            var jumpTarget = reverse ? reverseJump : nextJump;
            var jumpTargetNode = jumpTarget ? this.CreateNode(jumpTarget.transform) : EasyTabNode.None;

            if (!jumpTargetNode.IsNone && jumpTargetNode.IsSelectable)
                return jumpTargetNode;

            if (policy == JumpingPolicy.UseOnlyJumps)
                return EasyTabNode.None;

            if (policy == JumpingPolicy.UseJumpsOrTheirNext)
            {
                if (jumpTargetNode.IsNone)
                    return EasyTabNode.None;
                
                if (!_visitedGameObjects.Add(jumpTarget))
                {
                    // test-case: UseJumpsOrTheirNextTestCase5
                    Debug.LogError("Cyclic link detected in jumps");
                    return EasyTabNode.None;
                }

                return GetNextWithoutPolicyImpl(jumpTargetNode, reverse);
            }

            if (policy == JumpingPolicy.UseJumpsOrHierarchy)
                return GetNextByHierarchy(currentNode, reverse);
            
            throw new NotImplementedException($"Unsupported JumpingPolicy {policy}");
        }

        private EasyTabNode GetFallbackWhenCurrentIsNotSet()
        {
            var policy = WhenCurrentIsNotSet;

            if (policy.Has(FallbackNavigationPolicy.AllowNavigateToLastSelected) && IsCorrectFallback(LastSelected))
                return this.CreateNode(LastSelected.transform);
            if (policy.Has(FallbackNavigationPolicy.AllowNavigateToEntryPoint) && IsCorrectFallback(EntryPoint))
                return this.CreateNode(EntryPoint.transform);

            return EasyTabNode.None;
        }

        private EasyTabNode GetFallbackWhenCurrentIsNotSelectable(EasyTabNode currentNode, bool reverse)
        {
            var policy = WhenCurrentIsNotSelectable;

            if (policy.Has(FallbackNavigationPolicy.AllowNavigateToLastSelected) && IsCorrectFallback(LastSelected))
                return this.CreateNode(LastSelected.transform);

            if (policy.Has(FallbackNavigationPolicy.AllowNavigateToClosest))
            {
                var next = GetNextByHierarchy(currentNode, reverse);
                if (!next.IsNone)
                    return next;
            }

            if (policy.Has(FallbackNavigationPolicy.AllowNavigateToEntryPoint) && IsCorrectFallback(EntryPoint))
                return this.CreateNode(EntryPoint.transform);

            return EasyTabNode.None;
        }
        
        private EasyTabNode GetNextImpl(GameObject current, bool reverse = false, bool isEnterKey = false)
        {
            if (!current)
                return isEnterKey ? EasyTabNode.None : GetFallbackWhenCurrentIsNotSet();

            ExtractNavigationOptions(current, out var needLock, out var needHandleEnter);

            if (isEnterKey && !needHandleEnter)
                return EasyTabNode.None;

            if (needLock)
                return EasyTabNode.None;

            var currentNode = FindFirstNodeWithValidParent(current.transform);

            if (currentNode.IsNone)
            {
                Debug.LogError($"Internal error. Report the problem using the link: {Utils.LinkToReportIssues}");
                return EasyTabNode.None;
            }

            var next2 = GetNextWithoutPolicyImpl(currentNode, reverse);
            if (next2.Target.IsTransform)
                return next2;

            // is jumping continuation in same case...
            if (currentNode.IsSelectable)
                return EasyTabNode.None;

            return GetFallbackWhenCurrentIsNotSelectable(currentNode, reverse);
        }
        
        private EasyTabNode FindFirstNodeWithValidParent(Transform target)
        {
            if (!TryGetValidParent(target, out var result))
                target = result;

            return this.CreateNode(target);
        }

        private bool TryGetValidParent(Transform current, out Transform bestParent)
        {
            bestParent = null;

            var parent = current.parent;
            if (!parent)
                // if parent is null then is 'current' is one of root transform
                return true;

            // This is a recursion that goes up to the roots
            if (TryGetValidParent(parent, out var upperBestParent))
            {
                var parentNode = this.CreateNode(parent);
                if (parentNode.ChildrenCount == 0)
                {
                    bestParent = parent;
                    return false;
                }

                bestParent = parent;
                return true;
            }

            bestParent = upperBestParent;
            return false;
        }

        private void ExtractNavigationOptions(GameObject target, out bool needLock, out bool needHandleEnter)
        {
            needLock = false;
            needHandleEnter = false;

#if TMP_PRESENT // see TMP_PRESENT in EasyTab asmdef
            if (target.TryGetComponent<TMPro.TMP_InputField>(out var tmpInputField))
            {
                needLock = tmpInputField.multiLine && tmpInputField.isFocused && !tmpInputField.readOnly;
                needHandleEnter = tmpInputField.lineType != TMPro.TMP_InputField.LineType.MultiLineNewline;
            }
#endif

            if (target.TryGetComponent<InputField>(out var inputField))
            {
                needLock = inputField.multiLine && inputField.isFocused && !inputField.readOnly;
                needHandleEnter = inputField.lineType != InputField.LineType.MultiLineNewline;
            }

            if (target.TryGetComponent<EasyTab>(out var easyTab))
            {
                needLock = easyTab.NavigationLock == NavigationLock.Auto
                    ? needLock
                    : easyTab.NavigationLock == NavigationLock.Lock;

                needHandleEnter = easyTab.EnterHandling == EnterHandling.Auto
                    ? needHandleEnter
                    : easyTab.EnterHandling == EnterHandling.NavigateNext;
            }
        }
        
        private bool NeedUseJumping(EasyTabNode node, out JumpingPolicy jumpingPolicy, out GameObject next,
            out GameObject reverse)
        {
            if (!node.Target.IsTransform)
                throw new InvalidOperationException("Cant get jumps from not a transform");

            var nodeTransform = node.Target.AsTransform;
            
            if (!nodeTransform.TryGetComponent<EasyTab>(out var easyTab))
            {
                jumpingPolicy = default;
                next = reverse = default;
                return false;
            }

            next = easyTab.NextJump;
            reverse = easyTab.ReverseJump;
            jumpingPolicy = easyTab.JumpingPolicy;
            return jumpingPolicy != JumpingPolicy.DontUseJumps;
        }

        private EasyTabNode GetNextByHierarchy(EasyTabNode currentNode, bool reverse) 
            => _hierarchySolver.FindNext(currentNode, reverse);

        IEasyTabDriver IEasyTabDriverProvider.GetDriver() => Driver;
    }
}