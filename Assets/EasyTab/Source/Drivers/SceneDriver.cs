using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EasyTab
{
    public sealed class SceneDriver : IEasyTabNodeDriver<Scene>
    {
        private readonly EasyTabNodeDriver _drivers;
        
        // list for temporary storage of the root objects of the scene. it is needed to avoid frequent allocations
        // The initial capacity is 0, so GetRootGameObjects will set the best capacity before filling
        private readonly List<GameObject> _listOfGameObjects = new List<GameObject>(0);

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
            // Method overloading with List<> is used to avoid allocations
            target.GetRootGameObjects(_listOfGameObjects); 

            var child = _listOfGameObjects[childNumber];
            
            // It is necessary to clean the list so as not to store links and not to disrupt the GC
            _listOfGameObjects.Clear();

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