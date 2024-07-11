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
    public class Poop: ICollidable
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

        

        protected Texture2D mTexture;
        private Vector2 mCenter;
        private float mScale;

        protected float mTransparency;
        protected int mTimer;
        public Poop(int objectid, Vector2 position, bool despawn=true)
        {
            ObjectId = objectid;
            Random random = new Random();

            if (random.Next(0, 5) == 1)
            {
                //MainGame.mObjectManager.mSceneGraphGold.Remove(ObjectId);
                return;
            }


            int randomInt = Math.Clamp((int) Math.Pow(Gold * 1.25f, 1f / 2.9f) - 2, 1, 6);
            Asset = "poop.png";
            mCenter = new Vector2(ContentDictionary.TextureDict[Asset].Width / 2, ContentDictionary.TextureDict[Asset].Height / 2);
            mScale = 0.1f;

            int iteration = 0;
            int distance = 8;

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
           

            Game1.SoundManager.PlaySfx("fart.wav");
            
            // Rectangle = new Rectangle((int) Position.X, (int)Position.Y , mTexture.Width, mTexture.Height);
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
                Game1.PoopDict.Remove(ObjectId);
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            // behaviour of almost dead gold...
            if (mTimer < 130 && mTimer % 30 == 0)
            {
                mTransparency = 0.2f;
            }
            else if(mTimer < 130 && mTimer % 15 == 0)

            {
                mTransparency = 1;
            }

            if (mTimer < 590)
            {
                spriteBatch.Draw(ContentDictionary.TextureDict[Asset], Position, null, Color.White * mTransparency, 0, mCenter, mScale, SpriteEffects.None, 0);
            }
        }

    }
}
