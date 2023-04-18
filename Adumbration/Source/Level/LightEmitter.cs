using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Adumbration
{
    internal class LightEmitter : Wall
    {
        // Fields 
        private bool isOn;
        private LightBeam beam;
        private Direction dir;
        private Level currentLevel;
        private Texture2D whitePixelTexture;

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

        public LightEmitter(Dictionary<string, Texture2D> textureDict, Rectangle sourceRect, Rectangle position, Direction dir, Level currentLevel)
            : base(textureDict["walls"], sourceRect, position)
        {
            this.dir = dir;
            isOn = true;
            this.currentLevel = currentLevel;

            whitePixelTexture = textureDict["whitePixel"];

            // Instantiate Light Beam at center point of emitter's 
            // Source rectangle
            beam = new LightBeam(
                whitePixelTexture,
                new Rectangle(
                    position.X + position.Width / 2 - 1,
                    position.Y + position.Height / 2,
                    2, 2), 
                dir);
        }

        //public override void Update(GameTime gameTime)
        //{
        //    beam.Update(gameTime);
        //}

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
        }
    }
}
