using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Test.InputMangement;

public interface IKeybindField
{
    void Draw(SpriteBatch spriteBatch);
    
    void Update(InputState inputState);
    
    public void ChangePosition(float x, float y)
    {
    }
}