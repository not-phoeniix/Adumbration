using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
        Direction dir;

        // Properties
        public Direction Direction
        {
            get { return dir; }
        }

        //constructor for this class
        public LightBeam(Texture2D texture, Rectangle position, Direction dir)
             : base(texture, new Rectangle(0, 0, 1, 1), position)
        {
            this.dir = dir;
        }

        /// <summary>
        /// Width of the light beam rectangle
        /// </summary>
        public int Width
        {
            get { return positionRect.Width; }
        }

        /// <summary>
        /// Height of the light beam rectangle
        /// </summary>
        public int Height
        {
            get { return positionRect.Height; }
        }

        /// <summary>
        /// Updates beams rectangle per frame
        /// </summary>
        /// <param name="gameTime">The game's time</param>
        /// <param name="currentLevel">The level the player is currently on</param>
        public void Update(GameTime gameTime, Level currentLevel)   // May have to debug when implementing mirrors
        {
            // Check the direction of the light beam first
            switch (dir)
            {
                case Direction.Left:
                    // Expand left without moving origin
                    positionRect.X -= 1;
                    positionRect.Width += 1;

                    foreach (GameObject tile in currentLevel.TileList)
                    {
                        // If it is colliding with a wall
                        if (tile is Wall && tile is not LightEmitter && IsColliding(tile))
                        {
                            // Stop expansion
                            positionRect.X = tile.Position.X + tile.Position.Width;
                            positionRect.Width -= 1;
                        }
                    }
                    break;

                case Direction.Right:
                    // Expand right without moving origin
                    positionRect.Width += 1;

                    foreach (GameObject tile in currentLevel.TileList)
                    {
                        // If it is colliding with a wall
                        if (tile is Wall && tile is not LightEmitter && IsColliding(tile))
                        {
                            // Stop expansion
                            positionRect.Width -= 1;
                        }
                    }
                    break;

                case Direction.Up:
                    // Expand Up without moving origin
                    positionRect.Y -= 1;
                    positionRect.Height += 1;

                    foreach (GameObject tile in currentLevel.TileList)
                    {
                        // If it is colliding with a wall
                        if (tile is Wall && tile is not LightEmitter && IsColliding(tile))
                        {
                            // Stop expansion 
                            positionRect.Y = tile.Position.Y + tile.Position.Height;
                            positionRect.Height -= 1;
                        }
                    }
                    break;

                case Direction.Down:
                    // Expand Down without moving origin
                    positionRect.Height += 1;

                    foreach (GameObject tile in currentLevel.TileList)
                    {
                        // If it is colliding with a wall
                        if (tile is Wall && tile is not LightEmitter && IsColliding(tile))
                        {
                            // Stop expansion
                            positionRect.Height -= 1;
                        }
                    }
                    break;
            }
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
