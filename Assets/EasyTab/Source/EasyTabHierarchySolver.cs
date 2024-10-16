using Range = EasyTab.Internals.Range;

namespace EasyTab
{
    internal sealed class EasyTabHierarchySolver
    {
        public EasyTabNode FindNext(EasyTabNode current, bool reverse)
        {
            if (!reverse)
                if (TryFindFirstTabInChildren(current, out var nextInCurrent, false))
                    return nextInCurrent;

            return FindNextInNeighborsAndParents(current, reverse);
        }

        private EasyTabNode FindNextInNeighborsAndParents(EasyTabNode current, bool reverse)
        {
            var parent = current.Parent;
            if (parent.IsNone)
                return EasyTabNode.None;

            var indexOfCurrent = parent.GetChildNumberOf(current);
            if (indexOfCurrent == -1)
                // A node has a parent that does not know about this child node
                return EasyTabNode.None;

            var childrenCount = parent.ChildrenCount;
            var childrenNumbers = parent.BorderMode == BorderMode.Roll
                ? Range.Roll(childrenCount, indexOfCurrent, reverse)
                : Range.By(childrenCount, indexOfCurrent, reverse);

            foreach (var childNumber in childrenNumbers.Skip(1)) // skip self ('current')
            {
                var child = parent.GetChild(childNumber);
                if (child.IsSelectable)
                    if (reverse && TryFindFirstTabInChildren(child, out var firstTabInChild, true))
                        return firstTabInChild;
                    else
                        return child;

                if (TryFindFirstTabInChildren(child, out var nextTab, reverse))
                    return nextTab;
            }

            if (parent.BorderMode == BorderMode.Clamp)
            {
                if(current.IsSelectable)
                    return current;
                
                return EasyTabNode.None;
            }

            if (parent.BorderMode == BorderMode.Escape)
            {
                if (reverse && parent.IsSelectable)
                    return parent;

                var candidate = FindNextInNeighborsAndParents(parent, reverse);
                if (!candidate.IsNone)
                    return candidate;
            }

            if (TryFindFirstTabInChildren(current, out var nextTabInChildren, reverse))
                return nextTabInChildren;

            return EasyTabNode.None;
        }

        private bool TryFindFirstTabInChildren(EasyTabNode tab, out EasyTabNode firstTab, bool reverse)
        {
            firstTab = reverse ? FindFirstInChildrenReverse(tab) : FindFirstInChildren(tab);
            return !firstTab.IsNone;
        }

        private EasyTabNode FindFirstInChildren(EasyTabNode tab)
        {
            foreach (var childIndex in Range.By(tab.ChildrenCount, 0, reverse: false))
            {
                var childTab = tab.GetChild(childIndex);

                if (childTab.IsSelectable)
                    return childTab;

                var firstSelectableInChild = FindFirstInChildren(childTab);
                if (!firstSelectableInChild.IsNone)
                    return firstSelectableInChild;
            }

            return EasyTabNode.None;
        }


        private EasyTabNode FindFirstInChildrenReverse(EasyTabNode tab)
        {
            foreach (var childIndex in Range.By(tab.ChildrenCount, tab.ChildrenCount - 1 , reverse: true))
            {
                var childTab = tab.GetChild(childIndex);

                var firstSelectableInChild = FindFirstInChildrenReverse(childTab);
                if (!firstSelectableInChild.IsNone)
                    return firstSelectableInChild;

                if (childTab.IsSelectable)
                    return childTab;
            }

            return EasyTabNode.None;
        }
    }
}