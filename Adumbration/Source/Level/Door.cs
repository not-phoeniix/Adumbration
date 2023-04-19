using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.XAudio2;
using Penumbra;
using System.Collections.Generic;

namespace Adumbration
{
    #region// Delegate(s)

    /// <summary>
    /// Delegate for methods of the bool return type and take the parameters
    /// Keys, KeyboardState, and KeyboardState.
    /// </summary>
    /// <param name="keys">Key that is pressed.</param>
    /// <param name="current">Current key being pressed.</param>
    /// <param name="previous">Previous key that was pressed.</param>
    /// <returns>True or false for all methods tied to this delegate's event(s).</returns>
    public delegate bool KeyPressOnceDelegate(Keys keys, KeyboardState current, KeyboardState previous);

    /// <summary>
    /// Delegate for methods of the void return type and take the Player parameter
    /// </summary>
    /// <param name="player">Reference to the the player game object</param>
    public delegate void KeyPressDelegate(Player player, KeyboardState current, KeyboardState previous);
    #endregion

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
        private Rectangle unlockHitbox;
        private Rectangle hitbox;
        private KeyboardState previousState;
        private int level;
        private Dictionary<string, Texture2D> textureDict;

        #region// Event(s)

        /// <summary>
        /// Tie the methods for if a key is pressed once here.
        /// </summary>
        public event KeyPressOnceDelegate OnKeyPressOnce;

        /// <summary>
        /// Tie the methods for if a key is pressed here.
        /// </summary>
        public event KeyPressDelegate OnKeyPress;
        #endregion

        // Property
        public Rectangle Hitbox
        {
            get { return hitbox; }
        }

        public Rectangle UnlockHitbox
        {
            get { return unlockHitbox; }
            set { unlockHitbox = value; }
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
        public Door(bool isOpen, Dictionary<string, Texture2D> textureDict, Rectangle sourceRect, Rectangle position, int level)
             : base(textureDict["doors"], sourceRect, position)
        {
            this.isOpen = isOpen;
            this.level = level;
            this.textureDict = textureDict;

            // Create door hitboxes
            unlockHitbox = new Rectangle(
                position.X - position.Width / 2,
                position.Y - position.Height / 2,
                position.Width * 2,
                position.Height * 2);

            hitbox = new Rectangle(
                position.X - 1,
                position.Y - 1,
                position.Width + 2,
                position.Height + 2);
        }

        // Methods

        #region// Game Loop
        /// <summary>
        /// Updates the game's doors.
        /// </summary>
        /// <param name="gameTime">State of the game's time.</param>
        public override void Update(GameTime gameTime)
        {
            if (isOpen)
            {
                sourceRect.Y = 16;
            }
        }

        /// <summary>
        /// Updates the events.
        /// </summary>
        /// <param name="myPlayer">Reference to the player.</param>
        public void Update(Player myPlayer)
        {
            KeyboardState currentState = Keyboard.GetState();

            if (currentState.IsKeyUp(Keys.E) &&
                previousState.IsKeyDown(Keys.E) &&
                unlockHitbox.Contains(myPlayer.Position))
            {
                if (OnKeyPressOnce != null)
                {
                    OnKeyPressOnce(Keys.E, currentState, previousState);
                }
                if (OnKeyPress != null)
                {
                    OnKeyPress(myPlayer, currentState, previousState);
                }
            }
            previousState = currentState;
        }

        /// <summary>
        /// Draws the game's doors.
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
            return unlockHitbox.Intersects(obj.Position);
        }

        /// <summary>
        /// Loads the next level.
        /// </summary>
        /// <param name="myPlayer">Reference to Game1's player.</param>
        /// <param name="currentState">The current state of the keyboard.</param>
        /// <param name="previousState">The previous state of the keyboard.</param>
        public void Interact(Player myPlayer, KeyboardState currentState, KeyboardState previousState)
        {
            // Set locally declared boolean
            bool ifOpen = false;

            // If the door is not open set 'ifOpen' to true if the E key is pressed
            // AND when the door's unlock hitbox is colliding with the player.
            if (!isOpen &&
                currentState.IsKeyUp(Keys.E) &&
                previousState.IsKeyDown(Keys.E) &&
                IsColliding(myPlayer))
            {
                ifOpen = true;
            }

            // If the door is open, set 'ifOpen' to true.
            // Then load the level connected to the door.
            else if (isOpen)
            {
                ifOpen = true;
                if (hitbox.Intersects(myPlayer.Position))
                {
                    if (level == 1)
                    {
                        LevelManager.Instance.LoadLevel(GameLevels.Level1);
                    }
                    else if (level == 2)
                    {
                        LevelManager.Instance.LoadLevel(GameLevels.Level2);
                    }
                    else if (level == 3)
                    {
                        LevelManager.Instance.LoadLevel(GameLevels.Level3);
                    }
                    else if (level == 4)
                    {
                        LevelManager.Instance.LoadLevel(GameLevels.Level4);
                    }
                }
            }

            // Set the 'isOpen' field equal to the value of 'ifOpen'.
            isOpen = ifOpen;
        }
    }
}
