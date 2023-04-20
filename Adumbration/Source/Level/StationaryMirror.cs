using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            sb.Draw(spriteSheet, positionRect, sourceRect, Color.White);
        }
    }
}
