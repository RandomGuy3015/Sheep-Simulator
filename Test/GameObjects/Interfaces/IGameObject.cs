using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vector2 = Microsoft.Xna.Framework.Vector2;



public interface IGameObject
{
    public Vector2 Position { get; set; }
    public int ObjectId { get; set; }
    public string Asset { get; set; }
    public Rectangle Rectangle { get; }
    void Update(GameTime gameTime);
    void Draw(SpriteBatch spriteBatch);
}
