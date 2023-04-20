using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Adumbration
{
    internal class LightEmitter : Wall, ISignal
    {
        // Fields 
        private bool enabledState;
        private bool startingEnabled;
        private bool prevEnabledState;
        private bool textureFlipped;
        private LightBeam beam;
        private Direction dir;
        private Texture2D whitePixelTexture;
        private Rectangle enabledSource;
        private Rectangle disabledSource;
        private Vector2 beamStartPos;
        private int signalNum;

        public int SignalNum
        {
            get { return signalNum; }
        }

        // Properties
        /// <summary>
        /// Get and set property for if the 
        /// Emitter is on
        /// </summary>
        public bool Enabled
        {
            get { return enabledState; }
            set { enabledState = value; }
        }

        /// <summary>
        /// Returns whether the emitter started enabled or not
        /// </summary>
        public bool StartingEnabled
        {
            get { return startingEnabled; }
        }

        /// <summary>
        /// Beam that comes from the emitter
        /// </summary>
        public LightBeam Beam
        {
            get { return beam; }
        }

        /// <summary>
        /// Creates a new LightEmitter tile
        /// </summary>
        /// <param name="textureDict">Dictionary of game textures</param>
        /// <param name="position">Position to draw emitter</param>
        /// <param name="dir">Direction of the emitter</param>
        /// <param name="enabled">Whether to start the emitter enabled or not</param>
        public LightEmitter(Dictionary<string, Texture2D> textureDict, Rectangle position, Direction dir, bool enabled, int signalNum)
            : base(textureDict["walls"], new Rectangle(0, 0, 0, 0), position)
        {
            this.dir = dir;
            this.enabledState = startingEnabled = enabled;
            this.textureFlipped = false;
            this.signalNum = signalNum;

            whitePixelTexture = textureDict["whitePixel"];

            // determines source rect depending on direction of emitter
            switch(dir)
            {
                case Direction.Down:
                    enabledSource = new Rectangle(3 * 16, 3 * 16, 16, 16);
                    disabledSource = new Rectangle(0, 3 * 16, 16, 16);
                    beamStartPos = new Vector2(7, 9);
                    break;

                case Direction.Up:
                    enabledSource = new Rectangle(5 * 16, 3 * 16, 16, 16);
                    disabledSource = new Rectangle(2 * 16, 3 * 16, 16, 16);
                    beamStartPos = new Vector2(7, 1);
                    break;

                case Direction.Left:
                    enabledSource = new Rectangle(4 * 16, 3 * 16, 16, 16);
                    disabledSource = new Rectangle(1 * 16, 3 * 16, 16, 16);
                    beamStartPos = new Vector2(1, 7);
                    break;

                case Direction.Right:
                    enabledSource = new Rectangle(4 * 16, 3 * 16, 16, 16);
                    disabledSource = new Rectangle(1 * 16, 3 * 16, 16, 16);
                    beamStartPos = new Vector2(14, 7);
                    textureFlipped = true;
                    break;
            }
        }

        public override void Update(GameTime gameTime)
        {
            #region // State updates

            // creates beam if emitter is enabled
            if(enabledState == true && prevEnabledState == false)
            {
                beam = new LightBeam(
                    whitePixelTexture,
                    new Rectangle(
                        (int)beamStartPos.X + positionRect.X,
                        (int)beamStartPos.Y + positionRect.Y,
                        2, 
                        2),
                    dir);
            }

            // deletes beam if emitter isn't enabled
            if(enabledState == false && prevEnabledState == true)
            {
                beam = null;
                LevelManager.Instance.CurrentLevel.Beams.Clear();
            }

            #endregion

            prevEnabledState = enabledState;
        }

        public override void Draw(SpriteBatch sb)
        {
            // same as base.draw except flips texture sometimes
            sb.Draw(
                spriteSheet,
                positionRect,
                enabledState ? enabledSource : disabledSource,
                Color.White,
                0,
                Vector2.Zero,
                textureFlipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                0);
        }
    }
}
