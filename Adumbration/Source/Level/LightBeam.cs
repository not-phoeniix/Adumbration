using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Penumbra;
using SharpDX.Direct2D1.Effects;
using System.Collections.Generic;
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
        private Direction dir;
        private List<Light> lights;
        private Texture2D texture;
        private LightBeam reflectedBeam;
        private bool isReflected;
        private Mirror associatedMirror;
        private int expandSpeed;
        private Rectangle prevPosition;

        #region // Properties

        /// <summary>
        /// Direction the beam is facing
        /// </summary>
        public Direction Direction
        {
            get { return dir; }
        }

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

        public Mirror AssociatedMirror
        {
            get { return associatedMirror; }
            set { associatedMirror = value; }
        }

        public LightBeam ReflectedBeam
        {
            get { return reflectedBeam; }
        }

        public bool HasChanged
        {
            get { return prevPosition != positionRect; }
        }

        #endregion

        //constructor for this class
        public LightBeam(Texture2D texture, Rectangle position, Direction dir)
             : base(texture, new Rectangle(0, 0, 1, 1), position)
        {
            this.dir = dir;
            lights = new List<Light>();
            this.texture = texture;
            isReflected = false;
            reflectedBeam = null;
            associatedMirror = null;
            
            expandSpeed = 1;
        }

        public LightBeam(Texture2D texture, Rectangle position, Direction dir, Mirror associatedMirror)
             : base(texture, new Rectangle(0, 0, 1, 1), position)
        {
            this.dir = dir;
            lights = new List<Light>();
            this.texture = texture;
            isReflected = false;
            reflectedBeam = null;
            this.associatedMirror = associatedMirror;

            expandSpeed = 1;
        }

        /// <summary>
        /// Updates beams rectangle per frame
        /// </summary>
        /// <param name="gameTime">The game's time</param>
        /// <param name="currentLevel">The level the player is currently on</param>
        public void Update(GameTime gameTime)   // May have to debug when implementing mirrors
        {
            Level currentLevel = LevelManager.Instance.CurrentLevel;

            // Clears all previous spotlights every frame
            //lights.Clear();

            // Check the direction of the light beam first
            #region // Light size extending

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

                        // If it is colliding with a mirror and is reflecting
                        if (IsColliding(mirror) && isReflected)
                        {
                            // Stop expansion
                            positionRect.X = mirror.Position.X + mirror.Position.Width;
                            positionRect.Width -= expandSpeed;
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

                        // If it is colliding with a mirror and is reflecting
                        if (IsColliding(mirror) && isReflected)
                        {
                            // Stop Expansion
                            positionRect.Width -= expandSpeed;
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

                        // If it is colliding with a mirror and is reflecting
                        if (IsColliding(mirror) && isReflected)
                        {
                            // Stop expansion
                            positionRect.Y = mirror.Position.Y + mirror.Position.Height;
                            positionRect.Height -= expandSpeed;
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

                        // If it is colliding with a mirror and is reflecting
                        if (IsColliding(mirror) && isReflected)
                        {
                            // Stop expansion
                            positionRect.Height -= expandSpeed;
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
