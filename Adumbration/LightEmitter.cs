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
                    sourceRect.X + sourceRect.Width / 2,
                sourceRect.Y + sourceRect.Height / 2,
                2, 2), dir);
        }

        public override void Draw(SpriteBatch sb)
        {
            // First draw light beam
            beam.Draw(sb);
            
            // then draw emitter over it.
            base.Draw(sb);
        }
    }
}
