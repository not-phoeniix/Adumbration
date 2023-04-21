using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Adumbration
{
    internal class StationaryMirror : Mirror
    {
        // Fields 
        private Direction dir;

        /// <summary>
        /// Constructor that uses Mirror's contructor but changes texture
        /// </summary>
        /// <param name="texture">Different texture than regular mirrors</param>
        /// <param name="position">position of mirror</param>
        /// <param name="dir">Direction of the stationary mirror</param>
        /// <param name="type">Type of mirror</param>
        public StationaryMirror(Texture2D texture, Rectangle position, Direction dir, MirrorType type)
            : base(texture, position, type)
        {
            base.sourceRect = new Rectangle(0, 4 * 16, 16, 16);
            this.dir = dir;
        }

        /// <summary>
        /// Update that does nothing
        /// </summary>
        /// <param name="myplayer">The Player</param>
        /// <param name="currentLevel">Current Level the mirror is in</param>
        /// <param name="gameTime">Game</param>
        public override void Update(Player myplayer, Level currentLevel, GameTime gameTime)
        {
            // Do nothing stationary mirrors don't update position
        }

        /// <summary>
        /// Draws Different texture for Mirror
        /// </summary>
        /// <param name="sb">Game1 spritebatch</param>
        public override void Draw(SpriteBatch sb)
        {
            // Chooses source rect based on direction
            switch(dir)
            {
                case Direction.Right:
                    sourceRect.X = 0 * 16;
                    break;
                case Direction.Down:
                    sourceRect.X = 1 * 16;
                    break;
                case Direction.Left:
                    sourceRect.X = 2 * 16;
                    break;
                case Direction.Up:
                    sourceRect.X = 3 * 16;
                    break;
            }

            sb.Draw(spriteSheet, positionRect, sourceRect, Color.White);
        }
    }
}
