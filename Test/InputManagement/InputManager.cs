using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Test.InputMangement;

public class InputManager
{
    private MouseState mCurrentMouseState, mLastMouseState;
    private KeyboardState mCurrentKeyboardState, mLastKeyboardState;
    private Dictionary<ActionType, IEvent> mKeyBindings;
    private InputState mCurrentInputState;
    private KeybindingManager mKeyBindingsManager;



    public Vector2 MousePosition => new(mCurrentMouseState.X, mCurrentMouseState.Y);

    public InputManager(KeybindingManager keybindingManager)
    {
        mKeyBindingsManager = keybindingManager;
        mKeyBindings = mKeyBindingsManager.GetKeyBindings();
        mCurrentInputState = new InputState();
        mCurrentMouseState = Mouse.GetState();
        mCurrentKeyboardState = Keyboard.GetState();
    }
    
    public bool IsActionNumberInput(ActionType action)
    {
        return ActionType.PressKey0 == action| ActionType.PressKey1 == action| ActionType.PressKey2 == action| ActionType.PressKey3 == action| ActionType.PressKey4 == action| ActionType.PressKey5 == action| ActionType.PressKey6 == action| ActionType.PressKey7 == action | ActionType.PressKey8 == action| ActionType.PressKey9 == action| ActionType.HoldKey0 == action| ActionType.HoldKey1 == action| ActionType.HoldKey2 == action| ActionType.HoldKey3 == action| ActionType.HoldKey4 == action| ActionType.HoldKey5 == action| ActionType.HoldKey6 == action| ActionType.HoldKey7 == action| ActionType.HoldKey8 == action| ActionType.HoldKey9 == action;
    }
    
    public bool IsKeyPressed(Keys key)
    {
        return mCurrentKeyboardState.IsKeyDown(key);
    }

    // Check if a key was just pressed in the most recent update.
    public bool IsKeyTriggered(Keys key)
    {
        return (mCurrentKeyboardState.IsKeyDown(key)) &&
               (mLastKeyboardState.IsKeyUp(key));
    }

    public Dictionary<ActionType, IEvent> GetKeyBindings()
    {
        return mKeyBindings;
    }

    public void AddKeyBinding(KeyEvent keyEvents, ActionType actionType)
    {
        mKeyBindings.Add(actionType, keyEvents);
    }

    
    
    
    private bool IsDown(Keys key)
    {
        return mCurrentKeyboardState.IsKeyDown(key);
    }

    private static bool IsPressed(KeyboardState state, Keys key)
    {
        return state.IsKeyDown(key);
    }
    

    private static bool IsPressed(MouseState state, MouseButton mouseButton)
    {
        switch (mouseButton)
        {
            case MouseButton.LeftButton:
                return (state.LeftButton == ButtonState.Pressed);
            case MouseButton.MiddleButton:
                return (state.MiddleButton == ButtonState.Pressed);
            case MouseButton.RightButton:
                return (state.RightButton == ButtonState.Pressed);
            case MouseButton.Button1:
                return (state.XButton1 == ButtonState.Pressed);
            case MouseButton.Button2:
                return (state.XButton2 == ButtonState.Pressed);
        }

        return false;
    }

    private bool IsHeld(Keys key)
    {
        return (IsPressed(mCurrentKeyboardState, key) && IsPressed(mLastKeyboardState, key));
    }

    private bool IsHeld(MouseButton mouseButton)
    {
        return (IsPressed(mCurrentMouseState, mouseButton) && IsPressed(mLastMouseState, mouseButton));
    }

    private bool JustPressed(Keys key)
    {
        return (IsPressed(mCurrentKeyboardState, key) && (!IsPressed(mLastKeyboardState, key)));
    }

    private bool JustPressed(MouseButton mouseButton)
    {
        return (IsPressed(mCurrentMouseState, mouseButton) && (!IsPressed(mLastMouseState, mouseButton)));
    }

    private bool JustReleased(Keys key)
    {
        return ((!IsPressed(mCurrentKeyboardState, key)) && IsPressed(mLastKeyboardState, key));
    }

    private bool JustReleased(MouseButton mouseButton)
    {
        return ((!IsPressed(mCurrentMouseState, mouseButton)) && IsPressed(mLastMouseState, mouseButton));
    }

    public Point GetMousePosition()
    {
        return mCurrentMouseState.Position;
    }

    public bool IsMouseMoved()
    {
        return mCurrentMouseState.X != mLastMouseState.X || mCurrentMouseState.Y != mLastMouseState.Y;
    }

    public float GetMouseScroll()
    {
        return mCurrentMouseState.ScrollWheelValue - mLastMouseState.ScrollWheelValue;
    }
    
