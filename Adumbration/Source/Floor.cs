﻿using Microsoft.Xna.Framework;
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

        public override void Update(GameTime gameTime)
        {
            //it's a floor, what is there to update yet smh
        }

        public override bool IsColliding(GameObject obj)
        {
            return Position.Intersects(obj.Position);
        }
    }
}
