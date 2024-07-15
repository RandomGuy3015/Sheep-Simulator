using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Test.DynamicContentManagement;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using Test.InputMangement;

namespace Test
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private RenderTarget2D _renderTarget;
        private DynamicContentManager _contentManager;
        private Grid _grid;
        private NoiseMap _noiseMap;
        private WaveFunctionCollapse _waveFunctionCollapse;
        private Grid _WFCGrid;
        private Pathfinder _pathFinder;
        private InputManager _inputManager;
        private Camera _camera;

        public  static SoundManager SoundManager;

        private Sheep mSheep;
        private Sheep mShaun;
        public static Dictionary<int, Item> ItemDict;
        public static Dictionary<int, Sheep> SheepDict;
        public static Dictionary<int, Sheep> SheepQueue;
        public static int ItemCount;
        public static int SheepCount;

        private Vector2 _size;
        private int _gridSquareSize;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 1024;
            _graphics.PreferredBackBufferHeight = 1024;
            _graphics.ApplyChanges();

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _renderTarget = new RenderTarget2D(GraphicsDevice, 1000, 1000, false, GraphicsDevice.PresentationParameters.BackBufferFormat,
                DepthFormat.Depth24, 0, RenderTargetUsage.PreserveContents);
            _contentManager = new DynamicContentManager(Content);
            _size = new Vector2(128, 128);
            _gridSquareSize = 16;

            _grid = new (_size, _gridSquareSize, new Vector2(0, 0));
            _noiseMap = new (75, 75, 2295, (int)_size.X);
            //_waveFunctionCollapse = new(_grid, index => _grid.IsPathable(index));
            _pathFinder = new (_grid);
            _inputManager = new(new KeybindingManager());

            _WFCGrid = new(_size, _gridSquareSize, Vector2.Zero);
            _waveFunctionCollapse = new (_WFCGrid, _WFCGrid.IsPathable, () => new Point((int)_size.X, (int)_size.Y), _WFCGrid.GetNode, _WFCGrid.SetNode);

            _camera = new(_graphics, _inputManager);

            SoundManager = new SoundManager();
            base.Initialize();

            for (int i = 0; i < _noiseMap.WoodGrid.Count(); i++)
            {
                if (_noiseMap.WoodGrid[i] >= _noiseMap.WoodCutoff)
                {
                    _grid.SetFilled(i, true);
                }
            }
            for (int i = 0; i < _noiseMap.StoneGrid.Count(); i++)
            {
                if (_noiseMap.StoneGrid[i] >= _noiseMap.StoneCutoff)
                {
                    _grid.SetFilled(i, true);
                }
            }
            
            for (int i = (int)_size.X; i < _size.X * _size.Y - _size.X; i++)
            {
                if (i % _size.X > 0 && i % _size.X < _size.X -1)
                {
                    if (_grid.GetNeighbors(i).RemoveAll(loc => !_grid.GetFilled(loc)) <= 1)
                    {
                        _grid.SetFilled(i, false);
                    }
                }
            }
            

            Random rnd = new();

            int subdivs = 4;

            for (int j = 0; j < subdivs; j++)
            {
                for (int i = 0; i < _size.X * 6; i++)
                {

                    int a = rnd.Next(0 + j * (int)(0.25f * _size.X*_size.Y), (int)(_size.X*_size.Y) - (3 - j) * (int)(0.25f * _size.X*_size.Y));
                    while (_grid.GetFilled(a))
                    {
                        a = rnd.Next(0 + j * (int)(0.25f * _size.X * _size.Y), (int)(_size.X * _size.Y) - (3 - j) * (int)(0.25f * _size.X * _size.Y));
                    }
                    int b = rnd.Next(0 + j * (int)(0.25f * _size.X * _size.Y), (int)(_size.X * _size.Y) - (3 - j) * (int)(0.25f * _size.X * _size.Y));
                    while (_grid.GetFilled(a))
                    {
                        b = rnd.Next(0 + j * (int)(0.25f * _size.X * _size.Y), (int)(_size.X * _size.Y) - (3 - j) * (int)(0.25f * _size.X * _size.Y));
                    }

                    List<Point> path = _pathFinder.CalculatePath(_grid.GetPoint(a), _grid.GetPoint(b));

                    foreach (Point point in path)
                    {
                        _grid.SetPathable(point, true);
                    }
                }
            }
        }

        protected override void LoadContent()
        {
            List<string> everythingEverywhereAllAtOnce = ContentList();

            ContentDictionary.LoadContent(everythingEverywhereAllAtOnce, Content);
            
            mSheep = new Sheep(new Vector2(1000, 1000), "sheep.png",0, 0.05f, 0.2f, 1000);
            mShaun = new Sheep(new Vector2(1000, 1000), "shaun.png",1, 0.05f, 0.45f,1000);
            ItemDict = new Dictionary<int, Item>();
            SheepDict = new Dictionary<int, Sheep>();
            SheepQueue = new Dictionary<int, Sheep>();
            ItemCount = 0;
            SheepCount = 2; // 2 objects spawned already

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            mSheep.Update(gameTime);
            mShaun.Update(gameTime);
            
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

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            var inputState = _inputManager.Update();

            _camera.Update(inputState, _graphics);

            // TODO: Add your update logic here

            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(_renderTarget);
            GraphicsDevice.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };
            GraphicsDevice.Clear(Color.Transparent);

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: _camera.Transform);


            _grid.DrawGridSquares(_spriteBatch, _gridSquareSize);


            _grid.DrawGrid(_spriteBatch);

            //_waveFunctionCollapse.Draw(_spriteBatch);
            //_waveFunctionCollapse.Collapse();

            mSheep.Draw(_spriteBatch);
            mShaun.Draw(_spriteBatch);
            
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
            
            _spriteBatch.Begin();
            //_spriteBatch.Draw(_contentManager.Load<Texture2D>("square"), new Rectangle(50, 50, 100, 100), Color.IndianRed);
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.End();
            


            _spriteBatch.Begin(blendState: BlendState.AlphaBlend);
            _spriteBatch.Draw(_renderTarget, new Rectangle(0, 0, 1000, 1000), Color.White);
            _spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
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

            return contentStringList;
        }

    }
}