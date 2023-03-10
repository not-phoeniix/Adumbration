using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection.Metadata.Ecma335;

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

        /// <summary>
        /// Tile List associated with the level, 
        /// collection array of all GameObjects
        /// </summary>
        public GameObject[,] TileList
        {
            get { return tileList; }
        }

        #region Methods

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
            // This is Empty right now but should include update methods
            //   for all objects in game, i.e. light beams and mirrors
            //   and buttons and such.
        }

        /// <summary>
        /// Draws entire level.
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

        #region LevelLoading

        /// <summary>
        /// Loads a level from a file and returns an associated array. 
        /// The first line of the file should be "levelWidth,LevelHeight", 
        /// and all the other lines should be the numbers associated with the tiles.
        /// 0 is wall, 1 is floor, more numbers will be added later.
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
        private GameObject[,] LoadObjectsFromLayout(int[,] layout)
        {
            int levelWidth = layout.GetLength(0);
            int levelHeight = layout.GetLength(1);

            GameObject[,] returnArray = new GameObject[levelWidth, levelHeight];

            for(int y = 0; y < levelHeight; y++)
            {
                for(int x = 0; x < levelWidth; x++)
                {

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
                    if(sourceRect == floorSourceRect)
                    {
                        returnArray[x, y] = new Floor(spritesheet, sourceRect, positionRect);
                    }
                    else
                    {
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
        Rectangle DetermineSprite(int num, int tilePosX, int tilePosY)
        {
            // coordinates in spritesheet to multiply at end
            //   of method from where to draw the sprite.
            //
            // by default it is the completely empty black sprite
            Vector2 returnRectCoord = new Vector2(4, 1);

            // if 1, use the floor coords
            if(num == 1)
            {
                returnRectCoord.X = 1;
                returnRectCoord.Y = 1;

            }
            // else, run thru conditionals to check for correct bounds
            else
            {
                // TODO: conditionals to check surrounding tiles in
                //   levelLayout[] and set the correct coord numbers

                #region SurroundingTileChecks

                // boolean checks
                bool floorAbove = false;
                bool floorBelow = false;
                bool floorLeft = false;
                bool floorRight = false;
                int cornerRotNum = 0;

                // checking tiles ABOVE itself
                if(tilePosY > 0)
                {
                    floorAbove = (levelLayout[tilePosX, tilePosY - 1] == 1);

                    //if(levelLayout[tilePosX, tilePosY - 1] == 1)
                    //{
                    //    returnRectCoord.X = 1;
                    //    returnRectCoord.Y = 2;
                    //}
                }

                // checking tiles BELOW itself
                if(tilePosY < levelLayout.GetLength(1) - 1)
                {
                    floorBelow = (levelLayout[tilePosX, tilePosY + 1] == 1);

                    //if(levelLayout[tilePosX, tilePosY - 1] == 1)
                    //{
                    //    returnRectCoord.X = 1;
                    //    returnRectCoord.Y = 2;
                    //}
                }

                // checking tiles to LEFT of itself
                if(tilePosX > 0)
                {
                    floorLeft = (levelLayout[tilePosX - 1, tilePosY] == 1);

                    //if(levelLayout[tilePosX - 1, tilePosY] == 1)
                    //{
                    //    returnRectCoord.X = 2;
                    //    returnRectCoord.Y = 1;
                    //}
                }

                // checking tiles to RIGHT of itself
                if(tilePosX < levelLayout.GetLength(1) - 1)
                {
                    floorRight = (levelLayout[tilePosX + 1, tilePosY] == 1);

                    //if(levelLayout[tilePosX + 1, tilePosY] == 1)
                    //{
                    //    returnRectCoord.X = 0;
                    //    returnRectCoord.Y = 1;
                    //}
                }

                if(!floorAbove && !floorBelow && !floorLeft && !floorRight)
                {
                    cornerRotNum = 0;
                }

                #endregion

                #region RectValueSets

                // 4 cardinal directions setting
                if(floorAbove)
                {
                    returnRectCoord.X = 1;
                    returnRectCoord.Y = 2;
                } 
                else if(floorBelow)
                {
                    returnRectCoord.X = 1;
                    returnRectCoord.Y = 0;
                }
                else if(floorLeft)
                {
                    returnRectCoord.X = 2;
                    returnRectCoord.Y = 1;
                }
                else if(floorRight)
                {
                    returnRectCoord.X = 0;
                    returnRectCoord.Y = 1;
                } 
                // else, is a corner
                else
                {

                }

                #endregion
            }

            // returns calculated source rect
            return new Rectangle(
                (int)returnRectCoord.X * 16,
                (int)returnRectCoord.Y * 16,
                16,
                16);
        }

        #endregion
    }
}