    private bool CheckEvent(IEvent @event)
    {
        if (@event is KeyEvent keyEvent)
        {
            return CheckEvent(keyEvent);
        }
        if (@event is MouseEvent mouseEvent)
        {
            return CheckEvent(mouseEvent);
        }

        return false;
    }
    
    
    private bool CheckEvent(KeyEvent keyEvent)
    {
        var eventType = keyEvent.GetEventType();
        var key = keyEvent.GetKey();
        switch (eventType)
        {
            case EventType.OnButtonPress:
                return JustPressed(key);
            case EventType.OnButtonHeld:
                return IsHeld(key);
            case EventType.OnButtonRelease:
                return JustReleased(key);
        }

        return false;
    }

    private bool CheckEvent(MouseEvent mouseEvent)
    {
        switch (mouseEvent.mEventType)
        {
            case EventType.OnButtonPress:
                return JustPressed(mouseEvent.mButton);
            case EventType.OnButtonHeld:
                return IsHeld(mouseEvent.mButton);
            case EventType.OnButtonRelease:
                return JustReleased(mouseEvent.mButton);
        }

        return false;
    }

    private void PollActions(InputState currentInputState)
    {
        foreach (ActionType actionType in mKeyBindings.Keys)
        {
            // for each action, check if it is performed, by checking if related
            // keyEvent has occured.
            if (CheckEvent(mKeyBindings[actionType]))
            {
                currentInputState.mInputs.Add(new Input(actionType, mKeyBindings[actionType]));
            }
        }
    }

    private void SetMousePosition(InputState currentInputState)
    {
        currentInputState.mMousePosition =
            new System.Numerics.Vector2(mCurrentMouseState.X, mCurrentMouseState.Y);
    }

    public bool IsActionInputted(InputState inputState, ActionType action)
    {
        foreach (var inputAction in inputState.mInputs)
        {
            if (inputAction.GetActionType() == action)
            {
                return true;
            }
            
        }

        return false;
    }
    
    public bool IsKeyActionInputted(InputState inputState)
    {
        foreach (var action in inputState.mInputs)
        {
            if (IsKeyAction(action.GetActionType()))
            {
                return true;
            }
        }

        return false;
    }
    
    public Input GetFirstKeyInput(InputState inputState)
    {
        foreach (var action in inputState.mInputs)
        {
            if (IsKeyAction(action.GetActionType()))
            {
                return action;
            }
        }

        return null;
    }

    private static bool IsKeyAction(ActionType actionType)
    {
        return actionType < ActionType.NoAction;
    }
    
    
    public Rectangle GetSelectionRectangle(Vector2 currentMousePosition, Vector2 startSelectionMousePosition)
    {
        var mouseState = Mouse.GetState();
        return new Rectangle((int)Math.Min(startSelectionMousePosition.X, currentMousePosition.X),
            (int)Math.Min(startSelectionMousePosition.Y, currentMousePosition.Y),
            (int)Math.Abs(startSelectionMousePosition.X - currentMousePosition.X),
            (int)Math.Abs(startSelectionMousePosition.Y - currentMousePosition.Y));
    }
    public bool IsWithinSelection(Rectangle hitbox, Vector2 selectionMouseStartPosition, Vector2 selectionMouseEndPosition)
    {
        // check if any part of the object is within the selection rectangle
        // this can be expanded or modified
        var selectionRect = new Rectangle((int)Math.Min(selectionMouseStartPosition.X, selectionMouseEndPosition.X),
            (int)Math.Min(selectionMouseStartPosition.Y, selectionMouseEndPosition.Y),
            (int)Math.Abs(selectionMouseStartPosition.X - selectionMouseEndPosition.X),
            (int)Math.Abs(selectionMouseStartPosition.Y - selectionMouseEndPosition.Y));
        return selectionRect.Intersects(hitbox);
    }


    // returns InputState with current mouse Postition and all
    // actions that have been done since last frame.
    public InputState Update()
    {
        mKeyBindings = mKeyBindingsManager.GetKeyBindings();

        mLastKeyboardState = new KeyboardState(mCurrentKeyboardState.GetPressedKeys());
        mCurrentKeyboardState = Keyboard.GetState();

        mLastMouseState = new MouseState(mCurrentMouseState.X, mCurrentMouseState.Y, mCurrentMouseState.ScrollWheelValue,
            mCurrentMouseState.LeftButton, mCurrentMouseState.MiddleButton, mCurrentMouseState.RightButton,
            mCurrentMouseState.XButton1, mCurrentMouseState.XButton2);

        mCurrentMouseState = Mouse.GetState();

        mCurrentInputState = new InputState();
        PollActions(mCurrentInputState);
        SetMousePosition(mCurrentInputState);

        return mCurrentInputState;
    }
    
    public void Flush()
    {
        mCurrentInputState = new InputState();
        mLastKeyboardState = new KeyboardState();
        mLastMouseState = new MouseState();
    }
}