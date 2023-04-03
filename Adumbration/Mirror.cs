using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
<<<<<<< HEAD
using SharpDX.MediaFoundation;

namespace Adumbration
{
    /// <summary>
    /// this is a mirror class that can reflect light 
    /// and will be used to see if we need to reflect a light in a specific direction
    /// </summary>
    internal class Mirror : GameObject
    {
        //a basic constructor for the mirror class
        public Mirror(Texture2D spriteSheet, Rectangle sourceRect, Rectangle position)
            : base(spriteSheet, sourceRect, position)
        {}

        /// <summary>
        /// a method that will be mainly be used to see if the player collides
        /// with the mirror which allows for interaction
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool IsColliding(GameObject obj)
        {
            if (Position.Intersects(obj.Position))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
=======

namespace Adumbration
{
    public enum Orientation
    {
        Forward,
        Backward
    }
    internal class Mirror : GameObject
    {
        // Fields 
        Orientation orientation;
        LightBeam reflection;

        // Properties
        /// <summary>
        /// Set property for a light beam reflection
        /// </summary>
        public LightBeam Reflection
        {
            set { reflection = value; }
        }

        /// <summary>
        /// Parameterized constructor of the mirror
        /// </summary>
        /// <param name="spriteSheet">spritesheet where player's texture is</param>
        /// <param name="sourceRect">The source rectangle within the spritesheet</param>
        /// <param name="position">position of the player</param>
        /// <param name="orientation">Orietation of Mirror</param>
        public Mirror(Texture2D spriteSheet, Rectangle sourceRect, Rectangle position, Orientation orientation)
           : base(spriteSheet, sourceRect, position)
        {
            this.orientation = orientation;

            // There is no light beam reflection upon instantiation. 
            this.reflection = null;
        }

        /// <summary>
        /// Mirror's Update method
        /// </summary>
        /// <param name="gameTime">time passing in game</param>
        public void Update(GameTime gameTime)
        {
            // TODO: Make Mirror interactable and move with player
        }

        /// <summary>
        /// Draw's mirror according to internal position
        /// </summary>
        /// <param name="sb">SpriteBatch object used to draw with</param>
        public void Draw(SpriteBatch sb)
        {
            // TODO: Draw Mirror
        }

        /// <summary>
        /// Creates a new Light Beam object 
        /// as a result of colliding with a light beam
        /// </summary>
        /// <param name="incomingLight">The lightbeam that is incoming</param>
        public void CreateReflection(LightBeam incomingLight) // To be called in Light Beam Class
        {
            // TODO: Instantiate Light Beam 
        }

>>>>>>> 1c4568d66e6fbc7df44ed3db22f599307ebb3bac
    }
}
