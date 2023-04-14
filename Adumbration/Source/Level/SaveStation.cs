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

        // Properties
        public Rectangle Hitbox
        {
            get { return hitbox; }
        }

        // Constructor


        public SaveStation(Texture2D spriteSheet, Rectangle sourceRect, Rectangle position)
            : base(spriteSheet, sourceRect, position)
        {
            hitbox = new Rectangle(
                position.X - 1,
                position.Y - 1,
                position.Width + 2,
                position.Height + 2);
        }

        // Methods

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

            }
        }
    }
}
