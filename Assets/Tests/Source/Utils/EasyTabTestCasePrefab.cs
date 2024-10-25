using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace EasyTab.Tests
{
    public class EasyTabTestCasePrefab : MonoBehaviour
    {
        public Selectable FirstSelectable;
        public List<Selectable> ExcpectSelectableSequence = new List<Selectable>();
        public bool NavigateFirst;
        
        public FallbackNavigationPolicy WhenCurrentIsNotSet =
            FallbackNavigationPolicy.AllowNavigateToLastSelected | FallbackNavigationPolicy.AllowNavigateToEntryPoint;

        public FallbackNavigationPolicy WhenCurrentIsNotSelectable  = FallbackNavigationPolicy.AllowAll;
        
        public static EasyTabTestCasePrefab Instantiate(string prefabName)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/Tests/Prefabs/{prefabName}.prefab");

            var go = Instantiate(prefab);
            var easyTabTestCasePrefab = go.GetComponent<EasyTabTestCasePrefab>();
            
            if (!EventSystem.current)
                go.gameObject.AddComponent<EventSystem>();

            return easyTabTestCasePrefab;
        }

        [ContextMenu("FillSequence")]
        private void FillSequence()
        {
            var solver = new EasyTabSolver();
            solver.WhenCurrentIsNotSet = WhenCurrentIsNotSet;
            solver.WhenCurrentIsNotSelectable = WhenCurrentIsNotSelectable;

            var current = solver.GetNext(FirstSelectable.gameObject, reverse: false);

            do
            {
                ExcpectSelectableSequence.Add(current.GetComponent<Selectable>());
                current = solver.GetNext(current, reverse: false);
            } while (current != FirstSelectable.gameObject && current != null && !ExcpectSelectableSequence.Contains(current.GetComponent<Selectable>()));
            
            EditorUtility.SetDirty(this);
        }
    }
}