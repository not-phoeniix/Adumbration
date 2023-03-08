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
        private int[,] levelLayout;         // copy of level text file, just int's
        private GameObject[,] tileList;     // full array of GameObject's
        private Texture2D spritesheet;
        private int levelScale;

        // Constructor
        public Level(Texture2D spritesheet, int levelScale, string dataFilePath)
        {
            this.spritesheet = spritesheet;
            this.levelScale = levelScale;

            // loads and creates level from file path
            levelLayout = LoadLayoutFromFile(dataFilePath);
            tileList = LoadObjectsFromLayout(levelLayout);
        }

        // Properties
        public GameObject[,] TileList
        {
            get { return tileList; }
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

        #region LevelLoading

        /// <summary>
        /// Loads a level from a file and returns an associated array. 
        /// The first line of the file should be "levelWidth,LevelHeight", 
        /// and all the other lines should be the numbers associated with the tiles.
        /// </summary>
        /// <param name="filename">String of file name</param>
        /// <returns>2D integer array of level</returns>
        private int[,] LoadLayoutFromFile(string filename)
        {
            int[,] returnLayout = new int[1, 1];

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

                        // initializes the list with given file values
                        returnLayout = new int[levelWidth, levelHeight];
                    }

                    // loading level & filling array ==========================
                    else
                    {
                        // fills row in array with the split string
                        for(int i = 0; i < splitString.Length; i++) 
                        {
                            returnLayout[i, lineNum - 2] = int.Parse(splitString[i]);
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

            // return the layout at the end of EVERYTHING
            return returnLayout;
        }

        /// <summary>
        /// Loads and sets up the internal GameObject array 
        /// with various objects like walls/floors.
        /// </summary>
        /// <param name="layout"></param>
        private GameObject[,] LoadObjectsFromLayout(int[,] layout) {
            int levelWidth = layout.GetLength(0);
            int levelHeight = layout.GetLength(1);

            GameObject[,] returnArray = new GameObject[levelWidth, levelHeight];

            for(int y = 0; y < levelHeight; y++) {
                for(int x = 0; x < levelWidth; x++) {

                    Rectangle sourceRect = DetermineSprite(layout[x, y], x, y);

                    int sideSize = sourceRect.Width * levelScale;

                    // full pos on screen
                    Rectangle positionRect = new Rectangle(
                        x * sideSize,
                        y * sideSize,
                        sideSize,
                        sideSize);

                    // detects if rect is the coordinates of the floor sprite
                    Rectangle floorSourceRect = new Rectangle(16, 16, 16, 16);

                    // fills array with respective objects
                    if(sourceRect == floorSourceRect) {
                        returnArray[x, y] = new Floor(spritesheet, sourceRect, positionRect);
                    } else {
                        returnArray[x, y] = new Wall(spritesheet, sourceRect, positionRect);
                    }
                }
            }

            return returnArray;
        }

        #endregion

        // returns a source rectangle in wall
        //   spritesheet based on surrounding tiles
        //
        // "num" is the number in the file read
        // "pos" is position of current tile to check
        Rectangle DetermineSprite(int num, int tilePosX, int tilePosY) {
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

            // 1 1 1
            // 1 0 1
            // 1 1 1

            // use levelLayout[]

            Vector2 coord = new Vector2(0, 0);

            // if 0, return a floor
            if(num == 0) {
                coord.X = 1;
                coord.Y = 1;
            } else {
                // TODO: conditionals to check surrounding tiles in
                //   levelLayout[] and set the correct coord numbers

                coord.X = 0;
                coord.Y = 0;
            }

            // returns calculated source rect
            return new Rectangle(
                (int)coord.X * 16, 
                (int)coord.Y * 16,
                16, 
                16);
        }
    }
}
