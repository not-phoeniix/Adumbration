using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adumbration
{
    /// <summary>
    /// Alexander Gough & Nikki Murello
    /// Purpose: Set up each level
    /// </summary>
    public class Level
    {
        // Fields
        private GameObject[,] tileList;

        // Constructor
        public Level(Texture2D spritesheet)
        {
            // testing level drawing without loading from file yet, will remove later
            
            // initializing array size
            tileList = new GameObject[10, 10];

            int arrayHeight = tileList.GetLength(1);
            int arrayWidth = tileList.GetLength(0);

            // fills array with floor objects
            for(int y = 0; y < arrayHeight; y++) 
            {
                for(int x = 0; x < arrayWidth; x++) 
                {
                    Rectangle sourceRect = new Rectangle(0, 0, 16, 16);
                    int tileScale = 6;

                    // makes the top and bottom rows one sprite
                    int sideSize = sourceRect.Width * tileScale;

                    tileList[x, y] = new Floor(
                    spritesheet,                    // spritesheet
                    sourceRect,                     // source
                    new Rectangle(                  // position
                        x * sideSize,
                        y * sideSize,
                        sideSize,
                        sideSize)
                    );
                }
            }
        }


        // Methods

        /// <summary>
        /// Resets a level.
        /// </summary>
        public void ResetLevel()
        {

        }

        /// <summary>
        /// Updates the level's state of the game.
        /// </summary>
        /// <param name="gameTime">State of the game's time.</param>
        public void Update(GameTime gameTime)
        {

        }

        /// <summary>
        /// Draws object(s) in level.
        /// </summary>
        /// <param name="gameTime">State of the game's time.</param>
        public void Draw(SpriteBatch sb)
        {
            // this loop draws all objects in the tileList array

            // height/y-dimension being drawn
            for(int y = 0; y < tileList.GetLength(1); y++) 
            {
                // width/x-dimension being drawn
                for(int x = 0; x < tileList.GetLength(0); x++) 
                {
                    tileList[x, y].Draw(sb);
                }
            }
        }
    }
}
