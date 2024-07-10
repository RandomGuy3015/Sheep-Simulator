using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace Test.GameObjects
{
	public class Sheep: IMovable, ICollidable, IActive
	{
        public float Speed { get; set; }
        public float SpeedSprint { get; set; }
        public int SituationBehaviour { get; set; }
        public Vector2 Direction { get; set; }
        public float Facing { get; set; }
        public List<Vector2> Path { get; set; }
        public Vector2 Position { get; set; }
        public int ObjectId { get; set; }
        public string Asset { get; set; }
        public Rectangle Rectangle { get; }
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


        public Sheep()
		{

		}

        public Vector2 Center()
        {
            return Vector2.Zero;
        }

        public Vector2 Destination()
        {
            return Vector2.Zero;
        }

        public void Behaviour(GameTime gameTime) { }

        public void Animate(GameTime gameTime) { }
        public void MoveStep(bool sprinting) { }
        public void SetDestination(Vector2 destination) { }
        public void OnCollision(ICollidable collidingWith) { }

        public void Update(GameTime gameTime) { }
        public void Draw(SpriteBatch spriteBatch) { }
	}
}

