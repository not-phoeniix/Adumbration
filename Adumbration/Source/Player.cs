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
        // Player's input and state
        private KeyboardState previousKbState;
        private PlayerState currentState;

        // Player variables
        private int speed;
        private int dashSpeed;

        // Dashing variables
        private bool hasDash;
        private const float MaxDashTime = 0.5f;
        private float currentDashTime;
        private bool isDashing;

        // Player's previous X and Y positions
        private int prevX;
        private int prevY;

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

            // Set player dash speed
            dashSpeed = speed * 5;

            // Player's current X and Y positions
            int currentX = recPosition.X;
            int currentY = recPosition.Y;

            // Reset timer
            if (!isDashing && currentDashTime != 0)
            {
                currentDashTime = 0;
                //hasDash = false;
            }
            // Increase timer
            else if (isDashing)
            {
                currentDashTime += 0.1f;
            }

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
            if (currentKbState.IsKeyDown(Keys.W) && currentKbState.IsKeyDown(Keys.D) &&                                     // If moving north east
                hasDash && currentKbState.IsKeyDown(Keys.Space))                                                            // and space is pressed
            {
                if (currentDashTime < MaxDashTime)
                {
                    isDashing = true;
                    for (int i = 0; i < (int)(dashSpeed * Math.Cos(45)); i++)
                    {
                        // Changes position by 50 pixels in the diagonal direction
                        recPosition.X += 1;         // X component of the vector
                        recPosition.Y -= 1;         // Y component of the vector
                        //hasDash = false;
                    }
                }
            }

            // North West
            if (currentKbState.IsKeyDown(Keys.W) && currentKbState.IsKeyDown(Keys.A) &&                                    // If moving north west
               hasDash && currentKbState.IsKeyDown(Keys.Space))                                                            // and space is pressed
            {
                if (currentDashTime < MaxDashTime)
                {
                    isDashing = true;
                    for (int i = 0; i < (int)(dashSpeed * Math.Cos(45)); i++)
                    {
                        // Changes position by 50 pixels in the diagonal direction
                        recPosition.X -= 1;         // X component of the vector
                        recPosition.Y -= 1;         // Y component of the vector
                        //hasDash = false;
                    }
                }
            }

            // South East
            if (currentKbState.IsKeyDown(Keys.S) && currentKbState.IsKeyDown(Keys.D) &&                                   // If moving south east
               hasDash && currentKbState.IsKeyDown(Keys.Space))                                                           // and space is pressed
            {
                if (currentDashTime < MaxDashTime)
                {
                    isDashing = true;
                    for (int i = 0; i < (int)(dashSpeed * Math.Cos(45)); i++)
                    {
                        // Changes position by 50 pixels in the diagonal direction
                        recPosition.X += 1;         // X component of the vector
                        recPosition.Y += 1;         // Y component of the vector
                        //hasDash = false;
                    }
                }
            }

            // South West
            if (currentKbState.IsKeyDown(Keys.S) && currentKbState.IsKeyDown(Keys.A) &&                                   // If moving south west
               hasDash && currentKbState.IsKeyDown(Keys.Space))                                                           // and space is pressed                    
            {
                if (currentDashTime < MaxDashTime)
                {
                    isDashing = true;
                    for (int i = 0; i < (int)(dashSpeed * Math.Cos(45)); i++)
                    {
                        // Changes position by 50 pixels in the diagonal direction
                        recPosition.X -= 1;         // X component of the vector
                        recPosition.Y += 1;         // Y component of the vector
                        //hasDash = false;
                    }
                }
            }
            #endregion

            #region // Movement
            // North Movement
            NorthMovement(currentKbState, currentLevel, currentX);

            // East Movement
            EastMovement(currentKbState, currentLevel, currentX, currentY);

            // West Movement
            WestMovement(currentKbState, currentLevel, currentX, currentY);

            // South Movement
            SouthMovement(currentKbState, currentLevel, currentX, currentY);
            #endregion

            //checks to see if the lightbeam collides with the player

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

        public void IsDead(GameObject beam)
        {
            if (this.IsColliding(beam) && !isDashing)
            {
                recPosition.X = 150;
                recPosition.Y = 150;
            }
        }

        /// <summary>
        /// Controls player's movement north
        /// </summary>
        /// <param name="currentKbState">Current state of the keyboard</param>
        /// <param name="currentLevel">Current level the player is on</param>
        /// <param name="currentX">Current X position of player</param>
        private void NorthMovement(KeyboardState currentKbState, Level currentLevel, int currentX)
        {
            if (currentKbState.IsKeyDown(Keys.W))
            {
                // North Dash
                // If the player initates a dash
                if (hasDash && currentKbState.IsKeyDown(Keys.Space))
                {
                    if (currentDashTime < MaxDashTime)
                    {
                        // They're dashing
                        isDashing = true;
                        for (int i = 0; i < dashSpeed; i++)
                        {
                            recPosition.Y -= 1;
                        }
                    }
                }
                // Otherwise they're not
                else
                {
                    isDashing = false;
                }

                // Keeps player in window
                // If player is not touching a top wall let them move in that direction
                recPosition.Y -= speed;

                // While moving in the North direction
                foreach (GameObject tile in currentLevel.TileList)
                {
                    // If it is colliding with a wall
                    if (tile is Wall && IsColliding(tile))
                    {
                        // Snap the Player to the bottom of the wall
                        recPosition.Y = tile.Position.Height + tile.Position.Y;
                        recPosition.X = currentX;
                    }
                }
            }
        }

        /// <summary>
        /// Controls player's movement east
        /// </summary>
        /// <param name="currentKbState">Current state of the keyboard</param>
        /// <param name="currentLevel">Current level the player is on</param>
        /// <param name="currentX">Current X position of player</param>
        /// <param name="currentY">Current Y position of player</param>
        private void EastMovement(KeyboardState currentKbState, Level currentLevel, int currentX, int currentY)
        {
            if (currentKbState.IsKeyDown(Keys.D))
            {
                // East Dash
                // If the player initates a dash
                if (hasDash && currentKbState.IsKeyDown(Keys.Space))
                {
                    if (currentDashTime < MaxDashTime)
                    {
                        // They're dashing
                        isDashing = true;
                        for (int i = 0; i < dashSpeed; i++)
                        {
                            recPosition.X += 1;
                        }
                    }
                }
                // Otherwise they're not
                else
                {
                    isDashing = false;
                }

                // Keeps player in window
                recPosition.X += speed;


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
                        NorthMovement(currentKbState, currentLevel, currentX);
                    }
                }
            }
        }

        /// <summary>
        /// Controls player's movement west
        /// </summary>
        /// <param name="currentKbState">Current state of the keyboard</param>
        /// <param name="currentLevel">Current level the player is on</param>
        /// <param name="currentX">Current X position of player</param>
        /// <param name="currentY">Current Y position of player</param>
        private void WestMovement(KeyboardState currentKbState, Level currentLevel, int currentX, int currentY)
        {
            if (currentKbState.IsKeyDown(Keys.A))
            {
                // West Dash
                // If the player initates a dash
                if (hasDash && currentKbState.IsKeyDown(Keys.Space))
                {
                    if (currentDashTime < MaxDashTime)
                    {
                        // They're dashing
                        isDashing = true;
                        for (int i = 0; i < dashSpeed; i++)
                        {
                            recPosition.X -= 1;
                        }
                    }
                }
                // Otherwise they're not
                else
                {
                    isDashing = false;
                }

                // Keeps player in window
                recPosition.X -= speed;

                // While the player is moving in the West direction 
                foreach (GameObject tile in currentLevel.TileList)
                {
                    // If the player collides with a wall
                    if (tile is Wall && IsColliding(tile))
                    {
                        // Snap the player to the right side of the wall
                        recPosition.X = tile.Position.Width + tile.Position.X;
                        recPosition.Y = currentY;

                        // North Movement
                        NorthMovement(currentKbState, currentLevel, currentX);

                    }
                }
            }
        }

        /// <summary>
        /// Controls player's movement south
        /// </summary>
        /// <param name="currentKbState">Current state of the keyboard</param>
        /// <param name="currentLevel">Current level the player is on</param>
        /// <param name="currentX">Current X position of player</param>
        /// <param name="currentY">Current Y position of player</param>
        private void SouthMovement(KeyboardState currentKbState, Level currentLevel, int currentX, int currentY)
        {
            if (currentKbState.IsKeyDown(Keys.S))
            {
                // South Dash
                // If the player initates a dash
                if (hasDash && currentKbState.IsKeyDown(Keys.Space))
                {
                    if (currentDashTime < MaxDashTime)
                    {
                        // They're dashing
                        isDashing = true;
                        for (int i = 0; i < dashSpeed; i++)
                        {
                            recPosition.Y += 1;
                        }
                    }
                }
                // Otherwise they're not
                else
                {
                    isDashing = false;
                }

                // Move Player Down
                recPosition.Y += speed;

                // While moving in the South direction
                foreach (GameObject tile in currentLevel.TileList)
                {
                    // If the player collides with a wall
                    if (tile is Wall && IsColliding(tile))
                    {
                        // Snap player to the top of the wall
                        recPosition.Y = tile.Position.Y - recPosition.Height;
                        recPosition.X = currentX;

                        // Allow player to move west
                        WestMovement(currentKbState, currentLevel, currentX, currentY);

                        // Allow player to move east
                        EastMovement(currentKbState, currentLevel, currentX, currentY);
                    }
                }
            }
        }
    }
}
