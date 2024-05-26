using UnityEngine.SceneManagement;

namespace EasyTab
{
    public sealed class SceneDriver : IEasyTabNodeDriver<Scene>
    {
        private readonly EasyTabNodeDriver _drivers;

        public SceneDriver(EasyTabNodeDriver drivers)
        {
            _drivers = drivers;
        }

        public BorderMode GetBorderMode(Scene target)
        {
            return BorderMode.Escape;
        }

        public EasyTabNode GetChild(Scene target, int childNumber)
        {
            // todo: remove alloc
            var roots = target.GetRootGameObjects(); 
            var child = roots[childNumber];

            if (child.TryGetComponent<EasyTab>(out var childEasyTab))
                return EasyTabNode.ByDriver(childEasyTab, _drivers.EasyTabDriver);
            
            return EasyTabNode.ByDriver(child.transform, _drivers.TransformDriver);
        }

        public int GetChildrenCount(Scene target)
        {
            return target.rootCount;
        }

        public bool IsSelectable(Scene target)
        {
            return false;
        }

        public EasyTabNode GetParent(Scene target)
        {
            return new EasyTabNode(null, _drivers.RootDriver);
        }
        
        BorderMode IEasyTabNodeDriver.GetBorderMode(object target)
        {
            return GetBorderMode((Scene)target);
        }

        EasyTabNode IEasyTabNodeDriver.GetChild(object target, int childNumber)
        {
            return GetChild((Scene)target, childNumber);
        }

        int IEasyTabNodeDriver.GetChildrenCount(object target)
        {
            return GetChildrenCount((Scene)target);
        }

        bool IEasyTabNodeDriver.IsSelectable(object target)
        {
            return IsSelectable((Scene)target);
        }

        EasyTabNode IEasyTabNodeDriver.GetParent(object target)
        {
            return GetParent((Scene)target);
        }
    }
}