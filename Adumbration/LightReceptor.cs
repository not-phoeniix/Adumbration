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
    // Delegate for activating or deactivating emitter / door
    public delegate void OnLightBeamReceivedDelegate();

    /// <summary>
    /// the light receptor class will basically be a wall tile 
    /// that catches the light
    /// it's 
    /// </summary>
    internal class LightReceptor : Wall
    {
        // Fields
        private Rectangle activationPoint;

        // Event
        public event OnLightBeamReceivedDelegate OnActivation;

        //the constructor for this class
        public LightReceptor(Texture2D spriteSheet, Rectangle sourceRect, Rectangle position, Rectangle activationPoint)
            : base(spriteSheet, sourceRect, position)
        {
            this.activationPoint = activationPoint;
        }

        public void Update(GameTime gameTime, Level currentLevel, LightBeam beam)
        {
            //foreach(LightEmitter emitter in currentLevel.TileList)
            //{
                // If the light beam is activated
                if (IsColliding(beam))
                {
                    OnActivation();
                    System.Diagnostics.Debug.WriteLine("Activated");
                }
                else
                {
                System.Diagnostics.Debug.WriteLine("Not Activated");
                }
            //}            
        }

        //checks to see if the object that is colliding with is a lightbeam
        //if it is a lightbeam it will return as activated 
        //otherwise it will ignore other things and stay off
        public override bool IsColliding(GameObject obj)
        {
            if (obj.Position.Intersects(activationPoint))
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
