﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Adumbration
{
    /// <summary>
    /// Alexander Gough
    /// Door class that inherits from the Wall Class.
    /// Will have an open and closed state.
    /// Leads to other levels or back to the hub room.
    /// </summary>
    public class Door : Wall, IHitbox
    {
        // Fields
        private bool isOpen;
        private bool isInteracted;
        private Rectangle hitbox;
        private GameLevels level;
        private double doorOffsetTimer;     // keep track of time after interact

        KeyboardState kbState;
        KeyboardState kbStatePrev;

        // Property
        public Rectangle Hitbox
        {
            get { return hitbox; }
        }

        // Constructor(s)

        /// <summary>
        /// Parameterized Constructor of Door.
        /// Requires the base constructor parameters.
        /// </summary>
        /// <param name="isOpen"></param>
        /// <param name="spriteSheet"></param>
        /// <param name="sourceRect"></param>
        /// <param name="position"></param>
        public Door(Texture2D texture, Rectangle position, GameLevels level)
             : base(texture, new Rectangle(0, 0, 16, 16), position)
        {
            this.level = level;
            doorOffsetTimer = 20;   // num frames till load level

            isOpen = false;
            isInteracted = false;

            // Create door hitboxes
            hitbox = new Rectangle(
                position.X - position.Width / 2,
                position.Y - position.Height / 2,
                position.Width * 2,
                position.Height * 2);
        }

        // Methods

        #region // Game Loop

        /// <summary>
        /// Updates door state, checking for key presses and the player's hitbox.
        /// </summary>
        /// <param name="myPlayer">Reference to the player.</param>
        public void Update(Player myPlayer)
        {
            kbState = Keyboard.GetState();

            sourceRect.Y = isOpen ? 16 : 0;

            // if player interacts with door:
            if (hitbox.Intersects(myPlayer.Position) && 
                Game1.IsKeyPressedOnce(Keys.E, kbState, kbStatePrev) &&
                !isInteracted)
            {
                // change texture, start timer
                isOpen = true;
                isInteracted = true;
            }

            if(isInteracted)
            {
                doorOffsetTimer--;

                if(doorOffsetTimer <= 0)
                {
                    LevelManager.Instance.LoadLevel(level);
                }
            }

            kbStatePrev = kbState;
        }

        /// <summary>
        /// Draws this door.
        /// </summary>
        /// <param name="sb">Reference to the SpriteBatch.</param>
        public override void Draw(SpriteBatch sb)
        {
            // Draw door
            sb.Draw(
                spriteSheet,
                positionRect,
                sourceRect,
                Color.White);
        }

        #endregion

        /// <summary>
        /// Checks for a collision between an object and a door's hitbox.
        /// </summary>
        /// <param name="obj">References the object that may collide with a door's hitbox.</param>
        /// <returns>True if collision occurs, otherwise false.</returns>
        public override bool IsColliding(GameObject obj)
        {
            return hitbox.Intersects(obj.Position);
        }
    }
}
