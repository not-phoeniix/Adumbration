using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public virtual void Update(GameTime gameTime)
        {

        }

        /// <summary>
        /// Draws this GameObject to the screen with given position
        /// </summary>
        public virtual void Draw(SpriteBatch sb)
        {
            sb.Draw(spriteSheet, positionRect, sourceRect, Color.White);
        }

        /// <summary>
        /// Draws this GameObject to the screen with custom position and scale, uses Rectangle
        /// </summary>
        /// <param name="sb">SpriteBatch to draw with</param>
        /// <param name="position">Position Rectangle</param>
        public virtual void Draw(SpriteBatch sb, Rectangle position)
        {
            // draws object
            sb.Draw(spriteSheet, position, sourceRect, Color.White);

            // updates position field
            positionRect = position;
        }

        /// <summary>
        /// Draws this GameObject to the screen with custom position, no scale, uses Vector2
        /// </summary>
        /// <param name="sb">SpriteBatch to draw with</param>
        /// <param name="position">Position Vector2</param>
        public virtual void Draw(SpriteBatch sb, Vector2 position)
        {
            // draws object
            sb.Draw(spriteSheet, position, sourceRect, Color.White);

            // updates position field
            positionRect.X = (int)position.X;
            positionRect.Y = (int)position.Y;
        }

        /// <summary>
        /// Draws this GameObject to the screen with offset
        /// Vector2, doesn't modify internal position value.
        /// </summary>
        /// <param name="sb">SpriteBatch to draw with</param>
        /// <param name="offset">Offset Vector2</param>
        public void DrawOffset(SpriteBatch sb, Vector2 offset)
        {
            // calculated new position from offset
            Rectangle newPos = new Rectangle(
                positionRect.X + (int)offset.X,
                positionRect.Y + (int)offset.Y,
                positionRect.Width,
                positionRect.Height);

            // draws object
            sb.Draw(spriteSheet, newPos, sourceRect, Color.White);
        }

        /// <summary>
        /// Check if a specific object is colliding with another object.
        /// </summary>
        /// <param name="obj">Reference to the object in collision.</param>
        /// <returns>True if collision occurs, otherwise false.</returns>
        public abstract bool IsColliding(GameObject obj);
    }
}
