using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
<<<<<<< HEAD
<<<<<<< HEAD
using Microsoft.Xna.Framework.Input;
using SharpDX.XAudio2;
=======
>>>>>>> parent of 10d29d3 (Set up door texture change function but cannot currently test it because the wall blocks the way.)
=======
>>>>>>> parent of 10d29d3 (Set up door texture change function but cannot currently test it because the wall blocks the way.)

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
    /// <returns></returns>
    public delegate bool KeyPressOnceDelegate(Keys keys, KeyboardState current, KeyboardState previous);

    /// <summary>
    /// Delegate for methods of the void return type and take the Player parameter
    /// </summary>
    /// <param name="player">Reference to the the player game object</param>
    public delegate void KeyPressDelegate(Player player);
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
        private GameLevels level;
        private KeyboardState previousState;

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

        // Constructor(s)

        /// <summary>
        /// Parameterized Constructor of Door.
        /// Requires the base constructor parameters.
        /// </summary>
        /// <param name="isOpen"></param>
        /// <param name="spriteSheet"></param>
        /// <param name="sourceRect"></param>
        /// <param name="position"></param>
        public Door(bool isOpen, Texture2D spriteSheet, Rectangle sourceRect, Rectangle position)
             : base(spriteSheet, sourceRect, position)
        {
            this.isOpen = isOpen;
            sourceXOrigin = sourceRect.X;

            // Create door hitbox
            doorHitbox = new Rectangle(
                position.X,
                position.Y,
                position.Width,
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
<<<<<<< HEAD
<<<<<<< HEAD
            if (isOpen)
            {
                sourceRect.X = sourceXOrigin;
            }
            else
            {
                sourceRect.X = 4 * 16;
            }
        }

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
                    OnKeyPress(myPlayer);
                }
            }
            previousState = currentState;
=======

>>>>>>> parent of 10d29d3 (Set up door texture change function but cannot currently test it because the wall blocks the way.)
=======

>>>>>>> parent of 10d29d3 (Set up door texture change function but cannot currently test it because the wall blocks the way.)
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
        /// Checks for a collision between an object and a door.
        /// </summary>
        /// <param name="obj">References the object that may collide with a door.</param>
        /// <returns>True if collision occurs, otherwise false.</returns>
        public override bool IsColliding(GameObject obj)
        {
            return DoorHitbox.Intersects(obj.Position);
        }

        /// <summary>
        /// Loads the next level.
        /// </summary>
        public void Interact(Player myPlayer)
        {
<<<<<<< HEAD
<<<<<<< HEAD
            KeyboardState currentState = Keyboard.GetState();

            if (this.IsColliding(myPlayer) && !isOpen)
            {
                if (currentState.IsKeyDown(Keys.E))
                {
                    previousState = currentState;
                    if (previousState.IsKeyDown(Keys.E))
                    {
                        isOpen = true;
                    }
                }
=======
            if (isOpen && IsColliding(myPlayer))
            {

>>>>>>> parent of 10d29d3 (Set up door texture change function but cannot currently test it because the wall blocks the way.)
=======
            if (isOpen && IsColliding(myPlayer))
            {

>>>>>>> parent of 10d29d3 (Set up door texture change function but cannot currently test it because the wall blocks the way.)
            }
            previousState = currentState;
        }
    }
}
