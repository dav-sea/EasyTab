using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EasyTab
{
    public enum TargetType : byte
    {
        None = 0,
        Root,
        Transform,
        Scene
    }
    
    public struct Target : IEquatable<Target>
    {
        private TargetType _type;
        private Transform _transform;
        private Scene _scene;
        
        public TargetType TargetType => _type;
        public bool IsNone => _type == TargetType.None;
        public bool IsRoot => _type == TargetType.Root;
        public  bool IsTransform => _type == TargetType.Transform;
        public bool IsScene => _type == TargetType.Scene;
        
        public static Target None => default;
        public static Target Root => new Target(TargetType.Root, default, default);
        public static Target From(Transform transform) => transform ? new Target(TargetType.Transform, transform, default) : None;
        public static Target From(Scene scene) => new Target(TargetType.Scene, default, scene);
        
        public Transform AsTransform => IsTransform ? _transform : throw new InvalidCastException();
        public Scene AsScene => IsScene ? _scene : throw new InvalidCastException();
        
        public static implicit operator Target(Transform transform) => From(transform); 
        public static implicit operator Target(Scene scene) => From(scene); 
        
        private Target(TargetType type, Transform transform, Scene scene)
        {
            _type = type;
            _transform = transform;
            _scene = scene;
        }

        public bool Equals(Target other) 
            => _type == other._type 
               && Equals(_transform, other._transform) 
               && _scene.handle == other._scene.handle;

        public override bool Equals(object obj) 
            => obj is Target other && Equals(other);

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int)_type;
                hashCode = (hashCode * 397) ^ (_transform != null ? _transform.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ _scene.GetHashCode();
                return hashCode;
            }
        }
    }
}