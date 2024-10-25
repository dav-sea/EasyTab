using UnityEngine;
using UnityEngine.EventSystems;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace EasyTab
{
    public class EasyTabInput
    {
        private float _tabDownTime;

        public void GetInput(out float tabPressedTime, out bool isShiftPressed, out bool isEnterPressed)
        {
#if ENABLE_INPUT_SYSTEM
            var tabPressed = Keyboard.current.tabKey.isPressed;
            isShiftPressed = Keyboard.current.shiftKey.isPressed;
            isEnterPressed = Keyboard.current.enterKey.wasPressedThisFrame ||
                           Keyboard.current.numpadEnterKey.wasPressedThisFrame;
#else
            var tabPressed = Input.GetKey(KeyCode.Tab);
            isShiftPressed = Input.GetKey(KeyCode.LeftShift);
            isEnterPressed = Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter);
#endif

            if (_tabDownTime == 0 && tabPressed) // is first tab press
                _tabDownTime = Time.unscaledTime;
            else if (_tabDownTime != 0 && !tabPressed) // is tab up
                _tabDownTime = 0;

            tabPressedTime = _tabDownTime;
        }
    }
}