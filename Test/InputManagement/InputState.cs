using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Test.InputMangement;

public class InputState
{
    // postion of the mouse
    public Vector2 mMousePosition = new();
    // List of Actions that have been taken since last Update.
    public List<Input> mInputs = new();
    
    
    // get pressed keys.
    public List<ActionType> GetInputtedActions()
    {
        return mInputs.ConvertAll(input => input.GetActionType());
    }
}

public class Input
{
    private ActionType mActionType;
    private IEvent mInputEvent;
    
    public Input(ActionType actionType, IEvent inputEvent)
    {
        mActionType = actionType;
        mInputEvent = inputEvent;
    }
    
    public ActionType GetActionType()
    {
        return mActionType;
    }
    
    public IEvent GetInputEvent()
    {
        return mInputEvent;
    }
}