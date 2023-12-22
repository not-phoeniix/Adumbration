using Adumbration.Source.Level;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct2D1.Effects;
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
        private float speed;
        private bool[] collectedKeys;
        private bool isGrabbing;

        // Vector variables
        private Vector2 position;
        private Vector2 direction;
        private Vector2 velocity;

        // audio
        private SoundEffectInstance deathSound;

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

        public Vector2 Velocity
        {
            get { return velocity; }
        }

        public bool[] CollectedKeys
        {
            get { return collectedKeys; }
            set { collectedKeys = value; }
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
        /// <param name="rectPosition">position of the player's rectangle</param>
        public Player(Texture2D spritesheet, SoundEffect deathSound, Rectangle sourceRect, Rectangle rectPosition)
            : base(spritesheet, sourceRect, rectPosition)
        {
            currentMode = PlayerMode.NormalMode;
            upDownState = PlayerState.FacingDown;

            // Set player speed and the collectedKeys array to null
            speed = 2f;
            collectedKeys = new bool[4];

            // Animation data
            fps = 2.0;
            secondsPerFrame = 1.0 / fps;
            timeCounter = 0;

            // set up sound effects
            this.deathSound = deathSound.CreateInstance();
            this.deathSound.Volume = 0.8f;

            // Set Player's position to be the same as the rectangle's position
            position = new Vector2(rectPosition.X, rectPosition.Y);
            direction = Vector2.Zero;
            velocity = Vector2.Zero;
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

            #region === God Mode stuff ===
            if (currentKbState.IsKeyDown(Keys.F12) && previousKbState.IsKeyUp(Keys.F12))
            {
                currentMode = PlayerMode.GodMode;
            }
            else if (currentKbState.IsKeyDown(Keys.F11) && previousKbState.IsKeyUp(Keys.F11))
            {
                currentMode = PlayerMode.NormalMode;
            }
            #endregion

            //If any of the movement keys are not pressed down set direction to zero;
            direction = Vector2.Zero;

            // set the player's position Vector as it's rect position
            position.X = this.X;
            position.Y = this.Y;

            #region === Player Input ===
            // If W is pressed Direction points up
            if (currentKbState.IsKeyDown(Keys.W))
            {
                direction -= Vector2.UnitY;
                upDownState = PlayerState.FacingUp;
            }

            // If A is pressed Direction points Left
            if (currentKbState.IsKeyDown(Keys.A))
            {
                direction -= Vector2.UnitX;
                directionState = PlayerState.WalkingLeft;
            }

            // If S is pressed Direction points down
            if (currentKbState.IsKeyDown(Keys.S))
            {
                direction += Vector2.UnitY;
                upDownState = PlayerState.FacingDown;
            }

            // If D is pressed Direction points right
            if (currentKbState.IsKeyDown(Keys.D))
            {
                direction += Vector2.UnitX;
                directionState = PlayerState.WalkingRight;
            }
            #endregion

            // Every Frame check if the player is grabbing the mirror
            // If so halve the speed
            if (isGrabbing)
            {
                speed = 1;
            }
            else
            {
                speed = 2;
            }

            // makes player look backward when walking backward
            if (upDownState == PlayerState.FacingUp)
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

            #region ==== Movement ====

            // Normalize the Direction vector to maintain consistency
            if (direction != Vector2.Zero)
            {
                direction.Normalize();
            }

            // Calculate Velocity
            velocity = direction * speed;

            // Apply velocity to position
            position += velocity;

            #region === Collision Detection and Response ===

            // For every tile in the level
            foreach (GameObject tile in currentLevel.TileList)
            {
                // If it is colliding with a wall
                if (tile is Wall wall && currentMode == PlayerMode.NormalMode)
                {
                    // for non-opened wall doors
                    if (wall is LevelDoor door)
                    {
                        if (!door.IsOpen)
                        {
                            // Depending on which side of a wall you are touching 
                            // Snap player accordingly
                            if (IsTouchingLeft(tile))
                            {
                                position.X = tile.X - positionRect.Width;
                            }

                            if (IsTouchingRight(tile))
                            {
                                position.X = tile.X + tile.Width;
                            }

                            if (IsTouchingTop(tile))
                            {
                                position.Y = tile.Y - positionRect.Height;
                            }

                            if (IsTouchingBottom(tile))
                            {
                                position.Y = tile.Y + tile.Height;
                            }
                        }
                    }
                    // Otherwise for regular walls do the same
                    else
                    {
                        // Depending on which side of a wall you are touching 
                        // Snap player accordingly
                        if (IsTouchingLeft(tile))
                        {
                            position.X = tile.X - positionRect.Width;
                        }

                        if (IsTouchingRight(tile))
                        {
                            position.X = tile.X + tile.Width;
                        }

                        if (IsTouchingTop(tile))
                        {
                            position.Y = tile.Y - positionRect.Height;
                        }

                        if (IsTouchingBottom(tile))
                        {
                            position.Y = tile.Y + tile.Height;
                        }
                    }
                }
            }

            // Same Collision Detection response for mirrors
            foreach (Mirror mirror in currentLevel.Mirrors)
            {
                if (currentMode == PlayerMode.NormalMode)
                {
                    // Depending on which side of a wall you are touching 
                    // Snap player accordingly
                    if (IsTouchingLeft(mirror))
                    {
                        position.X = mirror.X - positionRect.Width;
                    }

                    if (IsTouchingRight(mirror))
                    {
                        position.X = mirror.X + mirror.Width;
                    }

                    if (IsTouchingTop(mirror))
                    {
                        position.Y = mirror.Y - positionRect.Height;
                    }

                    if (IsTouchingBottom(mirror))
                    {
                        position.Y = mirror.Y + mirror.Height;
                    }
                }
            }
            #endregion

            // Change position of the rectangle accordingly
            positionRect.X = (int)position.X;
            positionRect.Y = (int)position.Y;

            #endregion

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
                        deathSound.Play();
                        return;
                    }
                }
            }
        }

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

        #region === Collision Methods ===
        /// <summary>
        /// Determine whether player is touch the left side of an obstacle
        /// </summary>
        /// <param name="obst">The obstacle that is being checked</param>
        /// <returns>Whether or not the player is touching the left side</returns>
        private bool IsTouchingLeft(GameObject obst)
        {
            return positionRect.X + positionRect.Width + velocity.X > obst.X &&
                positionRect.X < obst.Position.X &&
                positionRect.Y + positionRect.Height > obst.Y &&
                positionRect.Y < obst.Y + obst.Height;
        }

        /// <summary>
        /// Determine whether player is touch the right side of an obstacle
        /// </summary>
        /// <param name="obst">The obstacle that is being checked</param>
        /// <returns>Whether or not the player is touching the right side</returns>
        private bool IsTouchingRight(GameObject obst)
        {
            return positionRect.X + velocity.X < obst.X + obst.Width &&
                positionRect.X + positionRect.Width > obst.X + obst.Width &&
                positionRect.Y + positionRect.Height > obst.Y &&
                positionRect.Y < obst.Y + obst.Height;
        }

        /// <summary>
        /// Determine whether player is touch the top side of an obstacle
        /// </summary>
        /// <param name="obst">The obstacle that is being checked</param>
        /// <returns>Whether or not the player is touching the top side</returns>
        private bool IsTouchingTop(GameObject obst)
        {
            return positionRect.Y + positionRect.Height + velocity.Y > obst.Y &&
                positionRect.Y < obst.Position.Y &&
                positionRect.X + positionRect.Width > obst.X &&
                positionRect.X < obst.X + obst.Width;
        }

        /// <summary>
        /// Determine whether player is touch the bottom side of an obstacle
        /// </summary>
        /// <param name="obst">The obstacle that is being checked</param>
        /// <returns>Whether or not the player is touching the bottom side</returns>
        private bool IsTouchingBottom(GameObject obst)
        {
            return positionRect.Y + velocity.Y < obst.Y + obst.Height &&
                positionRect.Y + positionRect.Height > obst.Y + obst.Height &&
                positionRect.X + positionRect.Width > obst.X &&
                positionRect.X < obst.X + obst.Width;
        }
        #endregion
    }
}