using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;


public interface ICollidable : IGameObject
{
    public int Damage { get; set; }
    public bool IsAlly { get; set; }
    public int StandingOn { get; set; }
    public List<int> StandingOver { get; set; }
    public int Gold { get; set; }
    public int Xp { get; set; }
    public Vector2 PositionNew { get; set; }
    public Vector2 PositionAfterCollision { get; set; }
    public Rectangle RectangleAfterCollision { get; }
    public int CollisionPriority { get; set; }
    public Rectangle RectangleNew { get; }
    public void OnCollision(ICollidable collidingWith);
}
