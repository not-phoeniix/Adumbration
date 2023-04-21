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
        Game,
        MainMenu,
        PauseMenu,
        HelpMenu
    }

    public class Game1 : Game
    {
        #region // Fields

        // MonoGame fields
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // states
        private KeyboardState kbState;
        private KeyboardState prevKbState;
        private static GameState gameState;

        // general game settings/fields
        private Vector2 fullscreenRes;
        private Vector2 windowedRes;
        private bool IsFullscreen;
        private float globalScale;
        private Matrix tMatrix;

        // textures
        private Dictionary<string, Texture2D> textureDict;

        // lighting stuff
        private PenumbraComponent penumbra;
        private PointLight playerLight;

        // Game objects
        private Player player;

        #endregion

        public static GameState GameState
        {
            get { return gameState; }
            set { gameState = value; }
        }

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            #region // Field inits

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
            gameState = GameState.MainMenu;

            #endregion

            #region // Penumbra inits

            // shading initializing
            penumbra = new PenumbraComponent(this);
            Components.Add(penumbra);

            // changing penumbra initial properties
            penumbra.SpriteBatchTransformEnabled = true;
            penumbra.AmbientColor = Color.FromNonPremultiplied(24, 20, 37, 255);

            //penumbra.AmbientColor = Color.Red;

            #endregion

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            #region // Texture loading

            textureDict = new Dictionary<string, Texture2D>();

            // game sprites
            textureDict.Add("player", Content.Load<Texture2D>("Sprites/sprite_player"));
            textureDict.Add("walls", Content.Load<Texture2D>("Sprites/sprite_walls"));
            textureDict.Add("doors", Content.Load<Texture2D>("Sprites/sprite_doors"));
            textureDict.Add("mirror", Content.Load<Texture2D>("Sprites/sprite_mirror"));
            textureDict.Add("key", Content.Load<Texture2D>("Sprites/sprite_key"));
            textureDict.Add("floors", Content.Load<Texture2D>("Sprites/sprite_altFloors"));
            textureDict.Add("whitePixel", Content.Load<Texture2D>("Sprites/sprite_whitePixel"));
            textureDict.Add("endLevel", Content.Load<Texture2D>("Sprites/sprite_endBg"));

            // ui textures
            textureDict.Add("pauseResume", Content.Load<Texture2D>("UI/ui_pauseResume"));
            textureDict.Add("pauseQuit", Content.Load<Texture2D>("UI/ui_pauseQuit"));
            textureDict.Add("mainStart", Content.Load<Texture2D>("UI/ui_mainStart"));
            textureDict.Add("mainHelp", Content.Load<Texture2D>("UI/ui_mainHelp"));
            textureDict.Add("mainQuit", Content.Load<Texture2D>("UI/ui_mainQuit"));
            textureDict.Add("help", Content.Load<Texture2D>("UI/ui_help"));

            #endregion

            #region // Object creation

            // PauseMenu singleton init
            PauseMenu.Instance.Initialize(textureDict);

            // MainMenu singleton init
            MainMenu.Instance.Initialize(textureDict);

            // HelpMenu singleton init
            HelpMenu.Instance.Initialize(textureDict);

            // Player Object
            player = new Player(
                textureDict["player"],          // spritesheet in dict
                new Rectangle(0, 0, 6, 8),      // source
                new Rectangle(50, 50, 6, 8));   // initial pos

            // LevelManager singleton init
            LevelManager.Instance.Initialize(textureDict, penumbra, player);

            LevelManager.Instance.LoadLevel(GameLevels.End);

            MainMenu.Instance.Exit += Exit;

            #endregion

            #region // Penumbra object setup

            // setting up player light
            playerLight = new PointLight()
            {
                Scale = new Vector2(300),
                Color = Color.White,
                ShadowType = ShadowType.Occluded
            };

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
                    #region // Game update logic

                    // Penumbra enabled while in-game
                    penumbra.Visible = true;

                    // transition to pause menu
                    if(IsKeyPressedOnce(Keys.Escape, kbState, prevKbState))
                    {
                        gameState = GameState.PauseMenu;
                    }

                    // object logic
                    LevelManager.Instance.Update(gameTime, player);
                    player.Update(gameTime, LevelManager.Instance.CurrentLevel);

                    // clears beams only on the first frame of a level being loaded
                    if(LevelManager.Instance.CurrentLevel.FirstFrame)
                    {
                        LevelManager.Instance.CurrentLevel.FirstFrame = false;
                        LevelManager.Instance.CurrentLevel.Beams.Clear();
                    }

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

                    // update player light location
                    playerLight.Position = player.CenterPos;

                    penumbra.Lights.Clear();

                    // add player light if it doesn't exist in list yet
                    if(!penumbra.Lights.Contains(playerLight))
                    {
                        penumbra.Lights.Add(playerLight);
                    }

                    // add beam lights if they don't exist in list yet
                    foreach(LightBeam beam in LevelManager.Instance.CurrentLevel.Beams)
                    {
                        foreach(Light light in beam.Lights)
                        {
                            if(!penumbra.Lights.Contains(light))
                            {
                                penumbra.Lights.Add(light);
                            }
                        }
                    }

                    // update t matrix w/ penumbra
                    penumbra.Transform = tMatrix;

                    #endregion

                    #endregion
                    break;

                case GameState.PauseMenu:
                    #region // Pause menu update logic

                    // turn off penumbra in pause menu
                    penumbra.Visible = false;

                    // transition back to game from pause menu
                    if(IsKeyPressedOnce(Keys.Escape, kbState, prevKbState))
                    {
                        gameState = GameState.Game;
                    }

                    // update menu logic
                    PauseMenu.Instance.Update(kbState, prevKbState);

                    #endregion
                    break;

                case GameState.MainMenu:
                    #region // Main menu update logic

                    // turn off penumbra in main menu
                    penumbra.Visible = false;

                    // update menu logic
                    MainMenu.Instance.Update(kbState, prevKbState, player);

                    #endregion
                    break;

                case GameState.HelpMenu:
                    #region // Help menu update logic

                    // turn off penumbra in help menu
                    penumbra.Visible = false;

                    // update menu logic
                    HelpMenu.Instance.Update(kbState, prevKbState);

                    #endregion
                    break;
            }

            base.Update(gameTime);

            // updates previous kb state
            prevKbState = kbState;
        }

        protected override void Draw(GameTime gameTime)
        {
            penumbra.BeginDraw();

            GraphicsDevice.Clear(Color.Black);

            #region // Game drawing

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

            // main game drawing
            if(gameState == GameState.Game)
            {
                // Draw level
                LevelManager.Instance.Draw(_spriteBatch);

                // draws the final level on top of the other level when that state is set
                if(LevelManager.Instance.CurrentLevelEnum == GameLevels.End)
                {
                    _spriteBatch.Draw(textureDict["endLevel"], new Vector2(0, 0), Color.White);
                }

                // Draw Player
                player.Draw(_spriteBatch);
            }

            _spriteBatch.End();

            #endregion

            #region // Menu drawing

            // MENU DRAWING (independent of transformation matrix)
            _spriteBatch.Begin(
                SpriteSortMode.Deferred,
                null,
                SamplerState.PointClamp,
                null,
                null,
                null,
                null);

            switch(gameState)
            {
                // draw pause menu
                case GameState.PauseMenu:
                    PauseMenu.Instance.Draw(
                        _spriteBatch,
                        _graphics.GraphicsDevice.Viewport.Bounds);

                    break;

                // draw main menu
                case GameState.MainMenu:
                    MainMenu.Instance.Draw(
                        _spriteBatch,
                        _graphics.GraphicsDevice.Viewport.Bounds);

                    break;

                // draw help menu
                case GameState.HelpMenu:
                    HelpMenu.Instance.Draw(
                        _spriteBatch,
                        _graphics.GraphicsDevice.Viewport.Bounds);

                    break;
            }

            _spriteBatch.End();

            #endregion

            base.Draw(gameTime);
        }

        #region // Helper methods

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
        public static bool IsKeyPressedOnce(Keys key, KeyboardState currentState, KeyboardState prevState)
        {
            return currentState.IsKeyDown(key) && !prevState.IsKeyDown(key);
        }

        #endregion

    }
}