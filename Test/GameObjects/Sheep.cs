using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;




namespace Test
{
    public class Sheep : IMovable, ICollidable, IActive
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
        public int Energy { get; set; }
        public int mReproduction { get; set; }
        public int Age { get; set; }
        public bool Gender { get; set; }


        // Collision
        public int CollisionPriority { get; set; }
        public Rectangle Rectangle { get; }
        public Rectangle RectangleNew { get; }
        public Rectangle RectangleAfterCollision { get; }

        private Vector2 mDestination;
        private Vector2 mCenter;
        private float mScale;
        private int mRange;
        public int LoverId; // Id to find Lover Sheep


        // time
        DateTime startTime;


        public Sheep(Vector2 position, string textureName, int objectId, float speed, bool gender, float scale, int range)
        {
            Position = position;
            Asset = textureName;
            ObjectId = objectId;
            mCenter = new Vector2((ContentDictionary.TextureDict[Asset].Width / 2), (ContentDictionary.TextureDict[Asset].Height / 2));
            Direction = new Vector2(0, 0);
            startTime = DateTime.Now;
            Speed = speed;
            Gender = gender;
            mScale = scale / 3;
            mRange = range;
            SetDestination(mRange, true);
            mReproduction = 0;
            Rectangle = new Rectangle(ContentDictionary.TextureDict[Asset].Width, ContentDictionary.TextureDict[Asset].Height, (int)mCenter.X, (int)mCenter.Y);
            
        }
        public Vector2 Center()
        {
            return mCenter;
        }

        public Vector2 Destination()
        {
            return mDestination;
        }

        public void SetDestination(int range, bool randomDestination)
        {
            if (randomDestination)
            { 
                Random random = new Random();
                mDestination = new Vector2(random.Next(0, range), random.Next(0, range));
            }

            Direction = mDestination - Position;

            while (Direction.Length() > 2)
            {
                Direction = Direction / 2;
            }
            //Direction.Normalize();
        }

        public void MoveStep(bool sprinting)
        {
            if (sprinting)
            {
                Position += Direction * Speed * 3;
            }
            
             Position += Direction * Speed;
            
        }
        /// <summary>
        /// Sheep walks through Landscape and looks for food
        /// </summary>
        public void MoveMode()
        {
            // sheep trys to get as close as possible to the Destination
            if ((mDestination - Position).Length() > 1)
            {
                MoveStep(false);
            }
            else if ((DateTime.Now - startTime).TotalMilliseconds > 250)
            {
                startTime = DateTime.Now;
                Game1.SoundManager.PlaySfxChecked("sheepSound.wav");
                SetDestination(mRange, true);

                Random random = new Random();

                if (random.Next(0, 20) == 5)
                {
                    Game1.ItemDict[Game1.ItemCount] = new Poop(Game1.ItemCount, Position);
                    Game1.ItemCount++;
                    mScale += 0.1f;
                }
                mReproduction++;
            }
        }

        public void Behaviour(GameTime gameTime)
        {
            if (mReproduction >= 2 && !Gender)
            { 
                LoveMode();
                
                mReproduction = 0;
            }

            else
            {
                MoveMode();
            }
           
        }

        public void LoveMode()
        {
            MoveStep(true);
            Reproduction();
        }

        public void Reproduction()
        {
            Random random = new Random();
            int randomInt = random.Next(0, 2);

            if (randomInt == 1)
            {
                Game1.SheepQueue[Game1.SheepCount] = new Sheep(Position, "sheep.png", Game1.SheepCount, 1, true, 0.1f, mRange);

            }
            else
            {
                Game1.SheepQueue[Game1.SheepCount] = new Sheep(Position, "shaun.png", Game1.SheepCount, 1, false, 0.2f, mRange);
            }
            Game1.SheepCount++;
        }

        public void OnCollision(ICollidable collidingWith)
        {
            Game1.SheepDict.Remove(collidingWith.ObjectId);
        }

        public void Update(GameTime gameTime)
        {

            Behaviour(gameTime);

        }

        public void Animate(GameTime gameTime) { }

        public void Draw(SpriteBatch spriteBatch)
        {
            // (float)Math.Atan2(Direction.Y, Direction.X) -> projectiles
            spriteBatch.Draw(ContentDictionary.TextureDict[Asset], Position, null, Color.White, (float)Math.Atan2(Direction.Y, Direction.X), mCenter, mScale, SpriteEffects.None, 0);
            
        }
    }
}

