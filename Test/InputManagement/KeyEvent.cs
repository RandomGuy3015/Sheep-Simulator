using System;
using Microsoft.Xna.Framework.Input;

namespace Test.InputMangement;
    public interface IEvent
    {
        Keys GetKey();
    }
    public class KeyEvent : IEvent
    {
        public EventType mEventType;
        public Keys mKey;
        public string Type => "KeyEvent";

        public KeyEvent(EventType eventType, Keys key)
        {
            mEventType = eventType;
            mKey = key;
        }

        public Keys GetKey()
        {
            return mKey;
        }
        
        public void SetKey(Keys key)
        {
            mKey = key;
        }
        
        public EventType GetEventType()
        {
            return mEventType;
        }
    }

    public class MouseEvent : IEvent
    {
        public EventType mEventType;
        public MouseButton mButton;
        public string Type => "MouseEvent";

        public MouseEvent(EventType eventType, MouseButton button)
        {
            mEventType = eventType;
            mButton = button;
        }

        public Keys GetKey()
        {
            return Keys.None;
        }

        public EventType GetEventType()
        {
            return mEventType;
        }
    }

