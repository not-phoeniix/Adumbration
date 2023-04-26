using Adumbration.Source.Level;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Adumbration
{
    /// <summary>
    /// The states the player can be in.
    /// </summary>
    public enum PlayerState
    {
        FacingUp,
        FacingDown,
        WalkingLeft,
        WalkingRight
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
        private PlayerState upDownState;
        private PlayerState directionState;

        // Player variables
        private int speed;
        private bool[] collectedKeys;
        private bool isGrabbing;

        // Whether player is flipped or not
        private bool playerIsFlipped;

        // Animation fields
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

        public bool[] CollectedKeys
        {
            get{ return collectedKeys; }
            set { collectedKeys = value; }
        }

        public int Speed
        {
            get { return speed; }
        }

        public bool IsGrabbing
        {
            get { return isGrabbing; }
            set { isGrabbing = value; }
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
            upDownState = PlayerState.FacingDown;

            // Set player speed and the collectedKeys array to null
            speed = 2;
            collectedKeys = new bool[4];

            // Animation data
            fps = 2.0;
            secondsPerFrame = 1.0 / fps;
            timeCounter = 0;
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

            // Every Frame check if the player is grabbing the mirror
            if (isGrabbing)
            {
                speed = 1;
            }
            else
            {
                speed = 2;
            }

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

            // makes player look backward when walking backward
            if(upDownState == PlayerState.FacingUp)
            {
                if (isGrabbing)
                {
                    sourceRect.X = 42;
                }
                else
                {
                    sourceRect.X = 14;
                }   
            }
            else
            {
                if (speed == 1)
                {
                    sourceRect.X = 28;
                }
                else
                {
                    sourceRect.X = 0;
                }
            }

            #region // Movement
            // North Movement
            NorthMovement(currentKbState, currentLevel);

            // East Movement
            EastMovement(currentKbState, currentLevel);

            // West Movement
            WestMovement(currentKbState, currentLevel);

            // South Movement
            SouthMovement(currentKbState, currentLevel);
            #endregion

            //MoveMirror(currentLevel, currentKbState);

            IsDead(currentLevel.Beams, LevelManager.Instance);

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
        private void IsDead(List<LightBeam> beams, LevelManager manager)
        {
            // When the player collides with a light beam, respawn at starting point
            // This is just for the test room
            if (currentMode == PlayerMode.NormalMode)
            {
                foreach (LightBeam beam in beams)
                {
                    if (this.IsColliding(beam))
                    {
                        if (manager.CurrentLevelEnum == GameLevels.Level1)
                        {
                            Debug.WriteLine("CollectedKeys[0] resetted");
                            collectedKeys[0] = false;
                        }

                        if (manager.CurrentLevelEnum == GameLevels.Level2)
                        {
                            collectedKeys[1] = false;
                        }

                        if (manager.CurrentLevelEnum == GameLevels.Level3)
                        {
                            collectedKeys[2] = false;
                        }

                        if (manager.CurrentLevelEnum == GameLevels.Level4)
                        {
                            collectedKeys[3] = false;
                        }

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
        private void NorthMovement(KeyboardState currentKbState, Level currentLevel)
        {
            if (currentKbState.IsKeyDown(Keys.W))
            {
                // If player is not touching a top wall let them move in that direction
                positionRect.Y -= speed;
                upDownState = PlayerState.FacingUp;

                // While moving in the North direction
                foreach (GameObject tile in currentLevel.TileList)
                {
                    // If it is colliding with a wall
                    if (tile is Wall wall && IsColliding(tile) && currentMode == PlayerMode.NormalMode)
                    {
                        if(wall is LevelDoor door)
                        {
                            if(!door.IsOpen)
                            {
                                // Snap the Player to the bottom of the wall
                                positionRect.Y = tile.Position.Height + tile.Position.Y;
                            }
                        }
                        else
                        {
                            // Snap the Player to the bottom of the wall
                            positionRect.Y = tile.Position.Height + tile.Position.Y;
                        }
                    }
                }

                foreach(Mirror mirror in currentLevel.Mirrors)
                {
                    if(IsColliding(mirror) && currentMode == PlayerMode.NormalMode)
                    {
                        positionRect.Y = mirror.Position.Height + mirror.Position.Y;
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
        private void EastMovement(KeyboardState currentKbState, Level currentLevel)
        {
            if (currentKbState.IsKeyDown(Keys.D))
            {
                // Keeps player in window
                positionRect.X += speed;
                directionState = PlayerState.WalkingRight;

                // makes player face RIGHT
                playerIsFlipped = false;

                // While moving in the East direction
                foreach (GameObject tile in currentLevel.TileList)
                {
                    // if the player is colliding with a wall
                    if (tile is Wall wall && IsColliding(tile) && currentMode == PlayerMode.NormalMode)
                    {
                        if(wall is LevelDoor door)
                        {
                            if(!door.IsOpen)
                            {
                                // Snap Player to the left side of the wall
                                positionRect.X = tile.Position.X - positionRect.Width;

                                // North Movement
                                NorthMovement(currentKbState, currentLevel);
                            }
                        }
                        else
                        {
                            // Snap Player to the left side of the wall
                            positionRect.X = tile.Position.X - positionRect.Width;

                            // North Movement
                            NorthMovement(currentKbState, currentLevel);
                        }
                    }
                }

                foreach (Mirror mirror in currentLevel.Mirrors)
                {
                    if (IsColliding(mirror) && currentMode == PlayerMode.NormalMode)
                    {
                        positionRect.X = mirror.Position.X - positionRect.Width;

                        NorthMovement(currentKbState, currentLevel);
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
        private void WestMovement(KeyboardState currentKbState, Level currentLevel)
        {
            if (currentKbState.IsKeyDown(Keys.A))
            {
                // Keeps player in window
                positionRect.X -= speed;
                directionState = PlayerState.WalkingLeft;

                // makes player face LEFT
                playerIsFlipped = true;

                // While the player is moving in the West direction 
                foreach (GameObject tile in currentLevel.TileList)
                {
                    // If the player collides with a wall
                    if (tile is Wall wall && IsColliding(tile) && currentMode == PlayerMode.NormalMode)
                    {
                        if(wall is LevelDoor door)
                        {
                            if(!door.IsOpen)
                            {
                                // Snap the player to the right side of the wall
                                positionRect.X = tile.Position.Width + tile.Position.X;

                                // North Movement
                                NorthMovement(currentKbState, currentLevel);
                            }
                        }
                        else
                        {
                            // Snap the player to the right side of the wall
                            positionRect.X = tile.Position.Width + tile.Position.X;

                            // North Movement
                            NorthMovement(currentKbState, currentLevel);
                        }
                    }
                }

                foreach (Mirror mirror in currentLevel.Mirrors)
                {
                    if (IsColliding(mirror) && currentMode == PlayerMode.NormalMode)
                    {
                        positionRect.X = mirror.Position.Width + mirror.Position.X;

                        NorthMovement(currentKbState, currentLevel);
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
        private void SouthMovement(KeyboardState currentKbState, Level currentLevel)
        {
            if (currentKbState.IsKeyDown(Keys.S))
            {
                // Move Player Down
                positionRect.Y += speed;
                upDownState = PlayerState.FacingDown;

                // While moving in the South direction
                foreach (GameObject tile in currentLevel.TileList)
                {
                    // If the player collides with a wall
                    if (tile is Wall wall && IsColliding(tile) && currentMode == PlayerMode.NormalMode)
                    {
                        if(wall is LevelDoor door)
                        {
                            if(!door.IsOpen)
                            {
                                // Snap player to the top of the wall
                                positionRect.Y = tile.Position.Y - positionRect.Height;

                                // Allow player to move west
                                WestMovement(currentKbState, currentLevel);

                                // Allow player to move east
                                EastMovement(currentKbState, currentLevel);
                            }
                        }
                        else
                        {
                            // Snap player to the top of the wall
                            positionRect.Y = tile.Position.Y - positionRect.Height;

                            // Allow player to move west
                            WestMovement(currentKbState, currentLevel);

                            // Allow player to move east
                            EastMovement(currentKbState, currentLevel);
                        }
                    }
                }

                foreach (Mirror mirror in currentLevel.Mirrors)
                {
                    if (IsColliding(mirror) && currentMode == PlayerMode.NormalMode)
                    {
                        positionRect.Y = mirror.Position.Y - positionRect.Height;

                        // Allow player to move North
                        NorthMovement(currentKbState, currentLevel);

                        // Allow player to move west
                        WestMovement(currentKbState, currentLevel);

                        // Allow player to move east
                        EastMovement(currentKbState, currentLevel);
                    }
                }
            }
        }
        #endregion

        /// <summary>
        /// resets all keys
        /// </summary>
        public void ResetKeys()
        {
            collectedKeys[0] = false;
            collectedKeys[1] = false;
            collectedKeys[2] = false;
            collectedKeys[3] = false;
        }
    }
}