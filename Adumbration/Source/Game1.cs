using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Penumbra;
using System.Collections.Generic;

// ===================================
// GAME NAME:   Adumbration
// TEAM NAME:   TBD Games
// MEMBERS:     Nikki Murello
//              Scott Au Yeung
//              Julian Alvia
//              Alexander Gough
// ===================================

namespace Adumbration
{
    /// <summary>
    /// Holds the current state of the overall game
    /// </summary>
    public enum GameState
    {
        Menu,
        Game,
        Pause
    }

    public class Game1 : Game
    {
        #region Fields

        // MonoGame fields
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // states
        private KeyboardState kbState;
        private KeyboardState prevKbState;
        private GameState gameState;

        // menu enum
        private enum PauseButtons {
            Resume,
            Quit
        }

        private enum MainMenuButtons {
            
        }

        // general game settings/fields
        private Vector2 fullscreenRes;
        private Vector2 windowedRes;
        private bool IsFullscreen;
        private float globalScale;
        private Matrix tMatrix;

        private Dictionary<string, Texture2D> textureDict;

        // menu tracking
        private PauseButtons selectedPauseItem;

        // lighting stuff
        private PenumbraComponent penumbra;
        private PointLight playerLight;

        // Game objects
        private Player player;
        private Door closedDoor;

        #endregion

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            #region FieldInit

            // resolution/fullscreen
            IsFullscreen = false;
            fullscreenRes = new Vector2(
                    _graphics.GraphicsDevice.DisplayMode.Width,
                    _graphics.GraphicsDevice.DisplayMode.Height);
            windowedRes = new Vector2(1280, 720);

            SetFullscreen(IsFullscreen, windowedRes);

            // others
            globalScale = 4.0f;
            tMatrix = Matrix.Identity;
            gameState = GameState.Game;
            selectedPauseItem = PauseButtons.Resume;

            #endregion

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

            #region TextureLoading

            textureDict = new Dictionary<string, Texture2D>();

            // game sprites
            textureDict.Add("player", Content.Load<Texture2D>("Sprites/sprite_player"));
            textureDict.Add("walls", Content.Load<Texture2D>("Sprites/sprite_walls"));
            textureDict.Add("doors", Content.Load<Texture2D>("Sprites/sprite_doors"));
            textureDict.Add("mirror", Content.Load<Texture2D>("Sprites/sprite_mirror"));
            textureDict.Add("floors", Content.Load<Texture2D>("Sprites/sprite_altFloors"));
            textureDict.Add("whitePixel", Content.Load<Texture2D>("Sprites/sprite_whitePixel"));

            // ui textures
            textureDict.Add("pauseResume", Content.Load<Texture2D>("UI/ui_pauseResume"));
            textureDict.Add("pauseQuit", Content.Load<Texture2D>("UI/ui_pauseQuit"));

            #endregion

            #region ObjectCreation

            // LevelManager init
            LevelManager.Instance.Initialize(textureDict, GameLevels.TestLevel);

            // Player Object
            player = new Player(
                textureDict["player"],          // spritesheet in dict
                new Rectangle(0, 0, 6, 8),      // source
                new Rectangle(50, 50, 6, 8));   // initial pos

            // Door Object
            closedDoor = new Door(
                false,
                textureDict,
                new Rectangle(      // Source Rectangle
                    0,              // - X Location
                    0,              // - Y Location
                    16,             // - Width
                    16),            // - Height
                new Rectangle(      // Position
                    16 * 11,        // - X Location
                    0,              // - Y Location
                    16,             // - Width
                    16),            // - Height
                1);                 // Level
            #endregion

            #region PenumbraSetup

            // setting up player light
            playerLight = new PointLight()
            {
                Scale = new Vector2(300),
                Color = Color.White,
                ShadowType = ShadowType.Occluded
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

            #region//Subscribing methods to events
            closedDoor.OnKeyPressOnce += IsKeyPressedOnce;
            closedDoor.OnKeyPress += closedDoor.Interact;
            #endregion
        }

