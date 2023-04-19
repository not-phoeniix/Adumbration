using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.XAudio2;
using Penumbra;
using System.Collections.Generic;

namespace Adumbration
{
    /// <summary>
    /// Alexander Gough
    /// SaveStation class that inherits from the Wall Class.
    /// Will be able to update respawn location,
    /// depending on the room it is in.
    /// </summary>
    public class SaveStation : GameObject, IHitbox
    {
        // Fields
        private Rectangle hitbox;
        private bool isInteracted;
        private KeyboardState kbState;
        private KeyboardState kbStatePrev;

        // Properties
        public Rectangle Hitbox
        {
            get { return hitbox; }
        }

        // Constructor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="texture">Reference to </param>
        /// <param name="sourceRect"></param>
        /// <param name="position"></param>
        public SaveStation(Texture2D texture, Rectangle sourceRect, Rectangle position)
            : base(texture, sourceRect, position)
        {
            hitbox = new Rectangle(
                position.X - 1,
                position.Y - 1,
                position.Width + 2,
                position.Height + 2);
        }

        // Methods

        #region // Game Loop

        /// <summary>
        /// Updates Save Station, checking for key presses and the player's hitbox.
        /// </summary>
        /// <param name="myPlayer">Reference to the player.</param>
        public void Update(Player myPlayer)
        {
            kbState = Keyboard.GetState();

            // If player interacts with save station
            if (hitbox.Intersects(myPlayer.Position) &&
                kbState.IsKeyUp(Keys.E) &&
                kbStatePrev.IsKeyDown(Keys.E) &&
                !isInteracted)
            {
                isInteracted = true;
            }

            if (isInteracted)
            {
                LevelManager.Instance.CurrentLevel.SpawnPoint = new Vector2(
                    myPlayer.Position.X,
                    myPlayer.Position.Y);
            }
        }

        /// <summary>
        /// Draws this Save Station.
        /// </summary>
        /// <param name="sb">Reference to the SpriteBatch.</param>
        public override void Draw(SpriteBatch sb)
        {
            // Draw Save Station
            sb.Draw(
                spriteSheet,
                positionRect,
                sourceRect,
                Color.White);
        }

        #endregion

        /// <summary>
        /// Checks for a collision between an object and the save station's hitbox.
        /// </summary>
        /// <param name="obj">References the object that may collide with a door's hitbox.</param>
        /// <returns>True if collision occurs, otherwise false.</returns>
        public override bool IsColliding(GameObject obj)
        {
            return hitbox.Intersects(obj.Position);
        }

        /// <summary>
        /// Saves the respawn point if the station's hitbox is colliding 
        /// with the player AND if the player presses the E key.
        /// </summary>
        /// <param name="myPlayer">Reference to Game1's player.</param>
        /// <param name="currentState">The current state of the keyboard.</param>
        /// <param name="previousState">The previous state of the keyboard.</param>
        public void Interact(Player myPlayer, KeyboardState currentState, KeyboardState previousState)
        {
            if (currentState.IsKeyUp(Keys.E) &&
                previousState.IsKeyDown(Keys.E) &&
                IsColliding(myPlayer))
            {
                // Updates the respawn point to the level it is located in.
                LevelManager.Instance.CurrentLevel.SpawnPoint = new Vector2(
                    positionRect.X,
                    positionRect.Y);
            }
        }
    }
}
