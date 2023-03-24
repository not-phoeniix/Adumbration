using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Adumbration
{
    internal class LightReceptor : Wall
    {
        private bool activated;

        //the constructor for this class
        public LightReceptor(Texture2D spriteSheet, Rectangle sourceRect, Rectangle position)
            : base(spriteSheet, sourceRect, position)
        {
            activated = false;
        }

        //checks to see if the object that is colliding with is a lightbeam
        //if it is a lightbeam it will return as activated 
        //otherwise it will ignore other things and stay off
        public override bool IsColliding(GameObject obj)
        {
            if (obj is LightBeam)
            {
                return activated = true;
            }
            else
            {
                return activated = false;
            }
        }
    }
}
