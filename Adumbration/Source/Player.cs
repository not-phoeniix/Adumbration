using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

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
        private List<bool> collectedKeys;

        // Whether player is flipped or not
        private bool playerIsFlipped;

        // Dashing variables
        private bool hasDash;
        private const float MaxDashTime = 0.5f;
        private float currentDashTime;

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

        public List<bool> CollectedKeys
        {
            get{ return collectedKeys; }
            set { collectedKeys = value; }
        }

        public int Speed
        {
            get { return speed; }
        }

        // Constructor
        /// <summary>
        /// Player takes everything from parent class
        /// </summary>
        /// <param name="spritesheet">spritesheet where player's texture is</param>
        /// <param name="sourceRect">The source rectangle within the spritesheet</param>
        /// <param name="position">position of the player</param>
        public Player(Texture2D spritesheet, Rectangle sourceRect, Rectangle position)
            : base(spritesheet, sourceRect, position)
        {
            currentMode = PlayerMode.NormalMode;

            // Set player speed and the collectedKeys array to null
            speed = 2;
            collectedKeys = new List<bool>();

            // Animation data
            fps = 2.0;
            secondsPerFrame = 1.0 / fps;
            timeCounter = 0;
            currentFrame = 1;
            widthOfSingleSprite = 7;
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

            // Player's current X and Y positions
            int currentX = positionRect.X;
            int currentY = positionRect.Y;

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

            MoveMirror(currentLevel, currentKbState);

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
        internal void IsDead(List<LightBeam> beams)
        {
            // When the player collides with a light beam, respawn at starting point
            // This is just for the test room
            if (currentMode == PlayerMode.NormalMode)
            {
                foreach (LightBeam beam in beams)
                {
                    if (this.IsColliding(beam))
                    {
                        LevelManager.Instance.ResetLevel();
                        return;
                    }
                }
            }
        }

        #region// Movement methods
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
                // If player is not touching a top wall let them move in that direction
                positionRect.Y -= speed;
                currentState = PlayerState.MovingUp;

                // While moving in the North direction
                foreach (GameObject tile in currentLevel.TileList)
                {
                    // If it is colliding with a wall
                    if (tile is Wall && IsColliding(tile) && currentMode == PlayerMode.NormalMode)
                    {
                        // Snap the Player to the bottom of the wall
                        positionRect.Y = tile.Position.Height + tile.Position.Y;
                        positionRect.X = currentX;
                    }
                }

                foreach(Mirror mirror in currentLevel.Mirrors)
                {
                    if(IsColliding(mirror) && currentMode == PlayerMode.NormalMode)
                    {
                        positionRect.Y = mirror.Position.Height + mirror.Position.Y;
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
                // Keeps player in window
                positionRect.X += speed;
                currentState = PlayerState.MovingRight;

                // makes player face RIGHT
                playerIsFlipped = false;

                // While moving in the East direction
                foreach (GameObject tile in currentLevel.TileList)
                {
                    // if the player is colliding with a wall
                    if (tile is Wall && IsColliding(tile) && currentMode == PlayerMode.NormalMode)
                    {
                        // Snap Player to the left side of the wall
                        positionRect.X = tile.Position.X - positionRect.Width;
                        positionRect.Y = currentY;

                        // North Movement
                        NorthMovement(currentKbState, currentLevel, currentX);
                    }
                }

                foreach (Mirror mirror in currentLevel.Mirrors)
                {
                    if (IsColliding(mirror) && currentMode == PlayerMode.NormalMode)
                    {
                        positionRect.X = mirror.Position.X - positionRect.Width;
                        positionRect.Y = currentY;

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
                // Keeps player in window
                positionRect.X -= speed;
                currentState = PlayerState.MovingLeft;

                // makes player face LEFT
                playerIsFlipped = true;

                // While the player is moving in the West direction 
                foreach (GameObject tile in currentLevel.TileList)
                {
                    // If the player collides with a wall
                    if (tile is Wall && IsColliding(tile) && currentMode == PlayerMode.NormalMode)
                    {
                        // Snap the player to the right side of the wall
                        positionRect.X = tile.Position.Width + tile.Position.X;
                        positionRect.Y = currentY;

                        // North Movement
                        NorthMovement(currentKbState, currentLevel, currentX);

                    }
                }

                foreach (Mirror mirror in currentLevel.Mirrors)
                {
                    if (IsColliding(mirror) && currentMode == PlayerMode.NormalMode)
                    {
                        positionRect.X = mirror.Position.Width + mirror.Position.X;
                        positionRect.Y = currentY;

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
                // Move Player Down
                positionRect.Y += speed;
                currentState = PlayerState.MovingRight;

                // While moving in the South direction
                foreach (GameObject tile in currentLevel.TileList)
                {
                    // If the player collides with a wall
                    if (tile is Wall && IsColliding(tile) && currentMode == PlayerMode.NormalMode)
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

                foreach (Mirror mirror in currentLevel.Mirrors)
                {
                    if (IsColliding(mirror) && currentMode == PlayerMode.NormalMode)
                    {
                        positionRect.Y = mirror.Position.Y - positionRect.Height;
                        positionRect.X = currentX;

                        // Allow player to move North
                        NorthMovement(currentKbState, currentLevel, currentX);

                        // Allow player to move west
                        WestMovement(currentKbState, currentLevel, currentX, currentY);

                        // Allow player to move east
                        EastMovement(currentKbState, currentLevel, currentX, currentY);
                    }
                }
            }
        }
        #endregion

        #region// Animations
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
                    if (currentFrame >= 3 || currentFrame == 1)
                    {
                        currentFrame = 1;
                        sourceRect.X = 0;
                    }
                    else
                    {
                        currentFrame = currentFrame * widthOfSingleSprite;
                    }
                }
                else if (currentState == PlayerState.MovingUp)
                {
                    if (currentFrame >= 5 || currentFrame == 3)
                    {
                        currentFrame = 3;
                    }
                    else
                    {
                    currentFrame = currentFrame * widthOfSingleSprite;
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

        public void MoveMirror(Level currentLevel, KeyboardState currentKbState)
        {
            foreach(Mirror mirror in currentLevel.Mirrors)
            {
                if (positionRect.Intersects(mirror.Hitbox) && currentKbState.IsKeyDown(Keys.Space))
                {
                    if (currentKbState.IsKeyDown(Keys.W))
                    {
                        mirror.Y -= speed;
                        mirror.HitBoxY -= speed;
                    }

                    if (currentKbState.IsKeyDown(Keys.A))
                    {
                        mirror.X -= speed;
                        mirror.HitBoxX -= speed;
                    }

                    if (currentKbState.IsKeyDown(Keys.S))
                    {
                        mirror.Y += speed;
                        mirror.HitBoxY += speed;
                    }

                    if (currentKbState.IsKeyDown(Keys.D))
                    {
                        mirror.X += speed;
                        mirror.HitBoxX += speed;
                    }
                }               
            }
        }

    }
}