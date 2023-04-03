using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.MediaFoundation;

namespace Adumbration
{
    /// <summary>
    /// this is a mirror class that can reflect light 
    /// and will be used to see if we need to reflect a light in a specific direction
    /// </summary>
    internal class Mirror : GameObject
    {
        //a basic constructor for the mirror class
        public Mirror(Texture2D spriteSheet, Rectangle sourceRect, Rectangle position)
            : base(spriteSheet, sourceRect, position)
        {}

        /// <summary>
        /// a method that will be mainly be used to see if the player collides
        /// with the mirror which allows for interaction
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
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
