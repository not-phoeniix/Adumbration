﻿using System;
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
        private List<LightBeam> reflectedBeams;
        private MirrorType type;
        private bool isLightColliding;
        private bool lightAlreadyCollided;
        private Texture2D whitePixelTexture;

        //constructor for this class
        public Mirror(Dictionary<string, Texture2D> textureDict, Rectangle position, MirrorType type)
             : base(
                   textureDict["mirror"],
                   new Rectangle(
                       0,
                       0,
                       textureDict["mirror"].Width,
                       textureDict["mirror"].Height
                       ),
                   position)
        {
            this.type = type;
            whitePixelTexture = textureDict["whitePixel"];
            reflectedBeams = new List<LightBeam>();
            isLightColliding = false;
            lightAlreadyCollided = false;
        }

        /// <summary>
        /// Returns the light beam attatched to this mirror, null if non existant
        /// </summary>
        public List<LightBeam> ReflectedBeams
        {
            get { return reflectedBeams; }
        }

        public MirrorType MirrorType
        {
            get { return type; }
        }

        public void Update(GameTime gameTime, Level currentLevel)
        {
            // For each light beam in the level
            foreach (LightBeam beam in currentLevel.Beams)
            {
                // If it collides with the Mirror
                if (IsColliding(beam) && !isLightColliding)
                {
                    // The mirror creates a new reflection
                    System.Diagnostics.Debug.WriteLine("collision");
                    reflectedBeams.Add(CreateReflection(beam));
                    isLightColliding = true;
                }
            }

            foreach (LightBeam beam in reflectedBeams)
            {
                beam?.Update(gameTime, currentLevel);
            }
        }

        /// <summary>
        /// Creates a reflected light beam
        /// </summary>
        /// <param name="beam">The beam that is causin the reflection</param>
        public LightBeam CreateReflection(LightBeam incomingBeam)
        {
            LightBeam returnBeam = null;

            switch (type)
            {
                // If the mirror is the forward facing type:
                case MirrorType.Forward:

                    // Determine beam direction then create new
                    // reflected beam properly
                    switch (incomingBeam.Direction)
                    {
                        case Direction.Up:
                            returnBeam = new LightBeam(whitePixelTexture,
                                new Rectangle(incomingBeam.X, incomingBeam.Y - incomingBeam.Height, 2, 2),
                                Direction.Right);
                            break;

                        case Direction.Down:
                            returnBeam = new LightBeam(whitePixelTexture,
                               new Rectangle(incomingBeam.X, incomingBeam.Y + incomingBeam.Height, 2, 2),
                               Direction.Left);
                            break;

                        case Direction.Right:
                            returnBeam = new LightBeam(whitePixelTexture,
                               new Rectangle(incomingBeam.X - incomingBeam.Width, positionRect.Y, 2, 2),
                               Direction.Up);
                            break;

                        case Direction.Left:
                            returnBeam = new LightBeam(whitePixelTexture,
                               new Rectangle(incomingBeam.X, incomingBeam.Y, 2, 2),
                               Direction.Down);
                            break;
                    }
                    break;

                // If the mirror is the backwards facing type:
                case MirrorType.Backward:

                    // Determine beam direction then create new
                    // reflected beam properly
                    switch (incomingBeam.Direction)
                    {
                        case Direction.Up:
                            returnBeam = new LightBeam(whitePixelTexture,
                                new Rectangle(incomingBeam.X, incomingBeam.Y, 2, 2),
                                Direction.Left);
                            break;

                        case Direction.Down:
                            returnBeam = new LightBeam(whitePixelTexture,
                               new Rectangle(incomingBeam.X, incomingBeam.Y + incomingBeam.Height, 2, 2),
                               Direction.Right);
                            break;

                        case Direction.Right:
                            returnBeam = new LightBeam(whitePixelTexture,
                               new Rectangle(incomingBeam.X + incomingBeam.Width, incomingBeam.Y, 2, 2),
                               Direction.Down);
                            break;

                        case Direction.Left:
                            returnBeam = new LightBeam(whitePixelTexture,
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