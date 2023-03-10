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
    /// The states the player can be in.
    /// </summary>
    public enum PlayerState
    {
        FacingRight,
        MovingRight,
        FacingLeft,
        MovingLeft
    }

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
        private int prevX;
        private int prevY;
        private PlayerState currentState;

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
        public void Update(GameTime gameTime, Level currentLevel)
        {    
            // Player input
            KeyboardState currentKbState = Keyboard.GetState();            

            // Set player speed
            speed = 5;

            // Player's current X and Y positions
            int currentX = recPosition.X;
            int currentY = recPosition.Y;

            #region// Keeping this in case we need to go back to it
            //foreach (GameObject tile in currentLevel.TileList)
            //{
            //    // If the player is touching a wall
            //    if (tile is Wall && recPosition.Intersects(tile.Position))
            //    {
            //        // They're touching a wall
            //        isTouchingWall = true;
            //    }
            //    // Otherwise
            //    if (tile is Floor && recPosition.Intersects(tile.Position))
            //    {
            //        // They're not
            //        isTouchingWall = false;
            //    }
            //}
            #endregion

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
            NorthMovement(currentKbState, currentLevel, currentX);

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

                // While moving in the East direction
                foreach (GameObject tile in currentLevel.TileList)
                {
                    // if the player is colliding with a wall
                    if (tile is Wall && IsColliding(tile))
                    {
                        // Snap Player to the left side of the wall
                        recPosition.X = tile.Position.X - recPosition.Width;
                        recPosition.Y = currentY;

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
                            // If player is not touching a top wall let them move in that direction
                            if (recPosition.Y > 0)
                            {
                                recPosition.Y -= speed;
                            }

                            // While moving in the North direction
                            foreach (GameObject tile2 in currentLevel.TileList)
                            {
                                // If it is colliding with a wall
                                if (tile2 is Wall && IsColliding(tile2))
                                {
                                    // Snap the Player to the bottom of the wall
                                    recPosition.Y = tile.Position.Height;
                                    recPosition.X = currentX;
                                }
                            }

                        }
                    }
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

                // While the player is moving in the West direction 
                foreach (GameObject tile in currentLevel.TileList)
                {
                    // If the player collides with a wall
                    if (tile is Wall && IsColliding(tile))
                    {
                        // Snap the player to the right side of the wall
                        recPosition.X = tile.Position.Width;
                        recPosition.Y = currentY;

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
                            // If player is not touching a top wall let them move in that direction
                            if (recPosition.Y > 0)
                            {
                                recPosition.Y -= speed;
                            }

                            // While moving in the North direction
                            foreach (GameObject tile2 in currentLevel.TileList)
                            {
                                // If it is colliding with a wall
                                if (tile2 is Wall && IsColliding(tile2))
                                {
                                    // Snap the Player to the bottom of the wall
                                    recPosition.Y = tile.Position.Height;
                                    recPosition.X = currentX;
                                }
                            }

                        }
                    }
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

                // While moving in the South direction
                foreach (GameObject tile in currentLevel.TileList)
                {
                    // If the player collides with a wall
                    if (tile is Wall && IsColliding(tile))
                    {
                        // Snap player to the top of the wall
                        recPosition.Y = tile.Position.Y - recPosition.Height;
                        recPosition.X = currentX;

                        if (currentKbState.IsKeyDown(Keys.A))
                        {
                            recPosition.X -= speed;

                        }

                        if (currentKbState.IsKeyDown(Keys.D))
                        {
                            recPosition.X += speed;
                        }
                    }
                }
            }
            #endregion

            previousKbState = currentKbState;

            // In case we need to use them keep them here
            prevX = currentX;
            prevY = currentY;
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

        private void NorthMovement(KeyboardState currentKbState, Level currentLevel, int currentX)
        {
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
                // If player is not touching a top wall let them move in that direction
                if (recPosition.Y > 0)
                {
                    recPosition.Y -= speed;
                }

                // While moving in the North direction
                foreach (GameObject tile in currentLevel.TileList)
                {
                    // If it is colliding with a wall
                    if (tile is Wall && IsColliding(tile))
                    {
                        // Snap the Player to the bottom of the wall
                        recPosition.Y = tile.Position.Height;
                        recPosition.X = currentX;
                    }
                }

            }
        }
    }
}
