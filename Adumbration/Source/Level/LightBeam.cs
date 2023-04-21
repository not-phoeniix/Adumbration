using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Penumbra;
using SharpDX.Direct2D1.Effects;
using SharpDX.WIC;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Runtime.InteropServices;

namespace Adumbration
{
    /// <summary>
    /// Determines direction of the light beam
    /// </summary>
    public enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }

    /// <summary>
    /// a lightbeam class so we have an object to check
    /// for if the user runs into something
    /// it will teleport them back to a starting point
    /// </summary>
    internal class LightBeam : GameObject
    {
        // Fields

        // Beam specific Fields
        private Direction dir;
        private List<Light> lights;
        private Texture2D texture;
        private bool isReflected;
        private int expandSpeed;

        // Previous Position of Rectangle each frame
        private Rectangle prevPosition;

        // Original X and Y of Beam rectangle
        private int originalX;
        private int originalY;

        // Mirrors that the light beam interacts with
        private Mirror associatedMirror;
        private Mirror collidedMirror;

        // The beam's own reflected beam
        private LightBeam reflectedBeam;

        #region // Properties

        /// <summary>
        /// Direction the beam is shooting
        /// </summary>
        public Direction Direction
        {
            get { return dir; }
        }

        /// <summary>
        ///  List of all Lights on the light beam
        /// </summary>
        public List<Light> Lights
        {
            get { return lights; }
        }

        /// <summary>
        /// Width of the light beam rectangle
        /// </summary>
        public int Width
        {
            get { return positionRect.Width; }
            set { positionRect.Width = value; }
        }

        /// <summary>
        /// Height of the light beam rectangle
        /// </summary>
        public int Height
        {
            get { return positionRect.Height; }
            set { positionRect.Height = value; }
        }

        /// <summary>
        /// The mirror the light beam is reflecting
        /// off of
        /// </summary>
        public Mirror AssociatedMirror
        {
            get { return associatedMirror; }
            set { associatedMirror = value; }
        }

        /// <summary>
        /// The Beam's own reflected beam
        /// </summary>
        public LightBeam ReflectedBeam
        {
            get { return reflectedBeam; }
        }

        /// <summary>
        /// Whether or not the beam has changed from the last frame
        /// </summary>
        public bool HasChanged
        {
            get { return prevPosition != positionRect; }
        }

        #endregion
        // Constructor
        /// <summary>
        /// Constructor for the Light Beam
        /// </summary>
        /// <param name="texture">White Pixel texture used for beam</param>
        /// <param name="position">Position of the Light Beam</param>
        /// <param name="dir">Direction Light Beam is shooting</param>
        public LightBeam(Texture2D texture, Rectangle position, Direction dir)
             : base(texture, new Rectangle(0, 0, 1, 1), position)
        {
            this.dir = dir;
            lights = new List<Light>();
            this.texture = texture;

            // Each non reflected Beam starts off 
            // with none of these fields
            reflectedBeam = null;
            associatedMirror = null;
            collidedMirror = null;
            isReflected = false;

            // How fast the beam expands
            expandSpeed = 3;

            // Original X and Y of beam rectangle
            originalX = position.X;
            originalY = position.Y;
        }

        /// <summary>
        /// Secondary Constructor for a new reflected beam
        /// </summary>
        /// <param name="texture">White Pixel texture used for beam</param>
        /// <param name="position">Position of the Light Beam</param>
        /// <param name="dir">Direction Light Beam is shooting</param>
        /// <param name="associatedMirror">The mirror this beam is reflecting off of</param>
        public LightBeam(Texture2D texture, Rectangle position, Direction dir, Mirror associatedMirror)
             : base(texture, new Rectangle(0, 0, 1, 1), position)
        {
            this.dir = dir;
            lights = new List<Light>();
            this.texture = texture;

            // Each non reflected Beam starts off 
            // with none of these fields
            isReflected = false;
            reflectedBeam = null;

            // The only filled in Mirror field is it's associated mirror
            this.associatedMirror = associatedMirror;
            collidedMirror = null;

            // Expansion speed of beam
            expandSpeed = 3;

            // Original X and Y of beam rectangle
            originalX = position.X;
            originalY = position.Y;
        }

        /// <summary>
        /// Updates beams rectangle per frame
        /// </summary>
        /// <param name="gameTime">The game's time</param>
        public override void Update(GameTime gameTime)   // May have to debug when implementing mirrors
        {
            // Level the player is currently on
            Level currentLevel = LevelManager.Instance.CurrentLevel;

            #region // Light size extending
            // Check the direction of the light beam first
            switch (dir)
            {
                case Direction.Left:
                    // Expand left without moving origin
                    positionRect.X -= expandSpeed;
                    positionRect.Width += expandSpeed;

                    foreach (GameObject tile in currentLevel.TileList)
                    {
                        // If it is colliding with a wall
                        if (tile is Wall && tile is not LightEmitter && IsColliding(tile))
                        {
                            // Stop expansion
                            positionRect.X = tile.Position.X + tile.Position.Width;
                            positionRect.Width -= expandSpeed;
                        }
                    }

                    foreach (Mirror mirror in currentLevel.Mirrors)
                    {
                        // If the beam is colliding with any mirror that isn't it's associated mirror
                        // and it is not reflecting
                        if (IsColliding(mirror) && mirror != associatedMirror && !isReflected)
                        {
                            // It is now reflecting
                            isReflected = true;

                            // It is now colliding with a specific mirror
                            collidedMirror = mirror;

                            // Make new beam dependent on mirror
                            if (mirror.Type == MirrorType.Backward)
                            {
                                reflectedBeam = new LightBeam(texture,
                                 new Rectangle(this.X, this.Y, 2, 2),
                                 Direction.Up, mirror);
                            }
                            else
                            {
                                reflectedBeam = new LightBeam(texture,
                                 new Rectangle(this.X, this.Y, 2, 2),
                                 Direction.Down, mirror);
                            }
                        }

                        // If it is no longer colliding with a mirror
                        // and the mirror was the mirror it collided with
                        if (!IsColliding(mirror) && mirror == collidedMirror)
                        {
                            // It's no longer reflecting
                            isReflected = false;

                            // Removes the reflected beam from the Level's
                            // List of beams
                            if (currentLevel.Beams.Contains(reflectedBeam))
                            {
                                currentLevel.Beams.Remove(reflectedBeam);
                            }

                            // Then removed reflected beam
                            reflectedBeam = null;
                        }

                        // If it is colliding with a mirror and is reflecting
                        if (IsColliding(mirror) && isReflected)
                        {
                            // Stop expansion
                            positionRect.Width = originalX - (mirror.X + mirror.Position.Width);
                            positionRect.X = mirror.X + mirror.Position.Width;
                        }

                        // If the reflected beam exists and it's position has changed
                        if (reflectedBeam != null && HasChanged)
                        {
                            // Change the reflected Beam's position
                            reflectedBeam.X = positionRect.X;
                        }
                    }

                    break;

                case Direction.Right:
                    // Expand right without moving origin
                    positionRect.Width += expandSpeed;

                    foreach (GameObject tile in currentLevel.TileList)
                    {
                        // If it is colliding with a wall
                        if (tile is Wall && tile is not LightEmitter && IsColliding(tile))
                        {
                            // Stop expansion
                            positionRect.Width = tile.X - positionRect.X;
                        }
                    }

                    foreach (Mirror mirror in currentLevel.Mirrors)
                    {
                        // If the beam is colliding with any mirror that isn't it's associated mirror
                        // and it is not reflecting
                        if (IsColliding(mirror) && mirror != associatedMirror && !isReflected)
                        {
                            // It is now reflecting
                            isReflected = true;

                            // It is now colliding with a specific mirror
                            collidedMirror = mirror;

                            // Make new beam dependent on mirror
                            if (mirror.Type == MirrorType.Backward)
                            {
                                reflectedBeam = new LightBeam(texture,
                                 new Rectangle(this.X + Width, this.Y, 2, 2),
                                 Direction.Down, mirror);
                            }
                            else
                            {
                                reflectedBeam = new LightBeam(texture,
                                 new Rectangle(this.X + this.Width, this.Y, 2, 2),
                                 Direction.Up, mirror);
                            }
                        }

                        // If it is no longer colliding with a mirror
                        // and the mirror was the mirror it collided with
                        if (!IsColliding(mirror) && mirror == collidedMirror)
                        {
                            // It's no longer reflecting
                            isReflected = false;

                            // Removes the reflected beam from the Level's
                            // List of beams
                            if (currentLevel.Beams.Contains(reflectedBeam))
                            {
                                currentLevel.Beams.Remove(reflectedBeam);
                            }

                            // Then removed reflected beam
                            reflectedBeam = null;
                        }

                        // If it is colliding with a mirror and is reflecting
                        if (IsColliding(mirror) && isReflected)
                        {
                            // Stop Expansion
                            positionRect.Width = mirror.X - positionRect.X;
                        }

                        // If the reflected beam exists and it's position has changed
                        if (reflectedBeam != null && HasChanged)
                        {
                            // Change the reflected Beam's position
                            reflectedBeam.X = positionRect.X + positionRect.Width;
                        }
                    }
                    break;

                case Direction.Up:
                    // Expand Up without moving origin
                    positionRect.Y -= expandSpeed;
                    positionRect.Height += expandSpeed;

                    foreach (GameObject tile in currentLevel.TileList)
                    {
                        // If it is colliding with a wall
                        if (tile is Wall && tile is not LightEmitter && IsColliding(tile))
                        {
                            // Stop expansion 
                            positionRect.Y = tile.Position.Y + tile.Position.Height;
                            positionRect.Height -= expandSpeed;
                        }
                    }

                    foreach (Mirror mirror in currentLevel.Mirrors)
                    {
                        // If the beam is colliding with any mirror that isn't it's associated mirror
                        // and it is not reflecting
                        if (IsColliding(mirror) && mirror != associatedMirror && !isReflected)
                        {
                            // It is now reflecting
                            isReflected = true;

                            // It is now colliding with a specific mirror
                            collidedMirror = mirror;

                            // Make new beam dependent on mirror
                            if (mirror.Type == MirrorType.Backward)
                            {
                                reflectedBeam = new LightBeam(texture,
                                 new Rectangle(this.X, this.Y, 2, 2),
                                 Direction.Left, mirror);
                            }
                            else
                            {
                                reflectedBeam = new LightBeam(texture,
                                 new Rectangle(this.X, this.Y, 2, 2),
                                 Direction.Right, mirror);
                            }
                        }

                        // If it is no longer colliding with a mirror
                        // and the mirror was the mirror it collided with
                        if (!IsColliding(mirror) && mirror == collidedMirror)
                        {
                            // It's no longer reflecting
                            isReflected = false;

                            // Removes the reflected beam from the Level's
                            // List of beams
                            if (currentLevel.Beams.Contains(reflectedBeam))
                            {
                                currentLevel.Beams.Remove(reflectedBeam);
                            }

                            // Then removed reflected beam
                            reflectedBeam = null;
                        }

                        // If it is colliding with a mirror and is reflecting
                        if (IsColliding(mirror) && isReflected)
                        {
                            // Stop expansion
                            positionRect.Height = originalY - (mirror.Y + mirror.Position.Height);
                            positionRect.Y = mirror.Y + mirror.Position.Height;
                        }

                        // If the reflected beam exists and it's position has changed
                        if (reflectedBeam != null && HasChanged)
                        {
                            // Change the reflected Beam's position
                            reflectedBeam.Y = positionRect.Y;
                        }
                    }
                    break;

                case Direction.Down:
                    // Expand Down without moving origin
                    positionRect.Height += expandSpeed;

                    foreach (GameObject tile in currentLevel.TileList)
                    {
                        // If it is colliding with a wall
                        if (tile is Wall && tile is not LightEmitter && IsColliding(tile))
                        {
                            // Stop expansion
                            positionRect.Height = tile.Y - positionRect.Y;
                        }
                    }

                    foreach (Mirror mirror in currentLevel.Mirrors)
                    {
                        // If the beam is colliding with any mirror that isn't it's associated mirror
                        // and it is not reflecting
                        if (IsColliding(mirror) && mirror != associatedMirror && !isReflected)
                        {
                            // It is now reflecting
                            isReflected = true;

                            // It is now colliding with a specific mirror
                            collidedMirror = mirror;

                            // Make new beam dependent on mirror
                            if (mirror.Type == MirrorType.Backward)
                            {
                                reflectedBeam = new LightBeam(texture,
                                 new Rectangle(this.X, this.Y + this.Height, 2, 2),
                                 Direction.Right, mirror);
                            }
                            else
                            {
                                reflectedBeam = new LightBeam(texture,
                                 new Rectangle(this.X, this.Y + this.Height, 2, 2),
                                 Direction.Left, mirror);
                            }
                        }

                        // If it is no longer colliding with a mirror
                        // and the mirror was the mirror it collided with
                        if (!IsColliding(mirror) && mirror == collidedMirror)
                        {
                            // It's no longer reflecting
                            isReflected = false;

                            // Removes the reflected beam from the Level's
                            // List of beams
                            if (currentLevel.Beams.Contains(reflectedBeam))
                            {
                                currentLevel.Beams.Remove(reflectedBeam);
                            }

                            // Then removed reflected beam
                            reflectedBeam = null;
                        }

                        // If it is colliding with a mirror and is reflecting
                        if (IsColliding(mirror) && isReflected)
                        {
                            // Stop expansion
                            positionRect.Height = mirror.Y - positionRect.Y;
                        }

                        // If the reflected beam exists and it's position has changed
                        if (reflectedBeam != null && HasChanged)
                        {
                            // Change the reflected Beam's position
                            reflectedBeam.Y = positionRect.Y + positionRect.Height;
                        }                        
                    }
                    break;
            }

            #endregion
            
            #region // Adding point lights across beam

            // light properties (easy to tweak)
            float radius = 1;
            float intensity = 0.1f;
            int skipNum = 10;
            
            // only deletes and recreates lights along beam if the position changes
            if(HasChanged)
            {
                lights.Clear();

                // up and down facing lights
                if(dir == Direction.Up || dir == Direction.Down)
                {
                    for(int h = 0; h < positionRect.Height; h++)
                    {
                        // creates light object
                        PointLight light = new PointLight()
                        {
                            Radius = radius,
                            Intensity = intensity,
                            Position = new Vector2(positionRect.X + positionRect.Width / 2, positionRect.Y + h)
                        };

                        // only adds the light every few pixels (determined by skip num)
                        if(h % skipNum == 0)
                        {
                            lights.Add(light);
                        }
                    }
                }

                // left and right facing lights
                else
                {
                    for(int w = 0; w < positionRect.Width; w++)
                    {
                        // creates light object
                        PointLight light = new PointLight()
                        {
                            Radius = radius,
                            Intensity = intensity,
                            Position = new Vector2(positionRect.X + w, positionRect.Y + positionRect.Height / 2)
                        };

                        // only adds the light every few pixels (determined by skip num)
                        if(w % skipNum == 0)
                        {
                            lights.Add(light);
                        }
                    }
                }

            }

            #endregion

            // updates previous position rectangle,
            // kinda like a previous state update
            prevPosition = positionRect;
        }

        #region// ================ EXPERIMENTAL RECURSIVE REFLECTION METHODS ================
        private void UpdateBeam(LightBeam beam, GameTime gameTime)
        {
            if( reflectedBeam != null)
            {
                UpdateBeam(reflectedBeam, gameTime);
            }

            this.Update(gameTime);
        }

        public void UpdateBeam(GameTime gameTime)
        {
            UpdateBeam(this, gameTime);
        }

        //private void Draw(SpriteBatch sb, LightBeam beam)
        //{
        //    if(reflectedBeam != null)
        //    {
        //        Draw(sb, reflectedBeam);
        //    } 
        //    base.Draw(sb);
        //}

        //public override void Draw(SpriteBatch sb)
        //{
        //    Draw(sb, this);
        //}
        #endregion

        /// <summary>
        /// the main colliding method like all the other gameobject 
        /// children classes
        /// </summary>
        /// <param name="obj">
        /// checks the object it is colliding with
        /// in this case, most likely be used along with the player
        /// </param>
        /// <returns></returns>
        public override bool IsColliding(GameObject obj)
        {
            if (Position.Intersects(obj.Position))
            {
                return true;
            }

            return false;
        }
    }
}
