using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Penumbra;

namespace Adumbration
{
    /// <summary>
    /// a wall class that is here to stop the player from going out of bounds
    /// </summary>
    public class Wall : GameObject
    {
        private Hull hull;

        /// <summary>
        /// Parameterized Constructor for this class
        /// </summary>
        /// <param name="spriteSheet">Full Texture2D sprite sheet.</param>
        /// <param name="sourceRect">Source to take from in sprite sheet to be drawn.</param>
        /// <param name="position">Position in window to draw Wall.</param>
        public Wall(Texture2D spriteSheet, Rectangle sourceRect, Rectangle position)
             : base(spriteSheet, sourceRect, position)
        {
            // creates shadow casting hull
            hull = new Hull(new Vector2[]
            {
                new Vector2(position.X, position.Y),
                new Vector2(position.X + position.Width, position.Y),
                new Vector2(position.X + position.Width, position.Y + position.Height),
                new Vector2(position.X, position.Y + position.Height)
            });
        }

        /// <summary>
        /// Wall's hull for shadow casting
        /// </summary>
        public Hull Hull { get { return hull; } }

        /// <summary>
        /// This is to get a bool value inside Game1
        /// to stop the player from passing this point.
        /// </summary>
        /// <param name="obj">Reference to the colliding object.</param>
        /// <returns></returns>
        public override bool IsColliding(GameObject obj)
        {
            return Position.Intersects(obj.Position);
        }
    }
}
