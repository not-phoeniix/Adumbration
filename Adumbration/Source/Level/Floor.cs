using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Adumbration
{
    /// <summary>
    /// this won't do anything, it's to just draw the texture
    /// it's a floor for god's sake what do you expect it to do?
    /// why are you still even reading this? Go do your code you bozo
    /// You're still here? What in god's green earth are you looking at.
    /// BRO GO DO YOUR CODE IT'S LITERALLY A FLOOR
    /// </summary>
    internal class Floor : GameObject
    {
        /// <summary>
        /// Parameterized Constructor for Floor class.
        /// Requires the base constructor parameters.
        /// </summary>
        /// <param name="spriteSheet">Full Texture2D spritesheet.</param>
        /// <param name="sourceRect">Source to take from in spritesheet to be drawn.</param>
        /// <param name="position">Position in window to draw Floor.</param>
        public Floor(Texture2D spriteSheet, Rectangle sourceRect, Rectangle position)
            : base(spriteSheet, sourceRect, position)
        {

        }
    }
}
