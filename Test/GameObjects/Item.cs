using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Test
{
    /// <summary>
    /// Can be used to create an Item, which can disappear after some time
    /// </summary>
    public class Item: ICollidable
    {
        public Vector2 Position { get; set; }
        public int ObjectId { get; set; }
        public string Asset { get; set; }
        public Rectangle Rectangle { get; set; }
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


        protected Vector2 mCenter;
        protected float mScale;
        protected string mSound;

        protected float mTransparency;
        protected int mTimer;

        public Item(int objectid, Vector2 position, string asset, string sound = "", bool despawn = true)
        {
            ObjectId = objectid;
            Random random = new Random();

            if (random.Next(0, 5) == 1)
            {
                Game1.ItemDict.Remove(ObjectId);
                return;
            }

            Asset = asset;
            mSound = sound;
            mCenter = new Vector2(ContentDictionary.TextureDict[Asset].Width / 2, ContentDictionary.TextureDict[Asset].Height / 2);
            mScale = 0.1f;


            mTransparency = 1;
            if (despawn)
            {
                mTimer = 600;
            }
            else
            {
                mTimer = -1;
            }

            Position = position;


            int randomDirection = random.Next(0, 8);
            Vector2 newPosition = Position;

            if (sound != "")
            {
                Game1.SoundManager.PlaySfx(mSound);
            }

            Rectangle = new Rectangle((int) Position.X, (int)Position.Y , ContentDictionary.TextureDict[Asset].Width, ContentDictionary.TextureDict[Asset].Height);
        }


        public virtual void OnCollision(ICollidable collidingWith)
        {

        }

        public virtual void Update(GameTime gameTime)
        {

            if (mTimer > 0)
            {
                mTimer--;
            }


            if (mTimer == 0)
            {
                Game1.ItemDict.Remove(ObjectId);
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            // behaviour of almost dead gold...
            if (mTimer < 130 && mTimer % 30 == 0)
            {
                mTransparency = 0.2f;
            }
            else if (mTimer < 130 && mTimer % 15 == 0)

            {
                mTransparency = 1;
            }

            if (mTimer < 590)
            {
                spriteBatch.Draw(ContentDictionary.TextureDict[Asset], Position, null, Color.White * mTransparency, 0, mCenter, mScale, SpriteEffects.None, 0);
            }
        }

    }


    public class Poop: Item, ICollidable
    {

        public Poop(int objectid, Vector2 position, bool despawn = true) : base(objectid, position, "poop.png", "fart.wav")
        {
            ObjectId = objectid;
            Random random = new Random();

            // sometimes poop disappears directly
            if (random.Next(0, 5) == 1)
            {
                Game1.ItemDict.Remove(ObjectId);
                return;
            }
            
            Asset = "poop.png";
            mSound = "fart.wav";
            
            int randomInt = Math.Clamp((int) Math.Pow(Gold * 1.25f, 1f / 2.9f) - 2, 1, 6);
            
            mCenter = new Vector2(ContentDictionary.TextureDict[Asset].Width / 2, ContentDictionary.TextureDict[Asset].Height / 2);
            mScale = 0.1f;

            

            mTransparency = 1;
            if (despawn)
            {
                mTimer = 600;
            }
            else
            {
                mTimer = -1;
            }

            Position = position;

            
            int randomDirection = random.Next(0, 8);
            Vector2 newPosition = Position;
           
            // sound
            Game1.SoundManager.PlaySfx(mSound);
            
        }

    }
}
