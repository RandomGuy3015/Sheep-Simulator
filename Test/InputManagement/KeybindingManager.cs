using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Input;

namespace Test.InputMangement;

public class KeybindingManager
{
    private Dictionary<ActionType, IEvent> mKeyBindings;
    
    public KeybindingManager()
    {
        SetDefaultBindings();
    }

    
    private void SetDefaultBindings()
        {
            mKeyBindings = new Dictionary<ActionType, IEvent>
            {
                { ActionType.MouseButton1Click, new MouseEvent(EventType.OnButtonPress, MouseButton.Button1) },
                { ActionType.MouseButton2Click, new MouseEvent(EventType.OnButtonPress, MouseButton.Button2) },
                { ActionType.MouseLeftButtonClick, new MouseEvent(EventType.OnButtonPress, MouseButton.LeftButton) },
                { ActionType.MouseMiddleButtonClick, new MouseEvent(EventType.OnButtonPress, MouseButton.MiddleButton) },
                { ActionType.MouseRightButtonClick, new MouseEvent(EventType.OnButtonPress, MouseButton.RightButton) },
                { ActionType.MouseButton1Held, new MouseEvent(EventType.OnButtonHeld, MouseButton.Button1) },
                { ActionType.MouseButton2Held, new MouseEvent(EventType.OnButtonHeld, MouseButton.Button2) },
                { ActionType.MouseLeftButtonHeld, new MouseEvent(EventType.OnButtonHeld, MouseButton.LeftButton) },
                { ActionType.MouseMiddleButtonHeld, new MouseEvent(EventType.OnButtonHeld, MouseButton.MiddleButton) },
                { ActionType.MouseRightButtonHeld, new MouseEvent(EventType.OnButtonHeld, MouseButton.RightButton) },
                { ActionType.MouseLeftButtonReleased, new MouseEvent(EventType.OnButtonRelease, MouseButton.LeftButton) },
                { ActionType.PressKey0, new KeyEvent(EventType.OnButtonPress, Keys.D0) },
                { ActionType.PressKey1, new KeyEvent(EventType.OnButtonPress, Keys.D1) },
                { ActionType.PressKey2, new KeyEvent(EventType.OnButtonPress, Keys.D2) },
                { ActionType.PressKey3, new KeyEvent(EventType.OnButtonPress, Keys.D3) },
                { ActionType.PressKey4, new KeyEvent(EventType.OnButtonPress, Keys.D4) },
                { ActionType.PressKey5, new KeyEvent(EventType.OnButtonPress, Keys.D5) },
                { ActionType.PressKey6, new KeyEvent(EventType.OnButtonPress, Keys.D6) },
                { ActionType.PressKey7, new KeyEvent(EventType.OnButtonPress, Keys.D7) },
                { ActionType.PressKey8, new KeyEvent(EventType.OnButtonPress, Keys.D8) },
                { ActionType.PressKey9, new KeyEvent(EventType.OnButtonPress, Keys.D9) },
                { ActionType.PressBackSpaceKey, new KeyEvent(EventType.OnButtonPress, Keys.Back) },
                { ActionType.PressEnterKey, new KeyEvent(EventType.OnButtonPress, Keys.Enter) },
                { ActionType.HoldKey0, new KeyEvent(EventType.OnButtonHeld, Keys.D0) },
                { ActionType.HoldKey1, new KeyEvent(EventType.OnButtonHeld, Keys.D1) },
                { ActionType.HoldKey2, new KeyEvent(EventType.OnButtonHeld, Keys.D2) },
                { ActionType.HoldKey3, new KeyEvent(EventType.OnButtonHeld, Keys.D3) },
                { ActionType.HoldKey4, new KeyEvent(EventType.OnButtonHeld, Keys.D4) },
                { ActionType.HoldKey5, new KeyEvent(EventType.OnButtonHeld, Keys.D5) },
                { ActionType.HoldKey6, new KeyEvent(EventType.OnButtonHeld, Keys.D6) },
                { ActionType.HoldKey7, new KeyEvent(EventType.OnButtonHeld, Keys.D7) },
                { ActionType.HoldKey8, new KeyEvent(EventType.OnButtonHeld, Keys.D8) },
                { ActionType.HoldKey9, new KeyEvent(EventType.OnButtonHeld, Keys.D9) },
                { ActionType.MoveCameraDown, new KeyEvent(EventType.OnButtonHeld, Keys.S) },
                { ActionType.MoveCameraLeft, new KeyEvent(EventType.OnButtonHeld, Keys.A) },
                { ActionType.MoveCameraUp, new KeyEvent(EventType.OnButtonHeld, Keys.W) },
                { ActionType.MoveCameraRight, new KeyEvent(EventType.OnButtonHeld, Keys.D) },
                { ActionType.PressKeyA, new KeyEvent(EventType.OnButtonPress, Keys.A) },
                { ActionType.PressKeyB, new KeyEvent(EventType.OnButtonPress, Keys.B) },
                { ActionType.PressKeyC, new KeyEvent(EventType.OnButtonPress, Keys.C) },
                { ActionType.PressKeyD, new KeyEvent(EventType.OnButtonPress, Keys.D) },
                { ActionType.PressKeyE, new KeyEvent(EventType.OnButtonPress, Keys.E) },
                { ActionType.PressKeyF, new KeyEvent(EventType.OnButtonPress, Keys.F) },
                { ActionType.PressKeyG, new KeyEvent(EventType.OnButtonPress, Keys.G) },
                { ActionType.PressKeyH, new KeyEvent(EventType.OnButtonPress, Keys.H) },
                { ActionType.PressKeyI, new KeyEvent(EventType.OnButtonPress, Keys.I) },
                { ActionType.PressKeyJ, new KeyEvent(EventType.OnButtonPress, Keys.J) },
                { ActionType.PressKeyK, new KeyEvent(EventType.OnButtonPress, Keys.K) },
                { ActionType.PressKeyL, new KeyEvent(EventType.OnButtonPress, Keys.L) },
                { ActionType.PressKeyM, new KeyEvent(EventType.OnButtonPress, Keys.M) },
                { ActionType.PressKeyN, new KeyEvent(EventType.OnButtonPress, Keys.N) },
                { ActionType.PressKeyO, new KeyEvent(EventType.OnButtonPress, Keys.O) },
                { ActionType.PressKeyP, new KeyEvent(EventType.OnButtonPress, Keys.P) },
                { ActionType.PressKeyQ, new KeyEvent(EventType.OnButtonPress, Keys.Q) },
                { ActionType.PressKeyR, new KeyEvent(EventType.OnButtonPress, Keys.R) },
                { ActionType.PressKeyS, new KeyEvent(EventType.OnButtonPress, Keys.S) },
                { ActionType.PressKeyT, new KeyEvent(EventType.OnButtonPress, Keys.T) },
                { ActionType.PressKeyU, new KeyEvent(EventType.OnButtonPress, Keys.U) },
                { ActionType.PressKeyV, new KeyEvent(EventType.OnButtonPress, Keys.V) },
                { ActionType.PressKeyW, new KeyEvent(EventType.OnButtonPress, Keys.W) },
                { ActionType.PressKeyX, new KeyEvent(EventType.OnButtonPress, Keys.X) },
                { ActionType.PressKeyY, new KeyEvent(EventType.OnButtonPress, Keys.Y) },
                { ActionType.PressKeyZ, new KeyEvent(EventType.OnButtonPress, Keys.Z) },
                { ActionType.PressKeyBackspace, new KeyEvent(EventType.OnButtonPress, Keys.Back) },
                { ActionType.PressKeyTab, new KeyEvent(EventType.OnButtonPress, Keys.Tab) },
                { ActionType.PressKeyEnter, new KeyEvent(EventType.OnButtonPress, Keys.Enter) },
                { ActionType.PressKeySpace, new KeyEvent(EventType.OnButtonPress, Keys.Space) },
                { ActionType.PressKeyCapsLock, new KeyEvent(EventType.OnButtonPress, Keys.CapsLock) },
                { ActionType.PressKeyEsc, new KeyEvent(EventType.OnButtonPress, Keys.Escape) },
                { ActionType.PressKeyF1, new KeyEvent(EventType.OnButtonPress, Keys.F1) },
                { ActionType.PressKeyF2, new KeyEvent(EventType.OnButtonPress, Keys.F2) },
                { ActionType.PressKeyF3, new KeyEvent(EventType.OnButtonPress, Keys.F3) },
                { ActionType.PressKeyF4, new KeyEvent(EventType.OnButtonPress, Keys.F4) },
                { ActionType.PressKeyF5, new KeyEvent(EventType.OnButtonPress, Keys.F5) },
                { ActionType.PressKeyF6, new KeyEvent(EventType.OnButtonPress, Keys.F6) },
                { ActionType.PressKeyF7, new KeyEvent(EventType.OnButtonPress, Keys.F7) },
                { ActionType.PressKeyF8, new KeyEvent(EventType.OnButtonPress, Keys.F8) },
                { ActionType.PressKeyF9, new KeyEvent(EventType.OnButtonPress, Keys.F9) },
                { ActionType.PressKeyF10, new KeyEvent(EventType.OnButtonPress, Keys.F10) },
                { ActionType.PressKeyF11, new KeyEvent(EventType.OnButtonPress, Keys.F11) },
                { ActionType.PressKeyF12, new KeyEvent(EventType.OnButtonPress, Keys.F12) },
                { ActionType.PressKeyPrintScreen, new KeyEvent(EventType.OnButtonPress, Keys.PrintScreen) },
                { ActionType.PressKeyScrollLock, new KeyEvent(EventType.OnButtonPress, Keys.Scroll) },
                { ActionType.PressKeyPause, new KeyEvent(EventType.OnButtonPress, Keys.Pause) },
                { ActionType.PressKeyInsert, new KeyEvent(EventType.OnButtonPress, Keys.Insert) },
                { ActionType.PressKeyHome, new KeyEvent(EventType.OnButtonPress, Keys.Home) },
                { ActionType.PressKeyPageUp, new KeyEvent(EventType.OnButtonPress, Keys.PageUp) },
                { ActionType.PressKeyDelete, new KeyEvent(EventType.OnButtonPress, Keys.Delete) },
                { ActionType.PressKeyEnd, new KeyEvent(EventType.OnButtonPress, Keys.End) },
                { ActionType.PressKeyPageDown, new KeyEvent(EventType.OnButtonPress, Keys.PageDown) },
                { ActionType.PressKeyArrowUp, new KeyEvent(EventType.OnButtonPress, Keys.Up) },
                { ActionType.PressKeyArrowDown, new KeyEvent(EventType.OnButtonPress, Keys.Down) },
                { ActionType.PressKeyArrowLeft, new KeyEvent(EventType.OnButtonPress, Keys.Left) },
                { ActionType.PressKeyArrowRight, new KeyEvent(EventType.OnButtonPress, Keys.Right) },
                { ActionType.PressKeyNumLock, new KeyEvent(EventType.OnButtonPress, Keys.NumLock) },
                { ActionType.PressKeyNumPad0, new KeyEvent(EventType.OnButtonPress, Keys.NumPad0) },
                { ActionType.PressKeyNumPad1, new KeyEvent(EventType.OnButtonPress, Keys.NumPad1) },
                { ActionType.PressKeyNumPad2, new KeyEvent(EventType.OnButtonPress, Keys.NumPad2) },
                { ActionType.PressKeyNumPad3, new KeyEvent(EventType.OnButtonPress, Keys.NumPad3) },
                { ActionType.PressKeyNumPad4, new KeyEvent(EventType.OnButtonPress, Keys.NumPad4) },
                { ActionType.PressKeyNumPad5, new KeyEvent(EventType.OnButtonPress, Keys.NumPad5) },
                { ActionType.PressKeyNumPad6, new KeyEvent(EventType.OnButtonPress, Keys.NumPad6) },
                { ActionType.PressKeyNumPad7, new KeyEvent(EventType.OnButtonPress, Keys.NumPad7) },
                { ActionType.PressKeyNumPad8, new KeyEvent(EventType.OnButtonPress, Keys.NumPad8) },
                { ActionType.PressKeyNumPad9, new KeyEvent(EventType.OnButtonPress, Keys.NumPad9) },
                { ActionType.PressKeyNumPadAdd, new KeyEvent(EventType.OnButtonPress, Keys.Add) },
                { ActionType.PressKeyNumPadSubtract, new KeyEvent(EventType.OnButtonPress, Keys.Subtract) },
                { ActionType.PressKeyNumPadMultiply, new KeyEvent(EventType.OnButtonPress, Keys.Multiply) },
                { ActionType.PressKeyNumPadDivide, new KeyEvent(EventType.OnButtonPress, Keys.Divide) },
                { ActionType.PressKeyNumPadDecimal, new KeyEvent(EventType.OnButtonPress, Keys.Decimal) },
                { ActionType.PressKeyEnterNumPad, new KeyEvent(EventType.OnButtonPress, Keys.Enter) }
            };
        }
    
        public Dictionary<ActionType, IEvent> GetKeyBindings()
        {
            return mKeyBindings;
        }
        public KeyEvent GetKeyBinding(ActionType action)
        {
            if (mKeyBindings[action] is not KeyEvent)
            {
                throw new System.Exception("Keybinding is not a KeyEvent");
            }
            return (KeyEvent) mKeyBindings[action];
        }
        
        
        public void SetKeyBindings(Dictionary<ActionType, IEvent> keyBindings)
        {
            mKeyBindings = keyBindings;
        }
        
        public void SetKeyBind(ActionType actionType, IEvent keyEvent)
        {
            if (mKeyBindings.ContainsKey(actionType)) {
                mKeyBindings.Remove(actionType);
            }
            mKeyBindings.Add(actionType, keyEvent);
        }
        
        
}