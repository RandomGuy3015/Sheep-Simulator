using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Test.InputMangement;

public class KeybindField : IKeybindField
{
    private InputManager mInputManager;
    private Vector2 mPosition;
    private Rectangle mRectangle;
    private KeyBind mKeybind;
    private Texture2D mTexture;
    private Texture2D mHoverTexture;

    private string mOptionText;
    private SpriteFont mFont;
    private bool mIsSelected;
    private Keys mBufferedKey;
    private string mBufferedKeyString;
    private const int OptionTextDrawPositionXOffset = 8;
    private const int OptionTextDrawPositionYOffset = 6;
    private int mKeybindValueDrawPositionXOffset;
    private int mKeybindValueDrawPositionYOffset;
    private Vector2 mOptionNameTextPosition;
    private Vector2 mValueTextPosition;
    private KeybindingManager mKeybindingManager;
    private ActionType mActionType;
    private bool mIsHovering;


    public KeybindField(KeybindingManager keybindingManager, KeyBind keybind, Texture2D hoverTexture, Texture2D texture, SpriteFont font, InputManager inputManager, int width, int height, int positionX, int positionY)
    {
        mActionType = keybind.GetAction();
        mRectangle = new Rectangle(positionX, positionY, width, height);
        mInputManager = inputManager;
        mKeybind = keybind;
        mTexture = texture;
        mHoverTexture = hoverTexture;

        mPosition = new Vector2(positionX, positionY);
        mOptionText = keybind.GetAction().ToString();
        mFont = font;
        mBufferedKey = keybind.GetKeyBinding();
        mBufferedKeyString = keybind.GetKeyBinding().ToString();
        mKeybindValueDrawPositionXOffset = 220;
        mKeybindValueDrawPositionYOffset = 6;
        mKeybindingManager = keybindingManager;
    }

    public void DrawTexture(SpriteBatch spriteBatch)
    {
        Texture2D textureToDraw = mIsHovering ? mHoverTexture : mTexture;
        spriteBatch.Draw(textureToDraw, mRectangle, Color.White);
    }

    public void DrawText(SpriteBatch spriteBatch)
    {
        spriteBatch.DrawString(mFont, mOptionText, mOptionNameTextPosition, Color.White);
    }

    public void DrawBufferedValue(SpriteBatch spriteBatch)
    {
        spriteBatch.DrawString(mFont, mBufferedKeyString, mValueTextPosition, Color.White);
    }

    public void ProcessInput(InputState inputState, InputManager inputManager)
    {
        if (inputManager.IsActionInputted(inputState, ActionType.PressBackSpaceKey))
        {
            mBufferedKey = mKeybind.GetKeyBinding();
            mBufferedKeyString = mKeybind.GetKeyBinding().ToString();

        }
        else if (inputManager.IsActionInputted(inputState, ActionType.PressEnterKey))
        {
            SaveKeybinding();
            Deselect();
        }
        else if (inputManager.IsKeyActionInputted(inputState))
        {
            var keyAction = inputManager.GetFirstKeyInput(inputState);
            // get key pressed
            mBufferedKey = keyAction.GetInputEvent().GetKey();
            mBufferedKeyString = mBufferedKey.ToString();
        }

    }

    private void SaveKeybinding()
    {
        var currentKeyBindDictEntry = mKeybindingManager.GetKeyBinding(mKeybind.GetAction());
        KeyEvent keyEvent = new KeyEvent(currentKeyBindDictEntry.GetEventType(), mBufferedKey);
        mKeybind = new KeyBind(mKeybind.GetAction(), keyEvent);
        mKeybindingManager.SetKeyBind(mActionType, keyEvent);
    }

    public void ChangePosition(float x, float y)
    {
        mRectangle.X = (int)x; mRectangle.Y = (int)y;
        mValueTextPosition = CalculateValueTextPosition();
        mOptionNameTextPosition = CalculateKeybindNameTextPosition();
    }

    public Vector2 CalculateKeybindNameTextPosition()
    {
        Vector2 textSize = mFont.MeasureString(mOptionText);
        return new Vector2(
            mRectangle.X + OptionTextDrawPositionXOffset,
            mRectangle.Y + OptionTextDrawPositionYOffset
        );
    }

    public Vector2 CalculateValueTextPosition()
    {
        Vector2 textSize = mFont.MeasureString(mOptionText);
        return new Vector2(
            mRectangle.X + mKeybindValueDrawPositionXOffset,
            mRectangle.Y + mKeybindValueDrawPositionYOffset
        );
    }




    public void Update(InputState inputState)
    {
        Vector2 worldMousePosition = inputState.mMousePosition;
        mIsHovering = mRectangle.Contains(worldMousePosition);
        // throw event when LMB clicked while on button
        if (mInputManager.IsActionInputted(inputState, ActionType.MouseLeftButtonClick) && mRectangle.Contains(worldMousePosition))
        {
            Select();
            mBufferedKeyString = "";

        }
        if (mInputManager.IsActionInputted(inputState, ActionType.MouseLeftButtonClick) && !mRectangle.Contains(worldMousePosition))
        {
            Deselect();
            mBufferedKey = mKeybind.GetKeyBinding();
            mBufferedKeyString = mKeybind.GetKeyBinding().ToString();

        }
        if (mIsSelected)
        {
            ProcessInput(inputState, mInputManager);
        }

    }

    public void Draw(SpriteBatch spriteBatch)
    {
        DrawTexture(spriteBatch);
        DrawText(spriteBatch);
        DrawBufferedValue(spriteBatch);
    }

    public void Select()
    {
        mIsSelected = true;
    }

    public void Deselect()
    {
        mIsSelected = false;
    }
}

