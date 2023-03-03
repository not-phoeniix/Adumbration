using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Adumbration
{
    /// <summary>
    /// Representation of a player within Adumbration
    /// </summary>
    public class Player : GameObject
    {
        // Fields
        private bool hasDash;
        private int speed;
        private bool isMoving;

        // Properties
        /// <summary>
        /// Get property for whether the player has a dash or not
        /// </summary>
        public bool HasDash
        {
            get { return hasDash; }
        }

        // Constructor
        /// <summary>
        /// Player takes completely from Parent class
        /// for the constructor
        /// </summary>
        public Player(Texture2D spriteSheet, Rectangle sourceRect, Rectangle position)
            : base(spriteSheet, sourceRect, position)
        {
            hasDash = false;
        }

        // Methods
        /// <summary>
        /// Updates the player.
        /// </summary>
        /// <param name="gameTime">State of the game's time.</param>
        public override void Update(GameTime gameTime)
        {
            // Player movement
            KeyboardState currentKbState = Keyboard.GetState();
            KeyboardState previousKbState;

            // Set player speed
            speed = 5;

            // Place holder until Wall class is finished
            //if (this.recPosition.Intersects(Wall.rectPosition))
            //{
            //    speed = 0;
            //}

            if (currentKbState.IsKeyDown(Keys.W))
            {
                recPosition.Y -= speed;
                //isMoving = true;
                //if (hasDash && isMoving && (currentKbState.IsKeyDown(Keys.Space) && previousKbState.IsKeyUp(Keys.Space)))
                //{
                //    recPosition.Y 
                //}
            }

            if (currentKbState.IsKeyDown(Keys.A))
            {
                recPosition.X -= speed;
            }

            if (currentKbState.IsKeyDown(Keys.S))
            {
                recPosition.Y += speed;
            }

            if (currentKbState.IsKeyDown(Keys.D))
            {
                recPosition.X += speed;
            }

            previousKbState = currentKbState;
        }

        /// <summary>
        /// Checks for player collision with any GameObject.
        /// </summary>
        /// <param name="obj">Reference to any GameObject</param>
        /// <returns>True if player is colliding with a GameObject, otherwise false.</returns>
        public override bool IsColliding(GameObject obj)
        {
            if (obj.Position.Intersects(Position))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
