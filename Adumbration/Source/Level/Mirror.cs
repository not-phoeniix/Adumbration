﻿using System;
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
        private bool isLightColliding;
        private bool lightAlreadyCollided;
        private Texture2D whitePixelTexture;
        private Rectangle hitbox;

        // Constructor

        /// <summary>
        /// Parameterized constructor for the mirror class.
        /// </summary>
        /// <param name="textureDict">The dictionary to get textures from.</param>
        /// <param name="position">The game position.</param>
        /// <param name="type">The type of mirror.</param>
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

        public MirrorType MirrorType
        {
            get { return type; }
        }

        public Rectangle Hitbox
        {
            get { return hitbox; }
        }

        // Methods

        /// <summary>
        /// Updates the mirror
        /// </summary>
        /// <param name="gameTime">State of the game's time.</param>
        /// <param name="currentLevel">The current level.</param>
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
        /// <param name="incomingBeam">The beam that is causing the reflection.</param>
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
            base.Draw(sb);
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
