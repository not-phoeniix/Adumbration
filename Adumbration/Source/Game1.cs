using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Penumbra;

// 
// ===================================
// GAME NAME:   Adumbration
// TEAM NAME:   TBD Games
// MEMBERS:     Nikki Murello
//              Scott Au Yeung
//              Julian Alvia
//              Alexander Gough
// ===================================
//

namespace Adumbration
{
    public class Game1 : Game
    {
        #region Fields

        // MonoGame fields
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // states
        private KeyboardState kbState;
        private KeyboardState kbStateOld;

        // general game settings/fields
        private Vector2 screenRes;
        private float globalScale;
        private Matrix tMatrix;

        // all textures
        private Texture2D fullSpritesheet;
        private Texture2D wallSpritesheet;
        private Texture2D playerTexture;
        private Texture2D doorTexture;

        // lighting stuff
        private PenumbraComponent penumbra;
        private PointLight playerLight;

        // Game objects
        private Player player;
        private Door closedDoor;
        private LightBeam beam;

        #endregion

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // global field value setting
            screenRes = new Vector2(1280, 720);
            globalScale = 4.0f;
            tMatrix = Matrix.Identity;

            #region PenumbraInit

            // shading initializing
            penumbra = new PenumbraComponent(this);
            Components.Add(penumbra);

            // changing penumbra initial properties
            penumbra.SpriteBatchTransformEnabled = true;
            penumbra.AmbientColor = Color.FromNonPremultiplied(24, 20, 37, 255);

            #endregion

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // loading sprites/textures
            wallSpritesheet = Content.Load<Texture2D>("wall_spritesheet");
            playerTexture = Content.Load<Texture2D>("player_spritesheet");
            fullSpritesheet = Content.Load<Texture2D>("spritesheet");
            doorTexture = Content.Load<Texture2D>("door_spritesheet");

            #region ObjectCreation

            // LevelManager init
            LevelManager.Instance.Initialize(wallSpritesheet, GameLevels.TestLevel);

            // Player Object
            player = new Player(
                playerTexture,                  // spritesheet
                new Rectangle(0, 0, 6, 8),      // source
                new Rectangle(50, 50, 6, 8));   // initial pos

            // Door Object
            closedDoor = new Door(
                false,
                fullSpritesheet,
                new Rectangle(                                          // Source Rectangle
                    4 * 16,                                             // - X Location
                    6 * 16,                                             // - Y Location
                    16,                                                 // - Width
                    16),                                                // - Height
                new Rectangle(                                          // Position
                    _graphics.PreferredBackBufferWidth / 2 - 215,       // - X Location
                    _graphics.PreferredBackBufferHeight / 2 - 240,      // - Y Location
                    16,                                                 // - Width
                    16));                                               // - Height

            // light beam test
            beam = new LightBeam(
                fullSpritesheet,
                new Rectangle(      //source rectangle
                    64,
                    0,
                    1,
                    1),
                new Rectangle(
                    _graphics.PreferredBackBufferWidth / 2 - 300,       // - X Location
                    _graphics.PreferredBackBufferHeight / 2 - 110,      // - Y Location
                    2,                                                 // - Width
                    2),
                    Direction.Down);                                              // - Height
            #endregion

            #region PenumbraSetup

            // setting up player light
            playerLight = new PointLight()
            {
                Scale = new Vector2(300),
                Color = Color.White,
                ShadowType = ShadowType.Illuminated
            };

            // add lights
            penumbra.Lights.Add(playerLight);

            // add hulls
            Hull[,] wallHulls = LevelManager.Instance.CurrentLevel.WallHulls;
            for(int y = 0; y < wallHulls.GetLength(1); y++)
            {
                for(int x = 0; x < wallHulls.GetLength(0); x++)
                {
                    if(wallHulls[x, y] != null)
                    {
                        penumbra.Hulls.Add(wallHulls[x, y]);
                    }
                }
            }

            #endregion
        }

        protected override void Update(GameTime gameTime)
        {
            kbState = Keyboard.GetState();

            // ESC exits game
            if(kbState.IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            // ALT+ENTER toggles fullscreen
            if(kbState.IsKeyDown(Keys.LeftAlt) && IsKeyPressedOnce(Keys.Enter, kbState, kbStateOld))
            {
                _graphics.ToggleFullScreen();
            }

            // setting screen res if it changes
            if(_graphics.PreferredBackBufferWidth != screenRes.X ||
               _graphics.PreferredBackBufferHeight != screenRes.Y)
            {
                _graphics.PreferredBackBufferWidth = (int)screenRes.X;
                _graphics.PreferredBackBufferHeight = (int)screenRes.Y;
                _graphics.ApplyChanges();
            }

            player.Update(gameTime, LevelManager.Instance.CurrentLevel);
            player.IsDead(beam);
            beam.Update(gameTime, LevelManager.Instance.CurrentLevel);

            #region Zoom

            // zoom in (one press)
            if(kbState.IsKeyDown(Keys.OemPlus))
            {
                globalScale += 0.2f;
            }

            // zoom out (one press)
            if(kbState.IsKeyDown(Keys.OemMinus))
            {
                globalScale -= 0.2f;
            }

            // zoom bounds setting (min/max zoom level)
            if(globalScale > 8) { globalScale = 8; }  // max zoom
            if(globalScale < 2) { globalScale = 2; }    // min zoom

            #endregion

            #region MatrixUpdating

            // updates transformation matrix values

            // position: (x & y)
            tMatrix.M41 = screenRes.X / 2 - player.CenterPos.X * globalScale;
            tMatrix.M42 = screenRes.Y / 2 - player.CenterPos.Y * globalScale;

            // scale: (x & y)
            tMatrix.M11 = globalScale;
            tMatrix.M22 = globalScale;

            #endregion

            #region Penumbra

            playerLight.Position = player.CenterPos;
            penumbra.Transform = tMatrix;

            if(IsKeyPressedOnce(Keys.L, kbState, kbStateOld)) { 
                penumbra.Visible = !penumbra.Visible;
            }

            #endregion

            base.Update(gameTime);

            kbStateOld = kbState;
        }

        protected override void Draw(GameTime gameTime)
        {
            // start penumbra effect drawing
            penumbra.BeginDraw();

            GraphicsDevice.Clear(Color.FromNonPremultiplied(24, 20, 37, 255));

            // Deferred sort mode is default, PointClamp makes it so
            //   pixel art doesn't get blurry when upscaled
            _spriteBatch.Begin(
                SpriteSortMode.Deferred, 
                null, 
                SamplerState.PointClamp,
                null,
                null,
                null,
                tMatrix);

            // Draw test level
            LevelManager.Instance.Draw(_spriteBatch);

            // Draw Player
            player.Draw(_spriteBatch);

            // Draw test beam
            beam.Draw(_spriteBatch);
            
            closedDoor.Draw(_spriteBatch);

            _spriteBatch.End();

            // Draw Closed Door
            //_spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);

            base.Draw(gameTime);
        }

        /// <summary>
        /// Used to detect single key presses
        /// </summary>
        /// <param name="key">Desired key to check for</param>
        /// <param name="currentState">Current kb state</param>
        /// <param name="prevState">Previous frame's kb state</param>
        /// <returns></returns>
        private bool IsKeyPressedOnce(Keys key, KeyboardState currentState, KeyboardState prevState)
        {
            return currentState.IsKeyDown(key) && !prevState.IsKeyDown(key);
        }
    }
}