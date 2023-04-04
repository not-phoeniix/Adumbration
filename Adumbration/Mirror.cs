using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Adumbration
{
    internal class Mirror : GameObject
    {
        //constructor for this class
        public Mirror(Texture2D spriteSheet, Rectangle sourceRect, Rectangle position, Direction dir)
             : base(spriteSheet, sourceRect, position)
        {}

        public override bool IsColliding(GameObject obj)
        {
            if (Position.Intersects(obj.Position))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
