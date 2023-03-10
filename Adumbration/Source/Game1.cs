﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D fullSpritesheet;
        private Texture2D wallSpritesheet;

        // Player Test
        Player player;
        private Texture2D playerTexture;

        // Door Test
        private Door door;
        private LightBeam beam;
        private Texture2D doorTexture;

        // Level Manager
        private static LevelManager lvlMgrSingleton;

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
            // TODO: Add your initialization logic here

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
            //levelTest = new Level(wallSpritesheet, 6, "../../../Source/LevelData/LevelTest2.txt");

            // Player Object
            player = new Player(
                playerTexture,
                new Rectangle(
                    0,
                    0,
                    6,
                    8),
                new Rectangle(
                    _graphics.PreferredBackBufferWidth/2,
                    _graphics.PreferredBackBufferHeight/2,
                    36,
                    48),
                _graphics.PreferredBackBufferHeight,
                _graphics.PreferredBackBufferWidth);

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
                    _graphics.PreferredBackBufferWidth / 2,     // - X Location
                    _graphics.PreferredBackBufferHeight / 2,    // - Y Location
                    10,                                         // - Width
                    10));                                       // - Height
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            // TODO: Add your update logic here
            player.Update(gameTime, levelTest);
            player.IsDead(beam);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.FromNonPremultiplied(24, 20, 37, 255));

            // Deferred sort mode is default, PointClamp makes it so
            //   pixel art doesn't get blurry when upscaled
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);


            lvlMgrSingleton.Draw(_spriteBatch);


            _spriteBatch.End();

            // Draw Player
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
            player.Draw(_spriteBatch);
            beam.Draw(_spriteBatch);
            //door.Draw(_spriteBatch);

            _spriteBatch.End();

            // Draw Closed Door
            //_spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);

            base.Draw(gameTime);
        }
    }
}