using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

// Describes the two types of mirrors we have
public enum MirrorType
{
    Forward,
    Backward
}

namespace Adumbration
{
    internal class Mirror : GameObject
    {
        // Fields
        private LightBeam reflectedBeam;
        private MirrorType type;
        private bool isLightColliding;
        private Texture2D wallTexture;

        //constructor for this class
        public Mirror(Texture2D spriteSheet, Texture2D wallTexture, Rectangle sourceRect, Rectangle position, MirrorType type)
             : base(spriteSheet, sourceRect, position)
        {
            this.type = type;
            this.wallTexture = wallTexture;
            reflectedBeam = null;
            isLightColliding = false;
        }

        /// <summary>
        /// Returns the light beam attatched to this mirror, null if non existant
        /// </summary>
        public LightBeam Beam
        {
            get { return reflectedBeam; }
        }

        public void Update(GameTime gameTime, Level currentLevel)
        {
            // For each light beam in the level
            foreach(LightBeam beam in currentLevel.Beams)
            {
                // If it collides with the Mirror
                if (IsColliding(beam) && !isLightColliding)
                {
                    // The mirror creates a new reflection
                    System.Diagnostics.Debug.WriteLine("collision");
                    isLightColliding = true;
                    reflectedBeam = CreateReflection(beam.Direction);
                }
            }
            reflectedBeam?.Update(gameTime, currentLevel);
        }

        /// <summary>
        /// Creates a reflected light beam
        /// </summary>
        /// <param name="beam">The beam that is causin the reflection</param>
        public LightBeam CreateReflection(Direction beamDirection)
        {
            LightBeam returnBeam = null;

            switch (type)
            {
                // If the mirror is the forward facing type:
                case MirrorType.Forward:

                    // Determine beam direction then create new
                    // reflected beam properly
                    switch (beamDirection)
                    {
                        case Direction.Up:
                            returnBeam = new LightBeam(wallTexture, 
                                new Rectangle(positionRect.X, positionRect.Y, 2, 2), 
                                Direction.Right);
                            break;

                        case Direction.Down:
                            returnBeam = new LightBeam(wallTexture,
                               new Rectangle(positionRect.X, positionRect.Y, 2, 2),
                               Direction.Left);
                            break;

                        case Direction.Right:
                            returnBeam = new LightBeam(wallTexture,
                               new Rectangle(positionRect.X, positionRect.Y, 2, 2),
                               Direction.Up);
                            break;

                        case Direction.Left:
                            returnBeam = new LightBeam(wallTexture,
                               new Rectangle(positionRect.X, positionRect.Y, 2, 2),
                               Direction.Down);
                            break;
                    }
                    break;

                // If the mirror is the backwards facing type:
                case MirrorType.Backward:

                    // Determine beam direction then create new
                    // reflected beam properly
                    switch (beamDirection)
                    {
                        case Direction.Up:
                            returnBeam = new LightBeam(wallTexture,
                                new Rectangle(positionRect.X, positionRect.Y, 2, 2),
                                Direction.Left);
                            break;

                        case Direction.Down:
                            returnBeam = new LightBeam(wallTexture,
                               new Rectangle(positionRect.X, positionRect.Y, 2, 2),
                               Direction.Right);
                            break;

                        case Direction.Right:
                            returnBeam = new LightBeam(wallTexture,
                               new Rectangle(positionRect.X, positionRect.Y, 2, 2),
                               Direction.Down);
                            break;

                        case Direction.Left:
                            returnBeam = new LightBeam(wallTexture,
                               new Rectangle(positionRect.X, positionRect.Y, 2, 2),
                               Direction.Up);
                            break;
                    }
                    break;
            }

            return returnBeam;
        }

        public override bool IsColliding(GameObject obj)
        {
            if (Position.Intersects(obj.Position) && obj is LightBeam)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
        }
    }
}
