﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using System.IO;

namespace Adumbration
{
    /// <summary>
    /// Class representation of a level, has drawing and 
    /// updating methods and loads the layout from a file.
    /// </summary>
    public class Level
    {
        // Fields
        private int[,] levelLayout;         // copy of level text file, just int's
        private GameObject[,] tileList;     // full array of GameObject's
        private Texture2D spritesheet;
        private int levelScale;

        /// <summary>
        /// Creates a new level object, initializing and loading from a file
        /// </summary>
        /// <param name="spritesheet">Texture2D wall spritesheet</param>
        /// <param name="levelScale">Overall scale of level being drawn</param>
        /// <param name="dataFilePath">File path of layout data file (Already in LevelData folder, only file name needed)</param>
        public Level(Texture2D spritesheet, int levelScale, string dataFilePath)
        {
            this.spritesheet = spritesheet;
            this.levelScale = levelScale;

            // loads and creates level from file path
            levelLayout = LoadLayoutFromFile("../../../Source/LevelData/" + dataFilePath);
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
                    // determines from below method what tile to draw,
                    //   like whether it's an edge or a corner
                    Rectangle sourceRect = DetermineSourceRect(layout[x, y], x, y);

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
        private Rectangle DetermineSourceRect(int num, int tilePosX, int tilePosY)
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
                // size of boolean array
                int sheetX = spritesheet.Bounds.Width / 16;
                int sheetY = spritesheet.Bounds.Height / 16;

                // 2D array same size as spritesheet zoomed out,
                //   only one item is true at a time and that is 
                //   the one being drawn
                bool[,] tileIsTrue = new bool[sheetX, sheetY];

                #region SurroundingTileChecks
                
                // checking tiles ABOVE itself
                if(tilePosY > 0)
                {
                    // TOP EDGE
                    bool isBottomEdge = levelLayout[tilePosX, tilePosY - 1] == 1;

                    // checks for corners
                    if(!isBottomEdge)
                    {
                        // BOTTOM RIGHT CORNER
                        if(tilePosX > 0)
                        {
                            tileIsTrue[2, 2] = levelLayout[tilePosX - 1, tilePosY - 1] == 1;
                        }
                        // BOTTOM LEFT CORNER
                        else if(tilePosX < levelLayout.GetLength(0) - 1)
                        {
                            tileIsTrue[0, 2] = levelLayout[tilePosX + 1, tilePosY - 1] == 1;
                        }
                    }
                    // else, sets it to the edge
                    else
                    {
                        tileIsTrue[1, 2] = isBottomEdge;
                    }
                }

                // checking tiles BELOW itself
                if(tilePosY < levelLayout.GetLength(1) - 1)
                {
                    // TOP EDGE
                    bool isTopEdge = levelLayout[tilePosX, tilePosY + 1] == 1;

                    // checks for corners
                    if(!isTopEdge)
                    {
                        // TOP RIGHT CORNER
                        if(tilePosX > 0)
                        {
                            tileIsTrue[2, 0] = levelLayout[tilePosX - 1, tilePosY + 1] == 1;
                        }
                        // TOP LEFT CORNER
                        else if(tilePosX < levelLayout.GetLength(0) - 1)
                        {
                            tileIsTrue[0, 0] = levelLayout[tilePosX + 1, tilePosY + 1] == 1;
                        }
                    } 
                    // else, sets it to the edge
                    else
                    {
                        tileIsTrue[1, 0] = isTopEdge;
                    }
                }

                // checking tiles to LEFT of itself
                if(tilePosX > 0)
                {
                    // RIGHT EDGE
                    bool isRightEdge = levelLayout[tilePosX - 1, tilePosY] == 1;
                    if(isRightEdge)
                    {
                        tileIsTrue = new bool[sheetX, sheetY];
                        tileIsTrue[2, 1] = true;
                    }
                }

                // checking tiles to RIGHT of itself
                if(tilePosX < levelLayout.GetLength(1) - 1)
                {
                    // LEFT EDGE
                    bool isLeftEdge = levelLayout[tilePosX + 1, tilePosY] == 1;
                    if(isLeftEdge)
                    {
                        tileIsTrue = new bool[sheetX, sheetY];
                        tileIsTrue[0, 1] = true;
                    }
                }

                #endregion

                // iterates through the boolean array, only
                //   setting coordinates for the true value
                for(int y = 0; y < tileIsTrue.GetLength(1); y++)
                {
                    for(int x = 0; x < tileIsTrue.GetLength(0); x++)
                    {
                        // only sets coordinates if true
                        if(tileIsTrue[x, y] == true)
                        {
                            returnRectCoord.X = x;
                            returnRectCoord.Y = y;
                        }
                    }
                }
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
