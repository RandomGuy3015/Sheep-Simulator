using System.ComponentModel;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Test.InputMangement;

public class Camera
{
    public static Camera Instance { get; private set; }
    public Matrix Transform { get; private set; }
    public Vector2 Position { get; private set; }

    public float mZoom = 6f;
    private float mRotation = 0.0f;

    public float mSpeed;

    private InputManager mInputManager;

    public Camera(GraphicsDeviceManager graphicsDeviceManager, InputManager inputManager)
    {
        // set the initial position to the center of the screen
        Position = new Vector2(graphicsDeviceManager.PreferredBackBufferWidth / 2f,
            graphicsDeviceManager.PreferredBackBufferHeight / 2f);
        UpdateMatrix(graphicsDeviceManager); // ensure the transformation matrix is updated
        mInputManager = inputManager;
        mSpeed = 1f;
    }

    public Vector2 ScreenToWorld(Vector2 screenPosition)
    {
        return Vector2.Transform(screenPosition, Matrix.Invert(Transform));
    }
    public Vector2 ScreenToWorld(Vector2 screenPosition, float cameraZoom)
    {
        //Matrix zoomMatrix = Matrix.CreateScale(new Vector3(cameraZoom, cameraZoom, 1));
        Matrix inverseTransform = Matrix.Invert(Transform);
        return Vector2.Transform(screenPosition, inverseTransform);
    }

    public void Update(InputState inputState, GraphicsDeviceManager graphicsDeviceManager)
    {
        var keyboardState = Keyboard.GetState();
        float x = 0;
        float y = 0;
        if (mInputManager.IsActionInputted(inputState, ActionType.SpeedUpCamera))
        {
            mSpeed *= 2;
        }
        if (mInputManager.IsActionInputted(inputState, ActionType.MoveCameraUp))
        {
            y = -1; // adjust the speed as necessary
        }
        if (mInputManager.IsActionInputted(inputState, ActionType.MoveCameraDown))
        {
            y = 1;
        }
        if (mInputManager.IsActionInputted(inputState, ActionType.MoveCameraLeft))
        {
            x = -1;
        }
        if (mInputManager.IsActionInputted(inputState, ActionType.MoveCameraRight))
        {
            x = 1;
        }
        if (mInputManager.IsActionInputted(inputState, ActionType.PressKeyC))
        {
            Position = new Vector2(0, 5000);
        }

        mZoom *= 1 + (0.0005f * mInputManager.GetMouseScroll());

        // limit zoom to certain values
        mZoom = MathHelper.Clamp(mZoom, 0.5f, 6f);

        mSpeed = 10 / mZoom;

        Vector2 movement = new(x, y);
        if (movement.Length() != 0)
        {
            movement.Normalize();
        }
        movement *= mSpeed;
        Position += movement; // update the position as a whole
        UpdateMatrix(graphicsDeviceManager);
    }

    private void UpdateMatrix(GraphicsDeviceManager graphicsDeviceManager)
    {

        Transform = Matrix.CreateTranslation(-Position.X, -Position.Y, 0) *
                    Matrix.CreateRotationZ(mRotation) *
                    Matrix.CreateScale(mZoom, mZoom, 1) *
                    Matrix.CreateTranslation(new Vector3(graphicsDeviceManager.GraphicsDevice.Viewport.Width / 2f,
                            graphicsDeviceManager.GraphicsDevice.Viewport.Height / 2f, 0));

    }
}