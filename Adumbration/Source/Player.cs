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
        private int stop;
        private int windowHeight;
        private int windowWidth;
        private KeyboardState previousKbState;

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
        public Player(Texture2D spriteSheet, Rectangle sourceRect, Rectangle position, int windowH, int windowW)
            : base(spriteSheet, sourceRect, position)
        {
            windowHeight = windowH;
            windowWidth = windowW;
            hasDash = true;
        }

        // Methods
        /// <summary>
        /// Updates the player.
        /// </summary>
        /// <param name="gameTime">State of the game's time.</param>
        public override void Update(GameTime gameTime)
        {
            // Player input
            KeyboardState currentKbState = Keyboard.GetState();            

            // Set player speed
            speed = 5;

            //adds a stop int to make my(scott's) life easier while making the wall stuff
            stop = 0;

            // Place holder until Wall class is finished
            //if (this.recPosition.Intersects())
            //{
            //    speed = 0;
            //}

            #region// Diagonal Dashes           
            // North East
            if (currentKbState.IsKeyDown(Keys.W) && currentKbState.IsKeyDown(Keys.D) &&
                hasDash && currentKbState.IsKeyDown(Keys.Space) && previousKbState.IsKeyUp(Keys.Space)
                && (recPosition.Y - 50 > 0) && (recPosition.X + 50 <= windowHeight - 37))
            {
                // Changes position by 50 pixels in the diagonal direction
                recPosition.X += 35;         // X component of the vector
                recPosition.Y -= 35;         // Y component of the vector
                //hasDash = false;
            }

            // North West
            if (currentKbState.IsKeyDown(Keys.W) && currentKbState.IsKeyDown(Keys.A) &&
               hasDash && currentKbState.IsKeyDown(Keys.Space) && previousKbState.IsKeyUp(Keys.Space)
               && (recPosition.Y - 50 > 0) && (recPosition.X - 50 > 0))
            {
                // Changes position by 50 pixels in the diagonal direction
                recPosition.X -= 35;         // X component of the vector
                recPosition.Y -= 35;         // Y component of the vector
                //hasDash = false;
            }

            // South East
            if (currentKbState.IsKeyDown(Keys.S) && currentKbState.IsKeyDown(Keys.D) &&
               hasDash && currentKbState.IsKeyDown(Keys.Space) && previousKbState.IsKeyUp(Keys.Space)
               && (recPosition.Y + 50 <= windowHeight - 49) && (recPosition.X + 50 <= windowHeight - 37))
            {
                // Changes position by 50 pixels in the diagonal direction
                recPosition.X += 35;        // X component of the vector
                recPosition.Y += 35;        // Y component of the vector
                //hasDash = false;
            }

            // South West
            if (currentKbState.IsKeyDown(Keys.S) && currentKbState.IsKeyDown(Keys.A) &&
               hasDash && currentKbState.IsKeyDown(Keys.Space) && previousKbState.IsKeyUp(Keys.Space)
               && (recPosition.Y + 50 <= windowHeight - 49) && (recPosition.X - 50 > 0))
            {
                // Changes position by 50 pixels in the diagonal direction
                recPosition.X -= 35;       // X component of the vector
                recPosition.Y += 35;       // Y component of the vector
                //hasDash = false;
            }
            #endregion

            #region // Movement
            // North Movement
            if (currentKbState.IsKeyDown(Keys.W))
            {
                // North Dash
                if (hasDash && (currentKbState.IsKeyDown(Keys.Space) && previousKbState.IsKeyUp(Keys.Space))
                    && (recPosition.Y - 50 > 0))
                {
                    recPosition.Y -= 50;
                    //hasDash = false;
                }

                // Keeps player in window
                if (recPosition.Y > 0)
                {
                    recPosition.Y -= speed;
                }
                else
                {
                    recPosition.Y -= stop;
                }
            }            

            // East Movement
            if (currentKbState.IsKeyDown(Keys.D))
            {
                // East Dash
                if (hasDash && currentKbState.IsKeyDown(Keys.Space) && previousKbState.IsKeyUp(Keys.Space)
                    && (recPosition.X + 50 <= windowHeight - 37))
                {
                    recPosition.X += 50;
                    //hasDash = false;
                }

                // Keeps player in window
                if (recPosition.X <= windowWidth - 37)
                {
                    recPosition.X += speed;
                }
                else
                {
                    recPosition.X -= stop;
                }
            }

            // West Movement
            if (currentKbState.IsKeyDown(Keys.A))
            {
               // West Dash
                if (hasDash && currentKbState.IsKeyDown(Keys.Space) && previousKbState.IsKeyUp(Keys.Space)
                    && (recPosition.X - 50 > 0))
                {
                    recPosition.X -= 50;
                    //hasDash = false;
                }

                // Keeps player in window
                if (recPosition.X > 0)
                {
                    recPosition.X -= speed;
                }
                else
                {
                    recPosition.X -= stop;
                }
            }

            // South Movement
            if (currentKbState.IsKeyDown(Keys.S))
            {
                // South Dash
                if (hasDash && currentKbState.IsKeyDown(Keys.Space) && previousKbState.IsKeyUp(Keys.Space)
                    && (recPosition.Y + 50 <= windowHeight - 49))
                {
                    recPosition.Y += 50;

                }

                // Keeps player in window
                if (recPosition.Y <= windowHeight - 49)
                {
                    recPosition.Y += speed;
                }
                else
                {
                    recPosition.Y -= stop;
                }
            }
            #endregion

            previousKbState = currentKbState;
        }

        /// <summary>
        /// Checks for player collision with any GameObject.
        /// </summary>
        /// <param name="obj">Reference to any GameObject</param>
        /// <returns>True if player is colliding with a GameObject, otherwise false.</returns>
        public override bool IsColliding(GameObject obj)
        {
            if (this.Position.Intersects(obj.Position))
            {
                return true;
            }
           
            return false;
        }
    }
}
