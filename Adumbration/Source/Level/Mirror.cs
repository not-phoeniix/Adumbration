using System;
using System.Collections.Generic;
using System.Linq;
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
        private LightBeam reflectedBeam;
        private MirrorType type;

        //constructor for this class
        public Mirror(Texture2D spriteSheet, Rectangle sourceRect, Rectangle position, MirrorType type)
             : base(spriteSheet, sourceRect, position)
        {
            this.type = type;
            reflectedBeam = null;
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
            foreach(LightBeam beam in currentLevel.Beams)
            {
                if (IsColliding(beam))
                {
                    CreateReflection(beam);
                    System.Diagnostics.Debug.WriteLine("collision");
                }
            }
        }

        public void CreateReflection(LightBeam beam)
        {
            switch (type)
            {
                // If the mirror is the forward facing type:
                case MirrorType.Forward:       
                    
                    // Determine beam direction
                    switch (beam.Direction)
                    {
                        case Direction.Up:
                            reflectedBeam = new LightBeam(spriteSheet, 
                                new Rectangle(beam.X, beam.Y, 2, 2), 
                                Direction.Right);
                            break;

                        case Direction.Down:
                            reflectedBeam = new LightBeam(spriteSheet,
                               new Rectangle(beam.X, beam.Y + beam.Height, 2, 2),
                               Direction.Left);
                            break;

                        case Direction.Right:
                            reflectedBeam = new LightBeam(spriteSheet,
                               new Rectangle(beam.X + beam.Width, beam.Y, 2, 2),
                               Direction.Up);
                            break;

                        case Direction.Left:
                            reflectedBeam = new LightBeam(spriteSheet,
                               new Rectangle(beam.X, beam.Y, 2, 2),
                               Direction.Down);
                            break;
                    }
                    break;

                case MirrorType.Backward:
                    // Determine beam direction
                    switch (beam.Direction)
                    {
                        case Direction.Up:
                            reflectedBeam = new LightBeam(spriteSheet,
                                new Rectangle(beam.X, beam.Y - beam.Height, 2, 2),
                                Direction.Left);
                            break;

                        case Direction.Down:
                            reflectedBeam = new LightBeam(spriteSheet,
                               new Rectangle(beam.X, beam.Y + beam.Height, 2, 2),
                               Direction.Right);
                            break;

                        case Direction.Right:
                            reflectedBeam = new LightBeam(spriteSheet,
                               new Rectangle(beam.X + beam.Width, beam.Y, 2, 2),
                               Direction.Down);
                            break;

                        case Direction.Left:
                            reflectedBeam = new LightBeam(spriteSheet,
                               new Rectangle(beam.X, beam.Y, 2, 2),
                               Direction.Up);
                            break;
                    }
                    break;
            }
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
    }
}
