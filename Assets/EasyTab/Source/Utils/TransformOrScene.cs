using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EasyTab
{
    public struct TransformOrScene
    {
        // 0 is none, 1 is transform, 2 is scene
        private byte _state;
        
        public bool IsNone => _state == 0;
        public  bool IsTransform => _state == 1;
        public  bool IsScene => _state == 2;
        
        private Transform _transform;
        private Scene _scene;

        public TransformOrScene(Transform transform)
        {
            _state = 1;
            _transform = transform;
            _scene = default;
        }
        
        public TransformOrScene(Scene scene)
        {
            _state = 2;
            _transform = default;
            _scene = scene;
        }

        public Transform AsTransform => IsTransform ? _transform : throw new InvalidOperationException();
        public Scene AsScene => !IsTransform ? _scene : throw new InvalidOperationException();
        
        public static implicit operator TransformOrScene(Transform t) => new TransformOrScene(t); 
        public static implicit operator TransformOrScene(Scene s) => new TransformOrScene(s); 
    }
}