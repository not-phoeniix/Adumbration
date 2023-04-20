using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Adumbration
{
    /// <summary>
    /// Alexander Gough
    /// Door class that inherits from the Wall Class.
    /// Will have an open and closed state.
    /// Leads to other levels or back to the hub room.
    /// </summary>
    public class Door : Wall, IHitbox
    {
        // Fields
        protected bool isOpen;
        private bool isFlipped;
        private bool isInteracted;
        private Rectangle hitbox;
        private GameLevels level;
        private double doorOffsetTimer;     // keep track of time after interact

        KeyboardState kbState;
        KeyboardState kbStatePrev;

        // Property
        public Rectangle Hitbox
        {
            get { return hitbox; }
        }

        // Constructor(s)

        /// <summary>
        /// Parameterized Constructor of Door.
        /// Requires the base constructor parameters.
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="position"></param>
        /// <param name="level"></param>
        public Door(Texture2D texture, Rectangle position, GameLevels level, Direction dir)
             : base(texture, new Rectangle(0, 0, 16, 16), position)
        {
            this.level = level;
            doorOffsetTimer = 20;   // num frames till load level

            isOpen = false;
            isInteracted = false;

            // Create door hitboxes
            hitbox = new Rectangle(
                position.X - position.Width / 2,
                position.Y - position.Height / 2,
                position.Width * 2,
                position.Height * 2);

            isFlipped = false;

            sourceRect = new Rectangle(0, 0, 16, 16);

            // determines source rect depending on direction
            switch(dir)
            {
                case Direction.Down:
                    sourceRect.X = 0;
                    break;
                case Direction.Up:
                    sourceRect.X = 2 * 16;
                    break;
                case Direction.Left:
                    sourceRect.X = 1 * 16;
                    break;
                case Direction.Right:
                    sourceRect.X = 1 * 16;
                    isFlipped = true;
                    break;
            }
        }

        // Methods

        #region // Game Loop

        /// <summary>
        /// Updates door state, checking for key presses and the player's hitbox.
        /// </summary>
        /// <param name="myPlayer">Reference to the player.</param>
        public virtual void Update(Player myPlayer)
        {
            kbState = Keyboard.GetState();

            // updates sprite depending on open/close state
            if(isOpen)
            {
                Hull.Enabled = false;
                sourceRect.Y = 1 * 16;
            }
            else
            {
                Hull.Enabled = true;
                sourceRect.Y = 0 * 16;
            }

            sourceRect.Y = isOpen ? 16 : 0;

            // if player interacts with door:
            if (hitbox.Intersects(myPlayer.Position) && 
                Game1.IsKeyPressedOnce(Keys.E, kbState, kbStatePrev) &&
                !isInteracted)
            {
                // change texture, start timer
                isOpen = true;
                isInteracted = true;
            }

            if(isInteracted)
            {
                doorOffsetTimer--;

                if(doorOffsetTimer <= 0)
                {
                    LevelManager.Instance.LoadLevel(level);
                }
            }

            kbStatePrev = kbState;
        }

        /// <summary>
        /// Draws this door.
        /// </summary>
        /// <param name="sb">Reference to the SpriteBatch.</param>
        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(
                spriteSheet,
                positionRect,
                sourceRect,
                Color.White,
                0,
                Vector2.Zero,
                isFlipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                0);
        }

        #endregion

        /// <summary>
        /// Checks for a collision between an object and a door's hitbox.
        /// </summary>
        /// <param name="obj">References the object that may collide with a door's hitbox.</param>
        /// <returns>True if collision occurs, otherwise false.</returns>
        public override bool IsColliding(GameObject obj)
        {
            return hitbox.Intersects(obj.Position);
        }
    }
}
