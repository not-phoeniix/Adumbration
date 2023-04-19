using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Adumbration
{
    /// <summary>
    /// the light receptor class will basically be a wall tile 
    /// that catches the light
    /// it's 
    /// </summary>
    internal class LightReceptor : Wall, ISignal
    {
        // Fields
        private Rectangle activationPoint;
        private bool textureFlipped;
        private int signalNum;
        private bool isActivated;

        public int SignalNum
        {
            get { return signalNum; }
        }

        /// <summary>
        /// Returns whether this receptor is currently activated
        /// </summary>
        public bool IsActivated
        {
            get { return isActivated; }
        }

        //the constructor for this class
        public LightReceptor(Texture2D spriteSheet, Rectangle position, Direction dir, int signalNum)
            : base(spriteSheet, new Rectangle(0, 0, 0, 0), position)
        {
            this.signalNum = signalNum;
            textureFlipped = false;

            // activation point is 1 pixel expanded from position rectangle
            activationPoint = new Rectangle(
                positionRect.X - 1,
                positionRect.Y - 1,
                positionRect.Width + 2, 
                positionRect.Height + 2);

            // determines source rect depending on direction of receptor
            switch(dir)
            {
                case Direction.Down:
                    base.sourceRect = new Rectangle(6 * 16, 3 * 16, 16, 16);
                    break;

                case Direction.Up:
                    base.sourceRect = new Rectangle(8 * 16, 3 * 16, 16, 16);
                    break;

                case Direction.Left:
                    base.sourceRect = new Rectangle(7 * 16, 3 * 16, 16, 16);
                    break;

                case Direction.Right:
                    base.sourceRect = new Rectangle(7 * 16, 3 * 16, 16, 16);
                    textureFlipped = true;
                    break;
            }
        }

        //for all beams inside the allbeams list,
        //it will check if the receptor is colliding with it
        //then it will 

        /// <summary>
        /// For all beams inside the list, it'll check for collision 
        /// and change the property for IsActivated if colliding
        /// </summary>
        /// <param name="beams">List of beams required to check for collision</param>
        public void Update(List<LightBeam> beams)
        {
            isActivated = false;

            foreach(LightBeam beam in beams)
            {
                // If the light beam is activated
                if(IsColliding(beam) && beam != null)
                {
                    isActivated = true;
                    //System.Diagnostics.Debug.WriteLine("Activated");
                    //  writeline's every frame KILL the framerate ^ 
                    //  be careful w/ them...
                }
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            // same as base.draw except flips texture sometimes
            sb.Draw(
                spriteSheet,
                positionRect,
                sourceRect,
                Color.White,
                0,
                Vector2.Zero,
                textureFlipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                0);
        }

        //checks to see if the object that is colliding with is a lightbeam
        //if it is a lightbeam it will return as activated 
        //otherwise it will ignore other things and stay off
        public bool IsColliding(LightBeam beam)
        {
            return activationPoint.Intersects(beam.Position);
        }
    }
}
