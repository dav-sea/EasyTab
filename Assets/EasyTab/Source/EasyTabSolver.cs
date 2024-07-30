using System.Collections.Generic;
using EasyTab.Internals;
using UnityEngine;
using UnityEngine.UI;

namespace EasyTab
{
    /// <summary>
    /// The solver's task is to determine the next selectable object relative to the game object.
    /// </summary>
    public sealed class EasyTabSolver
    {
        public FallbackNavigationPolicy WhenCurrentIsNotSet { set; get; } =
            FallbackNavigationPolicy.AllowNavigateToLastSelected | FallbackNavigationPolicy.AllowNavigateToEntryPoint;

        public FallbackNavigationPolicy WhenCurrentIsNotSelectable { set; get; } = FallbackNavigationPolicy.AllowAll;

        public GameObject EntryPoint { set; get; }
        public GameObject LastSelected { set; get; }

        public readonly EasyTabNodeDriver Drivers = new EasyTabNodeDriver();
        private readonly EasyTabNodeSolver _easyTabNodeSolver = new EasyTabNodeSolver();

        /// <summary>
        /// A collection of visited objects during jumps. 
        /// It is needed to detect recursive jumps and prevent an uncaught StackOverflowException and application crash
        /// </summary>
        private HashSet<GameObject> _visitedGameObjects = new HashSet<GameObject>();

        public GameObject GetNext(GameObject current, bool reverse = false, bool isEnterKey = false)
        {
            try
            {
                return GetNextImpl(current, reverse, isEnterKey);
            }
            finally // ..but if a real StackOverflowException is thrown, the application will crash
            {
                _visitedGameObjects.Clear();
            }
        }

        private GameObject GetNextImpl(GameObject current, bool reverse = false, bool isEnterKey = false)
        {
            bool IsCorrectFallback(GameObject fallback)
            {
                if (!fallback)
                    return false;

                if (!ParentsIsCorrect(fallback.transform))
                    return false;

                var fallbackNode = FindStartNodeRelativeBy(fallback.transform);
                if (!fallbackNode.IsSelectable)
                    return false;

                return true;
            }

            if (!current)
            {
                if (isEnterKey)
                    return null;

                var policy = WhenCurrentIsNotSet;

                if (policy.Has(FallbackNavigationPolicy.AllowNavigateToLastSelected) && IsCorrectFallback(LastSelected))
                    return LastSelected;
                if (policy.Has(FallbackNavigationPolicy.AllowNavigateToEntryPoint) && IsCorrectFallback(EntryPoint))
                    return EntryPoint;

                return null;
            }

            ExtractOptions(current, out var needLock, out var needHandleEnter);

            if (isEnterKey && !needHandleEnter)
                return null;

            if (needLock)
                return null;

            var currentNode = FindStartNodeRelativeBy(current.transform);

            if (currentNode.IsNone)
            {
                Debug.LogError($"Internal error. Report the problem using the link: {Utils.LinkToReportIssues}");
                return null;
            }

            if (NeedUseJumping(current, out var jumping, out var nextJump, out var reverseJump))
            {
                var candidate = reverse ? reverseJump : nextJump;
                var candidateNode = candidate ? Drivers.CreateNode(candidate.transform) : EasyTabNode.None;

                if (!candidateNode.IsNone && candidateNode.IsSelectable)
                    return candidate;

                // fallback
                switch (jumping)
                {
                    case JumpingPolicy.UseOnlyJumps:
                        return null;
                    case JumpingPolicy.UseJumpsOrTheirNext when !candidateNode.IsNone:
                    {
                        if (!_visitedGameObjects.Add(candidate))
                        {
                            // test-case: UseJumpsOrTheirNextTestCase5
                            Debug.LogError("Cyclic link detected in jumps");
                            return null;
                        }

                        var next = GetNext(candidate, reverse);
                        if (next)
                            return next;
                        break;
                    }
                    case JumpingPolicy.UseJumpsOrHierarchy:
                    {
                        var next = GetNextWithoutPolicies(currentNode, reverse);
                        if (next)
                            return next;
                        break;
                    }
                }
            }
            else
            {
                return GetNextWithoutPolicies(currentNode, reverse);
            }

            if (!currentNode.IsSelectable)
            {
                var policy = WhenCurrentIsNotSelectable;

                if (policy.Has(FallbackNavigationPolicy.AllowNavigateToLastSelected) && IsCorrectFallback(LastSelected))
                    return LastSelected;

                if (policy.Has(FallbackNavigationPolicy.AllowNavigateToClosest))
                {
                    var next = GetNextWithoutPolicies(currentNode, reverse);
                    if (next)
                        return next;
                }

                if (policy.Has(FallbackNavigationPolicy.AllowNavigateToEntryPoint) && IsCorrectFallback(EntryPoint))
                    return EntryPoint;

                return null;
            }

            return null;
        }
        
        private bool ParentsIsCorrect(Transform target) 
            => TryGetValidParent(target, out var result) && target.parent == result;

        private EasyTabNode FindStartNodeRelativeBy(Transform target)
        {
            if (!TryGetValidParent(target, out var result)) 
                target = result;

            return Drivers.CreateNode(target);
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
                var parentNode = Drivers.CreateNode(parent);
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

        private void ExtractOptions(GameObject target, out bool needLock, out bool needHandleEnter)
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

        private bool NeedUseJumping(GameObject target, out JumpingPolicy jumpingPolicy, out GameObject next,
            out GameObject reverse)
        {
            if (!target.TryGetComponent<EasyTab>(out var easyTab))
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

        private GameObject GetNextWithoutPolicies(EasyTabNode currentNode, bool reverse)
        {
            var next = _easyTabNodeSolver.FindNextTab(currentNode, reverse);
            if (!next.IsNone)
            {
                var nextTransform = (Component)next.GetTarget(); // target is EasyTab or Transform
                return nextTransform.gameObject;
            }

            return null;
        }
    }
}