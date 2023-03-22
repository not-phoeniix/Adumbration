﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.DirectoryServices.ActiveDirectory;

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

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private KeyboardState kbState;
        private KeyboardState kbStateOld;

        private Texture2D fullSpritesheet;
        private Texture2D wallSpritesheet;

        // scale/transformation matrix
        private float globalScale;
        private Matrix tMatrix;

        // Player object & related fields
        Player player;
        private Texture2D playerTexture;

        // Door Test
        private Door door;
        private LightBeam beam;
        private Texture2D doorTexture;

        // Level Manager
        private static LevelManager lvlMgrSingleton;
        private Level levelTest;

        #endregion

        // Level Manager Property
        public static LevelManager LevelMgrSingleton
        {
            get { return lvlMgrSingleton; }
        }

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            globalScale = 4.0f;
            tMatrix = Matrix.Identity;

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

            // creating test level
            levelTest = new Level(wallSpritesheet, "BigLevelTest.txt");

            // Player Object
            player = new Player(
                playerTexture,                      // spritesheet
                new Rectangle(0, 0, 6, 8),          // source
                new Rectangle(50, 50, 6, 8));       // pos

            // Door Object
            door = new Door(
                false,
                doorTexture,
                new Rectangle(  // Source Rectangle
                    64,         // - X Location
                    0,          // - Y Location
                    16,         // - Width
                    10),        // - Height
                new Rectangle(                                  // Position
                    _graphics.PreferredBackBufferWidth / 2,     // - X Location
                    _graphics.PreferredBackBufferHeight / 2,    // - Y Location
                    36,                                         // - Width
                    48));                                       // - Height


            //light beam test
            beam = new LightBeam(
                fullSpritesheet,
                new Rectangle(      //source rectangle
                    64,
                    0,
                    1,
                    1),
                new Rectangle(
                    _graphics.PreferredBackBufferWidth / 2 - 200,       // - X Location
                    _graphics.PreferredBackBufferHeight / 2 - 100,      // - Y Location
                    10,                                                 // - Width
                    10));                                               // - Height
        }

        protected override void Update(GameTime gameTime)
        {
            kbState = Keyboard.GetState();

            if(kbState.IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            player.Update(gameTime, levelTest);
            player.IsDead(beam);

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

            // prevents player from zooming too far in or out
            if(globalScale > 8) { globalScale = 8; }  // max zoom
            if(globalScale < 2) { globalScale = 2; }    // min zoom

            #endregion

            // updates transformation matrix values
            // position:
            tMatrix.M41 = (-player.Position.X * globalScale) + (_graphics.GraphicsDevice.Viewport.Width / 2 - player.Position.Width * globalScale / 2);
            tMatrix.M42 = (-player.Position.Y * globalScale) + (_graphics.GraphicsDevice.Viewport.Height / 2 - player.Position.Height * globalScale / 2);
            // scale:
            tMatrix.M11 = globalScale;
            tMatrix.M22 = globalScale;

            base.Update(gameTime);

            kbStateOld = kbState;
        }

        protected override void Draw(GameTime gameTime)
        {
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
            levelTest.Draw(_spriteBatch);

            // Draw Player
            player.Draw(_spriteBatch);

            // Draw test beam
            beam.Draw(_spriteBatch);
            
            //door.Draw(_spriteBatch);

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