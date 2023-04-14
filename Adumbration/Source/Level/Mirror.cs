using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

// Describes the two types of mirrors we have
public enum MirrorType
{
    Forward,
    Backward
}

namespace Adumbration
{
    internal class Mirror : GameObject, IHitbox
    {
        // Fields
        private List<LightBeam> reflectedBeams;
        private MirrorType type;
        private Rectangle hitbox;

        // Constructor

        /// <summary>
        /// Parameterized constructor for the mirror class.
        /// </summary>
        /// <param name="textureDict">The dictionary to get textures from.</param>
        /// <param name="position">The game position.</param>
        /// <param name="type">The type of mirror.</param>
        public Mirror(Texture2D mirrorTexture, Rectangle position, MirrorType type)
             : base(
                   mirrorTexture,
                   new Rectangle(
                       0,
                       0,
                       mirrorTexture.Width,
                       mirrorTexture.Height
                       ),
                   position)
        {
            this.type = type;
            reflectedBeams = new List<LightBeam>();

            // Create hitbox rectangle
            hitbox = new Rectangle(
                position.X - 1,
                position.Y - 1,
                position.Width + 2,
                position.Height + 2);
        }

        /// <summary>
        /// Returns the light beam attatched to this mirror, null if non existant
        /// </summary>
        public List<LightBeam> ReflectedBeams
        {
            get { return reflectedBeams; }
        }

        public MirrorType Type
        {
            get { return type; }
        }

        public Rectangle Hitbox
        {
            get { return hitbox; }
        }

        // Methods

        /// <summary>
        /// Checks if a mirror and a LightBeam are colliding.
        /// </summary>
        /// <param name="obj">Reference to a game object.</param>
        /// <returns>True if the collision occurs AND the object is a LightBeam, otherwise false.</returns>
        private bool BeamIsColliding(GameObject obj)
        {
            return Position.Intersects(obj.Position) && obj is LightBeam;
        }

        /// <summary>
        /// Checks for a collision between an object and the mirror.
        /// If the object is a LightBeam,
        /// check if the mirror's position intersects the object's.
        /// Otherwise, check if the mirror's hitbox intersects the object's position.
        /// </summary>
        /// <param name="obj">Reference to a game object</param>
        /// <returns>True if the collision occurs, otherwise false.</returns>
        public override bool IsColliding(GameObject obj)
        {
            if (obj is not LightBeam)
            {
                return hitbox.Intersects(obj.Position);
            }
            else
            {
                return BeamIsColliding(obj);
            }
        }

        /// <summary>
        /// Draw method that uses the base class's method.
        /// </summary>
        /// <param name="sb">SpriteBatch object to draw with.</param>
        public override void Draw(SpriteBatch sb)
        {
            // shorthand if/else that flips mirror sprite horizontally depending on type
            SpriteEffects fx = (type == MirrorType.Forward) ? 
                SpriteEffects.FlipHorizontally : 
                SpriteEffects.None;

            sb.Draw(spriteSheet, positionRect, null, Color.White, 0, Vector2.Zero, fx, 0);
        }

        /// <summary>
        /// When the player and a mirror are colliding and the E key is pressed,
        /// the player holds the mirror until the E key is pressed again.
        /// </summary>
        /// <param name="myPlayer">Reference to Game1's player.</param>
        /// <param name="currentState">Current state of the keyboard.</param>
        /// <param name="previousState">Previous state of the keyboard.</param>
        public void Interact(Player myPlayer, KeyboardState currentState, KeyboardState previousState)
        {

        }
    }
}
