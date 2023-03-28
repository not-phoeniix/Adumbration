using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

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
        MovingLeft,
        FacingUp,
        MovingUp
    }

    /// <summary>
    /// The states for the player's mode
    /// </summary>
    public enum PlayerMode
    {
        NormalMode,
        GodMode
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

        // Whether player is flipped or not
        private bool playerIsFlipped;

        // Dashing variables
        private bool hasDash;
        private const float MaxDashTime = 0.2f;
        private float currentDashTime;
        private bool isDashing;

        // Player's previous X and Y positions
        private int prevX;
        private int prevY;

        // Animation fields
        private int widthOfSingleSprite;
        private int currentFrame;
        private double fps;
        private double secondsPerFrame;
        private double timeCounter;

        // Position centered in screen
        public Rectangle CenterRect { get; set; }

        //to turn on god mode in this game
        private PlayerMode currentMode;

        // Properties
        /// <summary>
        /// Get property for whether the player has a dash or not
        /// </summary>
        public bool HasDash
        {
            get { return hasDash; }
        }

        public Vector2 CenterPos
        {
            get
            {
                return new Vector2(
                    positionRect.X + positionRect.Width / 2,
                    positionRect.Y + positionRect.Height / 2
                    );
            }
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
            currentMode = PlayerMode.NormalMode;

            // Animation data
            fps = 2.0;
            secondsPerFrame = 1.0 / fps;
            timeCounter = 0;
            currentFrame = 1;
            //widthOfSingleSprite = spriteSheet.Width;
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

            if (currentKbState.IsKeyDown(Keys.F12) && previousKbState.IsKeyUp(Keys.F12))
            {
                currentMode = PlayerMode.GodMode;
                System.Diagnostics.Debug.WriteLine("god mode");
            }
            else if (currentKbState.IsKeyDown(Keys.F11) && previousKbState.IsKeyUp(Keys.F11))
            {
                currentMode = PlayerMode.NormalMode;
                System.Diagnostics.Debug.WriteLine("normal mode");
            }

            // Player States
            switch (currentState)
            {
                case PlayerState.FacingLeft:
                    {
                        currentFrame = 1;
                        sourceRect.X = 0;

                        // If W is pressed, face up
                        if (currentKbState.IsKeyDown(Keys.W))
                        {
                            currentState = PlayerState.FacingUp;
                        }

                        // If A is pressed, move left
                        else if (currentKbState.IsKeyDown(Keys.A))
                        {
                            currentState = PlayerState.MovingLeft;
                        }

                        // If S is pressed, move down with MovingLeft animation
                        else if (currentKbState.IsKeyDown(Keys.S))
                        {
                            currentState = PlayerState.MovingLeft;
                        }

                        // If D is pressed, face right
                        else if (currentKbState.IsKeyDown(Keys.D))
                        {
                            currentState = PlayerState.FacingRight;
                        }
                        UpdateAnimation(gameTime);
                        break;
                    }

                case PlayerState.MovingLeft:
                    {
                        currentFrame = 1;
                        sourceRect.X = 0;

                        // If W is pressed, move up
                        if (currentKbState.IsKeyDown(Keys.W))
                        {
                            currentState = PlayerState.MovingUp;
                        }

                        // If A is pressed, move left
                        else if (currentKbState.IsKeyDown(Keys.A))
                        {
                            currentState = PlayerState.MovingLeft;
                        }

                        // If S is pressed, move down with MovingLeft animation
                        else if (currentKbState.IsKeyDown(Keys.S))
                        {
                            currentState = PlayerState.MovingLeft;
                        }

                        // If D is pressed, face right
                        else if (currentKbState.IsKeyDown(Keys.D))
                        {
                            currentState = PlayerState.FacingRight;
                        }

                        // If nothing is pressed, face left
                        else
                        {
                            currentState = PlayerState.FacingLeft;
                        }
                        UpdateAnimation(gameTime);
                        break;
                    }

                case PlayerState.FacingRight:
                    {
                        currentFrame = 1;
                        sourceRect.X = 0;

                        // If W is pressed, face up
                        if (currentKbState.IsKeyDown(Keys.W))
                        {
                            currentState = PlayerState.FacingUp;
                        }

                        // If A is pressed, face left
                        else if (currentKbState.IsKeyDown(Keys.A))
                        {
                            currentState = PlayerState.FacingLeft;
                        }

                        // If S is pressed, move down with MovingRight animation
                        else if (currentKbState.IsKeyDown(Keys.S))
                        {
                            currentState = PlayerState.MovingRight;
                        }

                        // If D is pressed, face right
                        else if (currentKbState.IsKeyDown(Keys.D))
                        {
                            currentState = PlayerState.MovingRight;
                        }
                        UpdateAnimation(gameTime);
                        break;
                    }

                case PlayerState.MovingRight:
                    {
                        currentFrame = 1;
                        sourceRect.X = 0;

                        // If W is pressed, move up
                        if (currentKbState.IsKeyDown(Keys.W))
                        {
                            currentState = PlayerState.MovingUp;
                        }

                        // If A is pressed, face left
                        else if (currentKbState.IsKeyDown(Keys.A))
                        {
                            currentState = PlayerState.FacingLeft;
                        }

                        // If S is pressed, move down with MovingRight animation
                        else if (currentKbState.IsKeyDown(Keys.S))
                        {
                            currentState = PlayerState.MovingRight;
                        }

                        // If D is pressed, move right
                        else if (currentKbState.IsKeyDown(Keys.D))
                        {
                            currentState = PlayerState.MovingRight;
                        }

                        // If nothing is pressed, face right
                        else
                        {
                            currentState = PlayerState.FacingRight;
                        }
                        UpdateAnimation(gameTime);
                        break;
                    }

                case PlayerState.FacingUp:
                    {
                        currentFrame = 3;
                        sourceRect.X = 14;

                        // If W is pressed, move up
                        if (currentKbState.IsKeyDown(Keys.W))
                        {
                            currentState = PlayerState.MovingUp;
                        }

                        // If A is pressed, move left with MovingUp animation
                        else if (currentKbState.IsKeyDown(Keys.A))
                        {
                            currentState = PlayerState.MovingUp;
                        }

                        // If S is pressed, face right
                        else if (currentKbState.IsKeyDown(Keys.S))
                        {
                            currentState = PlayerState.FacingRight;
                        }

                        // If D is pressed, move right with MovingUp animation
                        else if (currentKbState.IsKeyDown(Keys.D))
                        {
                            currentState = PlayerState.MovingUp;
                        }

                        // If nothing is pressed, continue facing up
                        else
                        {
                            currentState = PlayerState.FacingUp;
                        }
                        UpdateAnimation(gameTime);
                        break;
                    }

                case PlayerState.MovingUp:
                    {
                        currentFrame = 3;
                        sourceRect.X = 14;

                        // If W is pressed, move up
                        if (currentKbState.IsKeyDown(Keys.W))
                        {
                            currentState = PlayerState.MovingUp;
                        }

                        // If A is pressed, move left with MovingUp animation
                        else if (currentKbState.IsKeyDown(Keys.A))
                        {
                            currentState = PlayerState.MovingUp;
                        }

                        // If S is pressed, move down with MovingRight animation
                        else if (currentKbState.IsKeyDown(Keys.S))
                        {
                            currentState = PlayerState.MovingRight;
                        }

                        // If D is pressed, move right with MovingUp animation
                        else if (currentKbState.IsKeyDown(Keys.D))
                        {
                            currentState = PlayerState.MovingUp;
                        }

                        // If nothing is pressed, face up
                        else
                        {
                            currentState = PlayerState.FacingUp;
                        }
                        UpdateAnimation(gameTime);
                        break;
                    }
            }

            // Player Modes
            switch (currentMode)
            {
                case PlayerMode.NormalMode:
                    {
                        // Set player speed
                        speed = 2;

                        // Set player dash speed
                        dashSpeed = speed * 5;

                        // Player's current X and Y positions
                        int currentX = positionRect.X;
                        int currentY = positionRect.Y;

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
                                    positionRect.X += 1;         // X component of the vector
                                    positionRect.Y -= 1;         // Y component of the vector
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
                                    positionRect.X -= 1;         // X component of the vector
                                    positionRect.Y -= 1;         // Y component of the vector
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
                                    positionRect.X += 1;         // X component of the vector
                                    positionRect.Y += 1;         // Y component of the vector
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
                                    positionRect.X -= 1;         // X component of the vector
                                    positionRect.Y += 1;         // Y component of the vector
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



                        // In case we need to use them keep them here
                        prevX = currentX;
                        prevY = currentY;
                        break;
                    }
                case PlayerMode.GodMode:
                    {
                        // Set player speed
                        speed = 5;

                        // Set player dash speed
                        dashSpeed = speed * 5;

                        // Player's current X and Y positions
                        int currentX = positionRect.X;
                        int currentY = positionRect.Y;

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
                                    positionRect.X += 1;         // X component of the vector
                                    positionRect.Y -= 1;         // Y component of the vector
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
                                    positionRect.X -= 1;         // X component of the vector
                                    positionRect.Y -= 1;         // Y component of the vector
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
                                    positionRect.X += 1;         // X component of the vector
                                    positionRect.Y += 1;         // Y component of the vector
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
                                    positionRect.X -= 1;         // X component of the vector
                                    positionRect.Y += 1;         // Y component of the vector
                                                                 //hasDash = false;
                                }
                            }
                        }
                        #endregion


                        #region//all movements inside the godmode that allows the user to leave the stage and explore the entire window
                        GodNorthMove(currentKbState, currentLevel, currentX);

                        GodEastMove(currentKbState, currentLevel, currentX, currentY);

                        GodWestMove(currentKbState, currentLevel, currentX, currentY);

                        GodSouthMove(currentKbState, currentLevel, currentX, currentY);
                        #endregion

                        break;
                    }
            }
            previousKbState = currentKbState;
        }

        /// <summary>
        /// Draws the player normally according to internal position.
        /// </summary>
        /// <param name="sb">SpriteBatch object to draw with.</param>
        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(
                spriteSheet,
                positionRect,
                sourceRect,
                Color.White,
                0,
                new Vector2(0, 0),
                playerIsFlipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                0);
        }

        /// <summary>
        /// Checks for player collision with any GameObject.
        /// </summary>
        /// <param name="obj">Reference to any GameObject</param>
        /// <returns>True if player is colliding with a GameObject, otherwise false.</returns>
        public override bool IsColliding(GameObject obj)
        {
            // As long as the objects are colliding, return true
            if (this.Position.Intersects(obj.Position))
            {
                return true;
            }

            // When the collision ends, return false
            return false;
        }

        /// <summary>
        /// If the player is killed, it will respawn at the start of the room.
        /// </summary>
        /// <param name="beam">The light beam.</param>
        public void IsDead(GameObject beam)
        {
            // When the player collides with a light beam, respawn at starting point
            // This is just for the test room
            if (this.IsColliding(beam) && !isDashing)
            {
                positionRect.X = 50;
                positionRect.Y = 50;
            }
        }


        #region//all normal mode movement methods

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
                        positionRect.Y -= 1;
                    }
                }
                // Otherwise they're not
                else
                {
                    isDashing = false;
                }

                // Keeps player in window
                // If player is not touching a top wall let them move in that direction
                positionRect.Y -= speed;

                // While moving in the North direction
                foreach (GameObject tile in currentLevel.TileList)
                {
                    // If it is colliding with a wall
                    if (tile is Wall && IsColliding(tile))
                    {
                        // Snap the Player to the bottom of the wall
                        positionRect.Y = tile.Position.Height + tile.Position.Y;
                        positionRect.X = currentX;
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

                        positionRect.X += 1;

                    }
                }
                // Otherwise they're not
                else
                {
                    isDashing = false;
                }

                // Keeps player in window
                positionRect.X += speed;

                // makes player face RIGHT
                playerIsFlipped = false;

                // While moving in the East direction
                foreach (GameObject tile in currentLevel.TileList)
                {
                    // if the player is colliding with a wall
                    if (tile is Wall && IsColliding(tile))
                    {
                        // Snap Player to the left side of the wall
                        positionRect.X = tile.Position.X - positionRect.Width;
                        positionRect.Y = currentY;

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

                        positionRect.X -= 1;

                    }
                }
                // Otherwise they're not
                else
                {
                    isDashing = false;
                }

                // Keeps player in window
                positionRect.X -= speed;

                // makes player face LEFT
                playerIsFlipped = true;

                // While the player is moving in the West direction 
                foreach (GameObject tile in currentLevel.TileList)
                {
                    // If the player collides with a wall
                    if (tile is Wall && IsColliding(tile))
                    {
                        // Snap the player to the right side of the wall
                        positionRect.X = tile.Position.Width + tile.Position.X;
                        positionRect.Y = currentY;

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

                        positionRect.Y += 1;

                    }
                }
                // Otherwise they're not
                else
                {
                    isDashing = false;
                }

                // Move Player Down
                positionRect.Y += speed;

                // While moving in the South direction
                foreach (GameObject tile in currentLevel.TileList)
                {
                    // If the player collides with a wall
                    if (tile is Wall && IsColliding(tile))
                    {
                        // Snap player to the top of the wall
                        positionRect.Y = tile.Position.Y - positionRect.Height;
                        positionRect.X = currentX;

                        // Allow player to move west
                        WestMovement(currentKbState, currentLevel, currentX, currentY);

                        // Allow player to move east
                        EastMovement(currentKbState, currentLevel, currentX, currentY);
                    }
                }
            }
        }
        #endregion

        #region//all god mode movement methods

        /// <summary>
        /// this is just an extra method to not get normal mode and god mode movement mixed up
        /// </summary>
        /// <param name="currentKbState"></param>
        /// <param name="currentLevel"></param>
        /// <param name="currentX"></param>
        private void GodNorthMove(KeyboardState currentKbState, Level currentLevel, int currentX)
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
                            positionRect.Y -= 1;
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
                positionRect.Y -= speed;
            }
        }


        /// <summary>
        /// this is the god mode version of the east movement to ignore the walls
        /// </summary>
        /// <param name="currentKbState"></param>
        /// <param name="currentLevel"></param>
        /// <param name="currentX"></param>
        /// <param name="currentY"></param>
        private void GodEastMove(KeyboardState currentKbState, Level currentLevel, int currentX, int currentY)
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
                            positionRect.X += 1;
                        }
                    }
                }
                // Otherwise they're not
                else
                {
                    isDashing = false;
                }

                // Keeps player in window
                positionRect.X += speed;

                // makes player face RIGHT
                playerIsFlipped = false;


            }
        }

        /// <summary>
        /// god mode version of the west movement that ignores walls
        /// </summary>
        /// <param name="currentKbState"></param>
        /// <param name="currentLevel"></param>
        /// <param name="currentX"></param>
        /// <param name="currentY"></param>
        private void GodWestMove(KeyboardState currentKbState, Level currentLevel, int currentX, int currentY)
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
                            positionRect.X -= 1;
                        }
                    }
                }
                // Otherwise they're not
                else
                {
                    isDashing = false;
                }

                // Keeps player in window
                positionRect.X -= speed;

                // makes player face LEFT
                playerIsFlipped = true;


            }
        }

        /// <summary>
        /// god mode version of the south movement that ignores walls
        /// </summary>
        /// <param name="currentKbState"></param>
        /// <param name="currentLevel"></param>
        /// <param name="currentX"></param>
        /// <param name="currentY"></param>
        private void GodSouthMove(KeyboardState currentKbState, Level currentLevel, int currentX, int currentY)
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
                            positionRect.Y += 1;
                        }
                    }
                }
                // Otherwise they're not
                else
                {
                    isDashing = false;
                }

                // Move Player Down
                positionRect.Y += speed;
            }
        }
        #endregion

        #region//Animations
        /// <summary>
        /// Helper for updating player's animation based on time.
        /// </summary>
        /// <param name="gameTime">Info about time from MonoGame.</param>
        private void UpdateAnimation(GameTime gameTime)
        {
            // Increment the time
            timeCounter += gameTime.ElapsedGameTime.TotalSeconds;

            // 
            if (timeCounter >= secondsPerFrame)
            {
                // Change which frame is active, ensuring the frame is reset back to the first frame in the animation
                currentFrame++;
                if (currentState == PlayerState.MovingLeft || currentState == PlayerState.MovingRight)
                {
                    if (currentFrame >= 3)
                    {
                        currentFrame = 1;
                    }
                }
                else if (currentState == PlayerState.MovingUp)
                {
                    if (currentFrame >= 5)
                    {
                        currentFrame = 3;
                    }
                }

                // Reset time counter
                timeCounter -= secondsPerFrame;
            }
        }

        /// <summary>
        /// Helper method to animate the player's motion.
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="flip">Should he be flipped horizontally.</param>
        private void DrawMotion(SpriteBatch sb, SpriteEffects flip)
        {
            sb.Draw(
                spriteSheet,
                positionRect,
                new Rectangle(
                    currentFrame * widthOfSingleSprite,
                    0,
                    widthOfSingleSprite,
                    spriteSheet.Height),
                Color.White,
                0.0f,
                Vector2.Zero,
                flip,
                0.0f);
        }

        /// <summary>
        /// Helper method to draw player in standing position. Player is not animated.
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="flip">Should be able to flip horizontally.</param>
        private void DrawStanding(SpriteBatch sb, SpriteEffects flip)
        {
            if (currentState == PlayerState.FacingLeft || currentState == PlayerState.FacingRight)
            {
                sb.Draw(
                    spriteSheet,
                    positionRect,
                    sourceRect,
                    Color.White,
                    0.0f,
                    Vector2.Zero,
                    flip,
                    0.0f);
            }
            else if (currentState == PlayerState.FacingUp)
            {
                sb.Draw(
                    spriteSheet,
                    positionRect,
                    new Rectangle(
                        14,
                        0,
                        widthOfSingleSprite,
                        spriteSheet.Height),
                    Color.White,
                    0.0f,
                    Vector2.Zero,
                    flip,
                    0.0f);
            }
        }

        #endregion
    }
}
