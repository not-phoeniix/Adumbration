using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Adumbration
{
    internal class StationaryMirror : Mirror
    {
        // Fields 
        private Direction dir;

        public StationaryMirror(Texture2D texture, Rectangle position, Direction dir, MirrorType type)
            : base(texture, position, type)
        {
            base.sourceRect = new Rectangle(0, 4 * 16, 16, 16);
            this.dir = dir;
        }

        public override void Draw(SpriteBatch sb)
        {
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
