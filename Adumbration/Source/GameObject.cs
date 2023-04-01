using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Adumbration
{
    /// <summary>
    /// this is the abstract 
    /// parent class that almost EVERY
    /// object will use 
    /// </summary>
    public abstract class GameObject
    {
        //all the protected fields that will be used in every child class
        protected Texture2D spriteSheet;
        protected Rectangle sourceRect;
        protected Rectangle positionRect;

        /// <summary>
        /// Full position rectangle of this GameObject, get/set,
        /// public so it can be accessed outside class/children
        /// </summary>
        public Rectangle Position
        {
            get { return positionRect; }
            set { positionRect = value; }
        }

        /// <summary>
        /// Abstract constructor, takes in
        /// </summary>
        /// <param name="spriteSheet">Full Texture2D spritesheet.</param>
        /// <param name="sourceRect">Source to take from in spritesheet to be drawn.</param>
        /// <param name="position">Position in window to draw GameObject.</param>
        public GameObject(Texture2D spriteSheet, Rectangle sourceRect, Rectangle position)
        {
            this.spriteSheet = spriteSheet;
            this.sourceRect = sourceRect;
            Position = position;
        }

        //all the abstract methods that will be used in all of the classes

        /// <summary>
        /// will be changed to update anything that happens to this SPECIFIC sprite
        /// </summary>
        public virtual void Update(GameTime gameTime) { }

        /// <summary>
        /// Draws this GameObject to the screen with given position
        /// </summary>
        public virtual void Draw(SpriteBatch sb)
        {
            sb.Draw(spriteSheet, positionRect, sourceRect, Color.White);
        }

        /// <summary>
        /// Check if a specific object is colliding with another object.
        /// </summary>
        /// <param name="obj">Reference to the object in collision.</param>
        /// <returns>True if collision occurs, otherwise false.</returns>
        public virtual bool IsColliding(GameObject obj)
        {
            return positionRect.Intersects(obj.Position);
        }
    }
}
