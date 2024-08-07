using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using Test.InputMangement;

namespace Test
{
    public class Game1 : Game
    {
        // ########################################   VARIABLES   #############################################


        // -----------------------------------------   GRID VARS   -------------------------------------------
        private Grid _grid;
        private Grid _WFCGrid;
        private Grid _WFCGrid2;

        // -----------------------------------------   MANAGERS   --------------------------------------------

        private WFCManager _WFCManager;
        private Pathfinder _pathFinder;
        private InputManager _inputManager;
        public static SoundManager SoundManager;

        // ---------------------------------------   MONOGAME VARS   -----------------------------------------

        private SpriteBatch _spriteBatch;
        private RenderTarget2D _renderTarget;
        private Camera _camera;
        private GraphicsDeviceManager _graphics;

        // ---------------------------------------   SETTINGS VARS   -----------------------------------------

        private Vector2 _size = new (128, 128);
        private int _gridSquareSize = 16;

        private Point _windowSize = new (1024, 1024);

        // -----------------------------------------   OTHER VARS   ------------------------------------------

        public static Dictionary<int, Item> ItemDict;
        public static Dictionary<int, Sheep> SheepDict;
        public static Dictionary<int, Sheep> SheepQueue;
        public static int ItemCount;
        public static int SheepCount;
        private RenderTarget2D bgRenderTarget;


        // ########################################   CONSTRUCTOR   ###########################################
        public Game1()
        {
            _graphics = new(this)
            {
                PreferredBackBufferWidth = _windowSize.X,
                PreferredBackBufferHeight = _windowSize.Y
            };
            _graphics.ApplyChanges();

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        // ########################################   INITIALIZE   #############################################

        protected override void Initialize()
        {

            // ------------------------------------   GRID VARS   ------------------------------------------------

            _grid = new(_size, _gridSquareSize, new Vector2(0, 0));

            
            // -------------------------------------   MANAGERS   -----------------------------------------------

            SoundManager = new();
            _pathFinder = new(_grid);
            _inputManager = new(new());
            _WFCManager = new(_size, _gridSquareSize);

            // -----------------------------------   MONOGAME VARS   --------------------------------------------

            _renderTarget = new RenderTarget2D(GraphicsDevice, 1000, 1000, false, GraphicsDevice.PresentationParameters.BackBufferFormat,
                DepthFormat.Depth24, 0, RenderTargetUsage.PreserveContents);
            _camera = new(_graphics, _inputManager);

            // -----------------------------------   BackGround Rendering  --------------------------------------

            bgRenderTarget = new RenderTarget2D(GraphicsDevice, 2000, 2000);
           

            base.Initialize();
        }



        // #######################################################################################################
        // ######################################     UPDATE LOOP     ############################################
        // #######################################################################################################

        protected override void Update(GameTime gameTime)
        {
            
            // IMPORTANT POOP UPDATE
            foreach (Item item in ItemDict.Values)
            {
                item.Update(gameTime);
            }

            foreach (Sheep sheep in SheepDict.Values)
            {
                sheep.Update(gameTime);
                
            }

            foreach (Sheep sheep in SheepQueue.Values)
            {
                SheepDict.TryAdd(sheep.ObjectId, sheep);
            }

            SheepQueue.Clear();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            var inputState = _inputManager.Update();

            _camera.Update(inputState, _graphics);


            base.Update(gameTime);
        }



        // #######################################################################################################
        // ##########################################     DRAW     ###############################################
        // #######################################################################################################

        protected override void Draw(GameTime gameTime)
        {

            // ######################################   DRAW INIT   ##############################################
            /*
            count++;
            GraphicsDevice.SetRenderTarget(bgRenderTarget);
            GraphicsDevice.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };
            GraphicsDevice.Clear(Color.Transparent);

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: _camera.Transform);

            if (!bgRenderBool)
            {
                _waveFunctionCollapse.Draw(_spriteBatch);
                 //bgRenderBool = true;
            }
            
            _spriteBatch.End();
            */
            

            GraphicsDevice.SetRenderTarget(_renderTarget);
            GraphicsDevice.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };
            GraphicsDevice.Clear(Color.Transparent);


            // Draws prerendered BackGround
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: _camera.Transform);
            _spriteBatch.Draw(bgRenderTarget, Vector2.Zero, Color.White);
            _spriteBatch.End();

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: _camera.Transform);

            //_grid.DrawGridSquares(_spriteBatch, _gridSquareSize);
            //_grid.DrawGrid(_spriteBatch);
            //_waveFunctionCollapse.Draw(_spriteBatch);
            //_waveFunctionCollapse2.Draw(_spriteBatch);
            // IMPORTANT POOP DRAW
            foreach (Item item in ItemDict.Values)
            {
                item.Draw(_spriteBatch);
            }
            
            foreach (Sheep sheep in SheepDict.Values)
            {
                sheep.Draw(_spriteBatch);
            }

            _spriteBatch.End();

            
            // other
            GraphicsDevice.SetRenderTarget(null);
            

            // #########################################   UI   #################################################

            _spriteBatch.Begin();
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.End();
            
            // #####################################   DRAW FINISH   ############################################

            _spriteBatch.Begin(blendState: BlendState.AlphaBlend);
            _spriteBatch.Draw(_renderTarget, new Rectangle(0, 0, 1000, 1000), Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }



        // #######################################################################################################
        // #######################################     HELPERS     ###############################################
        // #######################################################################################################
        protected override void LoadContent()
        {
            List<string> everythingEverywhereAllAtOnce = ContentList();

            ContentDictionary.LoadContent(everythingEverywhereAllAtOnce, Content);
            ItemDict = new Dictionary<int, Item>();
            SheepDict = new Dictionary<int, Sheep>();
            SheepQueue = new Dictionary<int, Sheep>();
            ItemCount = 0;
            SheepCount = 2; // 2 objects spawned already

            _spriteBatch = new SpriteBatch(GraphicsDevice);
            


            SheepDict[0] = new Sheep(new Vector2(1000, 1000), "sheep.png", 0, 2f, true, 0.2f, 2000);
            SheepDict[1] = new Sheep(new Vector2(1000, 1000), "shaun.png", 1, 2f, false, 0.45f, 2000);



            // rendering of BACKGROUND 
            GraphicsDevice.SetRenderTarget(bgRenderTarget);
            GraphicsDevice.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };
            GraphicsDevice.Clear(Color.Transparent);

            _spriteBatch.Begin();

            _WFCManager.Draw(_spriteBatch);
   
            _spriteBatch.End();
        }

        private List<string> ContentList()
        {
            List<string> contentStringList = new List<string>
            {
                // pictures
                "sheep.png",
                "shaun.png",
                "poop.png",
                "dot.png",

                // sounds
                "sheepSound.wav",
                "fart.wav"
            };

            // WFC Tiles
            contentStringList.AddRange(_WFCManager.TextureExporter());

            return contentStringList;
        }

    }
}