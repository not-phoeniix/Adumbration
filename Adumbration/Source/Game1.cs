using Microsoft.Xna.Framework;
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
        private Level levelTest;

        // Player Test
        Player player;
        private Texture2D playerTexture;

        // Door Test
        private Door door;
        private Texture2D openDoorTexture;
        private Texture2D closedDoorTexture;

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

            // Full Sprite Sheet Texture
            fullSpritesheet = Content.Load<Texture2D>("spritesheet");

            // Level Test Object
            levelTest = new Level(fullSpritesheet);

            // Player Texture
            playerTexture = Content.Load<Texture2D>("player_spritesheet");

            // Player Object
            player = new Player(playerTexture,
                new Rectangle(0, 0, 6, 8),
                new Rectangle(
                    _graphics.PreferredBackBufferWidth/2,
                    _graphics.PreferredBackBufferHeight/2,
                    36, 48));

            // Closed Door Texture
            //closedDoorTexture = Content.Load<Texture2D>();
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            // TODO: Add your update logic here
            player.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Deferred sort mode is default, PointClamp makes it so
            //   pixel art doesn't get blurry when upscaled
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
            levelTest.Draw(_spriteBatch);
            _spriteBatch.End();

            // Draw Player
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
            player.Draw(_spriteBatch);
            _spriteBatch.End();

            // Draw Closed Door
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);

            base.Draw(gameTime);
        }
    }
}