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

        public void UpdateAll()
        {
            UpdateInput();
            UpdateLastSelected();
        }

        public void UpdateInput()
        {
            bool tabPressed, shiftPressed, enterPressed;

#if ENABLE_INPUT_SYSTEM
            tabPressed = Keyboard.current.tabKey.wasPressedThisFrame;
            shiftPressed = Keyboard.current.shiftKey.isPressed;
            enterPressed = Keyboard.current.enterKey.wasPressedThisFrame ||
                           Keyboard.current.numpadEnterKey.wasPressedThisFrame;
#else
            tabPressed = Input.GetKeyDown(KeyCode.Tab);
            shiftPressed = Input.GetKey(KeyCode.LeftShift);
            enterPressed = Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter);
#endif
            
            if (tabPressed || enterPressed)
                Navigate(reverse: shiftPressed, isEnter: enterPressed);
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