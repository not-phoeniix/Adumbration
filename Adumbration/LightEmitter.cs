using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.XAudio2;

namespace Adumbration
{
    internal class LightEmitter : Wall
    {
        // Fields 
        private bool isOn;
        private LightBeam beam;
        private Direction dir;

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

        public LightEmitter(Texture2D spriteSheet, Rectangle sourceRect, Rectangle position, Direction dir, Texture2D beamTexture)
            : base(spriteSheet, sourceRect, position)
        {
            this.dir = dir;
            this.isOn = true;

            // Instantiate Light Beam at center point of emitter's 
            // Source rectangle
            this.beam = new LightBeam(
                beamTexture,
                new Rectangle(
                    position.X + position.Width / 2,
                position.Y + position.Height / 2,
                2, 2), dir);
        }

        public void Update(GameTime gameTime, Level currentLevel)
        {
            beam.Update(gameTime, currentLevel);
        }
    }
}
