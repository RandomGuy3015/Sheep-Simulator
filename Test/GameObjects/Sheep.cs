using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Test.DynamicContentManagement;



namespace Test
{
	public class Sheep: IMovable, ICollidable, IActive
	{
        // Location
        public int ObjectId { get; set; }
        public Vector2 Direction { get; set; }
        public float Facing { get; set; }
        public List<Vector2> Path { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 PositionNew { get; set; }
        public Vector2 PositionAfterCollision { get; set; }


        public float Speed { get; set; }
        public float SpeedSprint { get; set; }
        public int Damage { get; set; }
        public int SituationBehaviour { get; set; }
        public bool IsAlly { get; set; }
        public int StandingOn { get; set; }
        public List<int> StandingOver { get; set; }

        public string Asset { get; set; }
        public int Gold { get; set; }
        public int Xp { get; set; }

        // Collision
        public int CollisionPriority { get; set; }
        public Rectangle Rectangle { get; }
        public Rectangle RectangleNew { get; }
        public Rectangle RectangleAfterCollision { get; }

        private Vector2 mDestination;
        private Vector2 mCenter;


        public Sheep(Vector2 position, string textureName)
		{
            Position = position;
            Asset = textureName;
            mCenter = new Vector2((ContentDictionary.TextureDict[Asset].Width / 2), (ContentDictionary.TextureDict[Asset].Height / 2));
            
		}
        public Vector2 Center()
        {
            return mCenter;
        }

        public Vector2 Destination()
        {
            return mDestination;
        }

        public void SetDestination(int range) {
            Random random = new Random();
            mDestination = new Vector2(random.Next(-range, range), random.Next(-range, range));
        }

        public void MoveStep(bool sprinting)
        {
            Direction = Position - Direction;
            Direction.Normalize();

            Position += Direction;
        }

        public void Behaviour(GameTime gameTime)
        {
            if ((Position - mDestination).Length() > 1)
            {
                MoveStep(false);
            }
            else
            {
                SetDestination(100);
            }
        }

        public void OnCollision(ICollidable collidingWith) { }

        public void Update(GameTime gameTime)
        {
            Behaviour(gameTime);
        }

        public void Animate(GameTime gameTime) { }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ContentDictionary.TextureDict[Asset], Position, null, Color.White, (float)Math.Atan2(Direction.Y, Direction.X), mCenter, 1, SpriteEffects.None, 0);

        }
    }
}

