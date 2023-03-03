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
    /// this won't do anything, it's to just draw the texture
    /// it's a floor for god's sake what do you expect it to do?
    /// why are you still even reading this? Go do your code you bozo
    /// You're still here? What in god's green earth are you looking at.
    /// BRO GO DO YOUR CODE IT'S LITERALLY A FLOOR
    /// </summary>
    internal class Floor : GameObject
    {
        public Floor(Texture2D spriteSheet, Rectangle sourceRect, Rectangle position) 
            : base(spriteSheet, sourceRect, position)
        {
            
        }

        public override void Update(GameTime gameTime) 
        {
            System.Diagnostics.Debug.WriteLine("I didn't write this yet");
        }

        public override bool IsColliding(GameObject obj) 
        {
            return this.Position.Intersects(obj.Position);
        }
    }
}
