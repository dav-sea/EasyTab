using System.Collections.Generic;
using UnityEngine;

namespace EasyTab
{
    public sealed class SceneDriver : IEasyTabNodeDriver
    {
        private readonly EasyTabNodeDriver _drivers;
        
        // list for temporary storage of the root objects of the scene. it is needed to avoid frequent allocations
        // The initial capacity is 0, so GetRootGameObjects will set the best capacity before filling
        private readonly List<GameObject> _listOfGameObjects = new List<GameObject>(0);

        public SceneDriver(EasyTabNodeDriver drivers)
        {
            _drivers = drivers;
        }

        public BorderMode GetBorderMode(TransformOrScene _)
        {
            return BorderMode.Escape;
        }

        public EasyTabNode GetChild(TransformOrScene target, int childNumber)
        {
            // Method overloading with List<> is used to avoid allocations
            target.AsScene.GetRootGameObjects(_listOfGameObjects); 

            var child = _listOfGameObjects[childNumber];
            
            // It is necessary to clean the list so as not to store links and not to disrupt the GC
            _listOfGameObjects.Clear();

            return new EasyTabNode(child.transform, _drivers.TransformDriver);
        }

        public int GetChildrenCount(TransformOrScene target)
        {
            return target.AsScene.rootCount;
        }

        public bool IsSelectable(TransformOrScene target)
        {
            return false;
        }

        public EasyTabNode GetParent(TransformOrScene target)
        {
            return new EasyTabNode(default, _drivers.RootDriver);
        }
    }
}