        protected override void Update(GameTime gameTime)
        {
            kbState = Keyboard.GetState();

            // ALT+ENTER toggles fullscreen
            if(kbState.IsKeyDown(Keys.LeftAlt) && IsKeyPressedOnce(Keys.Enter, kbState, prevKbState))
            {
                // toggles if game is fullscreen or not
                IsFullscreen = !IsFullscreen;

                // sets resolutions based on if game is fullscreen or not
                Vector2 newRes = IsFullscreen ? fullscreenRes : windowedRes;

                // set the state of fullscreen
                SetFullscreen(IsFullscreen, newRes);
            }

            // FULL UPDATE LOGIC
            switch(gameState)
            {
                case GameState.Game:
                    #region GameUpdateLogic

                    // transition to pause menu
                    if(IsKeyPressedOnce(Keys.Escape, kbState, prevKbState))
                    {
                        gameState = GameState.Pause;
                    }

                    // object logic
                    LevelManager.Instance.CurrentLevel.Update(gameTime);
                    player.Update(gameTime, LevelManager.Instance.CurrentLevel);
                    player.IsDead(LevelManager.Instance.CurrentLevel.Beams);
                    closedDoor.Update(gameTime);
                    closedDoor.Update(player);

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
                    tMatrix.M41 = _graphics.PreferredBackBufferWidth / 2 - player.CenterPos.X * globalScale;
                    tMatrix.M42 = _graphics.PreferredBackBufferHeight / 2 - player.CenterPos.Y * globalScale;

                    // scale: (x & y)
                    tMatrix.M11 = globalScale;
                    tMatrix.M22 = globalScale;

                    #endregion

                    #region Penumbra

                    playerLight.Position = player.CenterPos;
                    penumbra.Transform = tMatrix;

                    if(IsKeyPressedOnce(Keys.L, kbState, prevKbState))
                    {
                        penumbra.Visible = !penumbra.Visible;
                    }

                    #endregion

                    #endregion
                    break;

                case GameState.Pause:
                    #region PauseUpdateLogic

                    // turn off penumbra in pause menu
                    penumbra.Visible = false;

                    // transition back to game from pause menu
                    if(IsKeyPressedOnce(Keys.Escape, kbState, prevKbState))
                    {
                        penumbra.Visible = true;
                        gameState = GameState.Game;
                    }

                    // FSM for currently selected menu items and moving between menu options
                    switch(selectedPauseItem)
                    {
                        case PauseButtons.Resume:
                            // transition to quit state
                            if(IsKeyPressedOnce(Keys.Right, kbState, prevKbState))
                            {
                                selectedPauseItem = PauseButtons.Quit;
                            }

                            if(kbState.IsKeyDown(Keys.Enter))
                            {
                                penumbra.Visible = true;
                                gameState = GameState.Game;
                            }

                            break;

                        case PauseButtons.Quit:
                            // transition to resume state
                            if(IsKeyPressedOnce(Keys.Left, kbState, prevKbState))
                            {
                                selectedPauseItem = PauseButtons.Resume;
                            }

                            if(kbState.IsKeyDown(Keys.Enter))
                            {
                                Exit();
                            }

                            break;
                    }

                    #endregion
                    break;
            }

            base.Update(gameTime);

            // updates previous kb state
            prevKbState = kbState;
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

            // main game drawing FSM
            switch(gameState)
            {
                case GameState.Game:
                    #region GameDrawing

                    // Draw test level
                    LevelManager.Instance.Draw(_spriteBatch);

                    // Draw Player
                    player.Draw(_spriteBatch);

                    // Draw closed door
                    closedDoor.Draw(_spriteBatch);

                    #endregion

                    break;

                case GameState.Pause:
                    #region PauseDrawing

                    // FSM for drawing the pause menu options
                    switch(selectedPauseItem)
                    {
                        // TODO: make it so pause menu doesn't scale with the game

                        // draw button "Resume" hovered, centered around player
                        case PauseButtons.Resume:
                            _spriteBatch.Draw(
                                textureDict["pauseResume"],
                                new Vector2(
                                    player.CenterPos.X - textureDict["pauseResume"].Width / 2,
                                    player.CenterPos.Y - textureDict["pauseResume"].Height / 2),
                                Color.White);

                            break;

                        // draw button "Quit" hovered, centered around player
                        case PauseButtons.Quit:
                            _spriteBatch.Draw(
                                textureDict["pauseQuit"],
                                new Vector2(
                                    player.CenterPos.X - textureDict["pauseQuit"].Width / 2,
                                    player.CenterPos.Y - textureDict["pauseQuit"].Height / 2),
                                Color.White);

                            break;
                    }

                    #endregion

                    break;
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Sets the state of the game window's fullscreen and resolution
        /// </summary>
        /// <param name="isFullscreen">Whether fullscreen or not</param>
        /// <param name="resolution">Desired resolution to set</param>
        private void SetFullscreen(bool isFullscreen, Vector2 resolution)
        {
            _graphics.IsFullScreen = isFullscreen;
            _graphics.PreferredBackBufferWidth = (int)resolution.X;
            _graphics.PreferredBackBufferHeight = (int)resolution.Y;
            _graphics.ApplyChanges();
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