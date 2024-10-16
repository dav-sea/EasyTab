using UnityEngine;
using UnityEngine.EventSystems;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace EasyTab
{
    /// <summary>
    /// It represents the integration of EasyTabSolver with the EventSystem and the InputSystem
    /// </summary>
    public sealed partial class EasyTabIntegration
    {
        public readonly EasyTabSolver Solver = new EasyTabSolver();
        private readonly EasyTabInput _easyTabInput = new EasyTabInput();

        public float SecondsForStartFastNavigation = 0.5f;
        public float SecondsBetweenSelectablesInFastNavigation = 0.1f;

        private float _nextNavigationTime;
        
        public void UpdateAll()
        {
            UpdateInput();
            UpdateLastSelected();
        }

        public void UpdateInput()
        {
            _easyTabInput.GetInput(out float tabPressTime, out var isShiftPressed, out var isEnterPressed);

            bool needNavigate = false;

            if (tabPressTime != 0)
            {
                if (_nextNavigationTime == 0)
                {
                    needNavigate = true;
                    _nextNavigationTime = Time.unscaledTime + SecondsForStartFastNavigation;
                }
                else if (_nextNavigationTime - Time.unscaledTime <= 0)
                {
                    needNavigate = true;
                    _nextNavigationTime = Time.unscaledTime + SecondsBetweenSelectablesInFastNavigation;
                }
            }
            else
            {
                _nextNavigationTime = 0;
            }
            
            if (needNavigate || isEnterPressed)
                Navigate(reverse: isShiftPressed, isEnter: isEnterPressed);
        }

        public void UpdateLastSelected()
        {
            var currentEventSystem = EventSystem.current;
            if (!currentEventSystem)
                return;

            var selected = currentEventSystem.currentSelectedGameObject;
            if (selected)
                Solver.LastSelected = selected;
        }

        public void Navigate(bool reverse = false, bool isEnter = false)
        {
            var currentEventSystem = EventSystem.current;
            if (!currentEventSystem)
                return;

            var current = currentEventSystem.currentSelectedGameObject;
            var next = Solver.GetNext(current, reverse, isEnter);
            if (next)
                currentEventSystem.SetSelectedGameObject(next);
        }
    }
}