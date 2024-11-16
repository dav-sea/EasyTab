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
            bool tabPressed;
            
#if ENABLE_INPUT_SYSTEM
            var keyboard = Keyboard.current;
            if (keyboard != null)
            {
                tabPressed = keyboard.tabKey.isPressed;
                isShiftPressed = keyboard.shiftKey.isPressed;
                isEnterPressed = keyboard.enterKey.wasPressedThisFrame ||
                                 keyboard.numpadEnterKey.wasPressedThisFrame;
            }
            else // if not keyboard. like PS5. https://github.com/dav-sea/EasyTab/issues/7
            {
                tabPressed = false;
                isShiftPressed = false;
                isEnterPressed = false;
            }
#else
            tabPressed = Input.GetKey(KeyCode.Tab);
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