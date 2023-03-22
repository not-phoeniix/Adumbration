using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Adumbration
{
    /// <summary>
    /// a wall class that is here to stop the player from going out of bounds
    /// </summary>
    public class Wall : GameObject
    {
        /// <summary>
        /// Parameterized Constructor for this class
        /// </summary>
        /// <param name="spriteSheet">Full Texture2D sprite sheet.</param>
        /// <param name="sourceRect">Source to take from in sprite sheet to be drawn.</param>
        /// <param name="position">Position in window to draw Wall.</param>
        public Wall(Texture2D spriteSheet, Rectangle sourceRect, Rectangle position)
             : base(spriteSheet, sourceRect, position)
        {

        }

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
