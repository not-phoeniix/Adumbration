using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using System.IO;

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
        private Texture2D spritesheet;
        private int levelScale;

        // Constructor
        public Level(Texture2D spritesheet, int levelScale, string dataFilePath)
        {
            this.spritesheet = spritesheet;
            this.levelScale = levelScale;
            LoadFromFile(dataFilePath);
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
            for (int y = 0; y < tileList.GetLength(1); y++)
            {
                // width/x-dimension being drawn
                for (int x = 0; x < tileList.GetLength(0); x++)
                {
                    tileList[x, y].Draw(sb);
                }
            }
        }

        /// <summary>
        /// Loads a level from a file and initializes array
        /// </summary>
        /// <param name="filename">String of file name</param>
        private void LoadFromFile(string filename) 
        {
            StreamReader reader = null;

            // try catch for all stream reading in case
            //   there's an error with reading
            try
            {
                // makes reader object
                reader = new StreamReader(filename);

                string lineString = "";
                int lineNum = 1;
                int levelWidth, levelHeight;

                // loops through every line until reading null
                while((lineString = reader.ReadLine()) != null)
                {
                    // splits the line at beginning of read
                    string[] splitString = lineString.Split(",");

                    // sets array size & initializes ==========================
                    if(lineNum == 1) 
                    {
                        levelWidth = int.Parse(splitString[0]);
                        levelHeight = int.Parse(splitString[1]);

                        // initializes the list
                        tileList = new GameObject[levelWidth, levelHeight];
                    }

                    // loading level & filling array ==========================
                    else
                    {
                        // fills row in array with the split string
                        for(int i = 0; i < splitString.Length; i++) 
                        {
                            // current coords in array
                            int arrayX = i;
                            int arrayY = lineNum - 2;

                            // takes the "2|0" and converts to just a "2" and a "0"
                            //string[] tileCoords = splitString[i].Trim().Split("|");
                            //int textureWidth = 16;
                            //int sourceX = int.Parse(tileCoords[0]) * textureWidth;
                            //int sourceY = int.Parse(tileCoords[1]) * textureWidth;

                            //Rectangle sourceRect = new Rectangle(
                            //    sourceX, 
                            //    sourceY, 
                            //    textureWidth, 
                            //    textureWidth);

                            Rectangle sourceRect = DetermineSprite(int.Parse(splitString[i]));

                            int sideSize = sourceRect.Width * levelScale;
                            
                            Rectangle positionRect = new Rectangle(
                                arrayX * sideSize,
                                arrayY * sideSize,
                                sideSize,
                                sideSize);

                            if(arrayX == 1 && arrayY == 1) {
                                tileList[arrayX, arrayY] = new Floor(spritesheet, sourceRect, positionRect);
                            } else {
                                tileList[arrayX, arrayY] = new Wall(spritesheet, sourceRect, positionRect);
                            }
                        }
                    }

                    // increments lineNum after each loop
                    lineNum++;
                }

            } 
            catch(Exception ex)
            {
                // prints exception if there is one
                Debug.WriteLine($"Error in file reading! Error: {ex.Message}");
            }
            finally
            {
                // closes reader if it's not closed already
                if(reader != null) 
                {
                    reader.Close();
                }
            }
        }

        // "num" is the number in the file read
        Rectangle DetermineSprite(int num) {
            int[,] neighbors = new int[3, 3];

            for(int y = 0; y < 3; y++) {
                for(int x = 0; x < 3; x++) {
                    if(y == 1 && x == 1) {
                        neighbors[x, y] = 0;
                    } else {
                        neighbors[x, y] = 1;
                    }
                }
            }

            // n[0,0]   n[1,0]  n[2,0]
            // n[0,1]   n[1,1]  n[2,1]
            // n[0,2]   n[1,2]  n[2,2

            int coordX = 1;
            int coordY = 1;

            // if 0, return a floor
            if(num == 0) {
                coordX = 1;
                coordY = 1;
            } else {
                coordX = 0;
                coordY = 0;
            }

            return new Rectangle(coordX * 16, coordY * 16, 16, 16);
        }
    }
}
