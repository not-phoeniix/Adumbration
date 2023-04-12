﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Adumbration
{
    internal class LightEmitter : Wall
    {
        // Fields 
        private bool isOn;
        private LightBeam beam;
        private Direction dir;
        private Level currentLevel;

        // Properties
        /// <summary>
        /// Get and set property for if the 
        /// Emitter is on
        /// </summary>
        public bool IsOn
        {
            get { return isOn; }
            set { isOn = value; }
        }

        /// <summary>
        /// Beam that comes from the emitter
        /// </summary>
        public LightBeam Beam
        {
            get { return beam; }
        }

        public LightEmitter(Texture2D spriteSheet, Rectangle sourceRect, Rectangle position, Direction dir, Level currentLevel)
            : base(spriteSheet, sourceRect, position)
        {
            this.dir = dir;
            isOn = true;
            this.currentLevel = currentLevel;

            // Instantiate Light Beam at center point of emitter's 
            // Source rectangle
            beam = new LightBeam(
                spriteSheet,
                new Rectangle(
                    position.X + position.Width / 2 - 1,
                    position.Y + position.Height / 2,
                    2, 2), 
                dir);
        }

        public override void Update(GameTime gameTime)
        {
            beam.Update(gameTime, currentLevel);
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
        }
    }
}
