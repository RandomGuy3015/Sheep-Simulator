using Microsoft.Xna.Framework.Input;

namespace Test.InputMangement;

public class KeyBind
{
    private ActionType mAction;
    private KeyEvent mEvent;
    
    public KeyBind(ActionType action, KeyEvent keyEvent)
    {
        mAction = action;
        mEvent = keyEvent;
    }
    
    public ActionType GetAction()
    {
        return mAction;
    }
    
    public void SetAction(ActionType action)
    {
        mAction = action;
    }
    
    public void SetKeyEvent(KeyEvent keyEvent)
    {
        mEvent = keyEvent;
    }
    
    public KeyEvent GetKeyEvent()
    {
        return mEvent;
    }

    public Keys GetKeyBinding()
    {
        return mEvent.GetKey();
    }
    
    public void ChangeKeyBinding(Keys key)
    {
        mEvent.SetKey(key);
    }
}