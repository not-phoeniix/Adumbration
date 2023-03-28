﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Adumbration
{
    /// <summary>
    /// Alexander Gough
    /// Door class that inherits from abstract GameObject.
    /// Will have an open and closed state.
    /// Leads to other levels.
    /// </summary>
    public class Door : GameObject
    {
        // Fields
        private bool isOpen;

        // Properties

        /// <summary>
        /// Property that allows boolean isOpen to be instantiated from the constructor.
        /// </summary>
        public bool IsOpen
        {
            get { return isOpen; }
            set { isOpen = value; }
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
        public Door(bool isOpen, Texture2D spriteSheet, Rectangle sourceRect, Rectangle position)
             : base(spriteSheet, sourceRect, position)
        {
            this.isOpen = isOpen;
        }

        // Methods

        /// <summary>
        /// Updates the game's doors.
        /// </summary>
        /// <param name="gameTime">State of the game's time.</param>
        public override void Update(GameTime gameTime)
        {
            if (!isOpen)
            {
                sourceRect.X = 4 * 16;
            }
            else if (isOpen)
            {
                sourceRect.X = 0;
            }
        }

        /// <summary>
        /// Draws the game's doors.
        /// </summary>
        /// <param name="gameTime">State of the game's time.</param>
        public override void Draw(SpriteBatch sb)
        {
            // Draw open door if isOpen is true
            sb.Draw(
                spriteSheet,
                positionRect,
                sourceRect,
                Color.White);
        }

        /// <summary>
        /// Checks for a collision between an object and a door.
        /// </summary>
        /// <param name="obj">References the object that may collide with a door.</param>
        /// <returns>True if collision occurs, otherwise false.</returns>
        public override bool IsColliding(GameObject obj)
        {
            if (obj.Position.Intersects(Position))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Loads the next level.
        /// </summary>
        public void Interact(Player myPlayer)
        {
            KeyboardState kb = Keyboard.GetState();
            if (IsColliding(myPlayer))
            {
                if (kb.IsKeyDown(Keys.E))
                {
                    isOpen = true;
                }
            }
        }
    }
}
