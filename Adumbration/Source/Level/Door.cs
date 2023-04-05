using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.XAudio2;


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
    /// Door class that inherits from abstract GameObject.
    /// Will have an open and closed state.
    /// Leads to other levels.
    /// </summary>
    public class Door : GameObject
    {
        // Fields
        private bool isOpen;
        private Rectangle doorHitbox;
        private int sourceXOrigin;
        private KeyboardState previousState;
        private int level;

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

        // Properties

        /// <summary>
        /// Property that allows boolean isOpen to be instantiated from the constructor.
        /// </summary>
        public bool IsOpen
        {
            get { return isOpen; }
            set { isOpen = value; }
        }

        /// <summary>
        /// Property that creates a hitbox for the door.
        /// </summary>
        public Rectangle DoorHitbox
        {
            get { return doorHitbox; }
        }

        /// <summary>
        /// Gets the int that represents the level the door leads to.
        /// </summary>
        public int Level
        {
            get { return level; }
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
        public Door(bool isOpen, Texture2D spriteSheet, Rectangle sourceRect, Rectangle position, int level)
             : base(spriteSheet, sourceRect, position)
        {
            this.isOpen = isOpen;
            this.level = level;
            sourceXOrigin = sourceRect.X;

            // Create door hitbox
            doorHitbox = new Rectangle(
                position.X - position.Width,
                position.Y - position.Height,
                position.Width * 2,
                position.Height * 2);
        }

        // Methods

        #region// Game Loop
        /// <summary>
        /// Updates the game's doors.
        /// </summary>
        /// <param name="gameTime">State of the game's time.</param>
        public override void Update(GameTime gameTime)
        {
            if (IsOpen)
            {
                sourceRect.X = (level - 1) * 16;
            }
            else if (!IsOpen)
            {
                sourceRect.X = 4 * 16;
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
                DoorHitbox.Contains(myPlayer.Position))
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
        /// <param name="gameTime">State of the game's time.</param>
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
            return DoorHitbox.Intersects(obj.Position);
        }

        /// <summary>
        /// Loads the next level.
        /// </summary>
        /// <param name="myPlayer">Reference to Game1's player.</param>
        /// <param name="currentState">The current state of the keyboard.</param>
        /// <param name="previousState">The previous state of the keyboard.</param>
        public void Interact(Player myPlayer, KeyboardState currentState, KeyboardState previousState)
        {
            bool ifOpen = false;
            if (!IsOpen)
            {
                if (currentState.IsKeyUp(Keys.E) &&
                    previousState.IsKeyDown(Keys.E) &&
                    IsColliding(myPlayer))
                {
                    ifOpen = true;
                }
            }
            IsOpen = ifOpen;
        }
    }
}
