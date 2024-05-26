using UnityEngine;
using EasyTab.Internals;

namespace EasyTab
{
    [DisallowMultipleComponent]
    public sealed class EasyTab : MonoBehaviour
    {
        // *** General *** //
        public SelectableRecognition SelectableRecognition;
        public BorderMode BorderMode;
        public ChildrenExtracting ChildrenExtracting;
        
        // *** Jumping *** //
        [Header("Jumping")]
        public JumpingPolicy JumpingPolicy;

        [ShowIf(nameof(_isVisibleJumpingFields))]
        public GameObject NextJump;

        [ShowIf(nameof(_isVisibleJumpingFields))]
        public GameObject ReverseJump;
        
        // *** Options *** //
        [Header("Options")] 
        public NavigationLock NavigationLock;
        public EnterHandling EnterHandling;

        private bool _isVisibleJumpingFields() => JumpingPolicy != JumpingPolicy.DontUseJumps; 
    }
}