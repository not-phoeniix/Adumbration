using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Penumbra;
using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using SharpDX.MediaFoundation;

namespace Adumbration
{
    /// <summary>
    /// Class representation of a level, has drawing and 
    /// updating methods and loads the layout from a file.
    /// </summary>
    public class Level
    {
        private SpriteBatch _spriteBatch;

        // Fields
        private char[,] levelLayout;            // copy of level text file, just int's
        private GameObject[,] objectArray;      // full array of GameObject's
        private Texture2D wallSpritesheet;
        private Hull[,] wallHulls;              // for shadow casting
        private List<LightBeam> allBeams;       // all beams in the level currently

        // Mirror Testing
        private Mirror mirror;

        /// <summary>
        /// Creates a new level object, initializing and loading from a file
        /// </summary>
        /// <param name="wallSpritesheet">Texture2D wall spritesheet</param>
        /// <param name="dataFilePath">File path of layout data file (Already in LevelData folder, only file name needed)</param>
        public Level(Texture2D wallSpritesheet, string dataFilePath)
        {
            this.wallSpritesheet = wallSpritesheet;

            // loads and creates level from file path
            levelLayout = LoadLayoutFromFile("../../../Source/LevelData/" + dataFilePath);
            objectArray = LoadObjectsFromLayout(levelLayout);
            wallHulls = LoadHulls(objectArray);

            allBeams = new List<LightBeam>();
        }

        public void LoadMirrorTexture(Texture2D texture, Texture2D wallTexture)
        {
            mirror = new Mirror(texture, wallTexture,
                new Rectangle(0, 0, 12, 12),
                new Rectangle(16 * 8, 125, 12, 12),
                MirrorType.Backward);
        }

        /// <summary>
        /// Tile List associated with the level, 
        /// collection array of all GameObjects
        /// </summary>
        public GameObject[,] TileList
        {
            get { return objectArray; }
            set { objectArray = value; }
        }

        public Hull[,] WallHulls
        {
            get { return wallHulls; }
            set { wallHulls = value; }
        }

        internal List<LightBeam> Beams
        {
            get { return allBeams; }
            set { allBeams = value; }
        }

        /// <summary>
        /// Updates the level's state of the game.
        /// </summary>
        /// <param name="gameTime">State of the game's time.</param>
        public void Update(GameTime gameTime)
        {
            allBeams.Clear();
            
            // updates all GameObjects each frame
            foreach(GameObject obj in objectArray)
            {
                if(obj is LightEmitter emitter)
                {
                    allBeams.Add(emitter.Beam);
                    emitter.Update(gameTime);
                }
                
                if(obj is Mirror mirror)
                {
                    allBeams.Add(mirror?.Beam);
                }

                if (obj is LightReceptor receptor)
                {
                    //for all beams inside the allbeams class,
                    //it will check if the receptor is colliding with it
                    //then it will do a specific action
                    for (int i = 0; i < allBeams.Count; i++)
                    {
                        if (receptor.IsColliding(allBeams[i]))
                        {
                            receptor.Update(gameTime);
                            System.Diagnostics.Debug.WriteLine("IT WORKS");
                        }
                    }
                }
            }

            mirror.Update(gameTime, this);
            if(mirror.Beam != null)
            {
                allBeams.Add(mirror.Beam);
            }
        }

        /// <summary>
        /// Draws entire level.
        /// </summary>
        public void Draw(SpriteBatch sb)
        {
            // this loop draws all objects in the tileList array
            for (int y = 0; y < objectArray.GetLength(1); y++)
            {
                for (int x = 0; x < objectArray.GetLength(0); x++)
                {
                    // draws object
                    objectArray[x, y].Draw(sb);
                }
            }

            // draws all light beams after tile drawing
            foreach (LightBeam beam in allBeams)
            {
                beam.Draw(sb);
            }

            mirror.Draw(sb);
        }

        #region LevelLoading

        // These methods only run once upon level loading/object
        //   creation, they DO NOT occur every frame

        /// <summary>
        /// Loads a level from a file and returns an associated array. 
        /// The first line of the file should be "levelWidth,LevelHeight", 
        /// and all the other lines should be the chars associated with the tiles.
        /// 0 is wall, _ is floor, more chars will be added later.
        /// </summary>
        /// <param name="filename">String of file name</param>
        /// <returns>2D char array of level</returns>
        public char[,] LoadLayoutFromFile(string filename)
        {
            char[,] returnLayout = new char[1, 1];

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
                while ((lineString = reader.ReadLine()) != null)
                {
                    // splits the line at beginning of read
                    string[] splitString = lineString.Split(",");

                    // sets array size & initializes ==========================
                    if (lineNum == 1)
                    {
                        levelWidth = int.Parse(splitString[0]);
                        levelHeight = int.Parse(splitString[1]);

                        // initializes the list with given file values
                        returnLayout = new char[levelWidth, levelHeight];
                    }

                    // loading level & filling array ==========================
                    else
                    {
                        // fills row in array with the split string
                        for (int i = 0; i < splitString.Length; i++)
                        {
                            // removes space and parses to char
                            char trimmedChar = char.Parse(splitString[i].Trim());

                            // adds to layout
                            returnLayout[i, lineNum - 2] = trimmedChar;
                        }
                    }

                    // increments lineNum after each loop
                    lineNum++;
                }
            }
            catch (Exception ex)
            {
                // prints exception if there is one
                Debug.WriteLine($"Error in file reading! Error: {ex.Message}");
            }
            finally
            {
                // closes reader if it's not closed already
                if (reader != null)
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
        public GameObject[,] LoadObjectsFromLayout(char[,] layout)
        {
            int levelWidth = layout.GetLength(0);
            int levelHeight = layout.GetLength(1);

            GameObject[,] returnArray = new GameObject[levelWidth, levelHeight];

            // iterates through array and adds objects
            for (int y = 0; y < levelHeight; y++)
            {
                for (int x = 0; x < levelWidth; x++)
                {
                    // determines what source rect to add to list depending on neighboring tiles
                    Rectangle sourceRect = DetermineSourceRect(
                        layout[x, y],   // number of object in file
                        x,              // tile position X
                        y,              // tile position Y
                        wallSpritesheet,    // Texture2D spritesheet
                        16,             // sprite width
                        16);            // sprite height

                    int sideSizeX = sourceRect.Width;
                    int sideSizeY = sourceRect.Height;

                    // full pos on screen
                    Rectangle positionRect = new Rectangle(
                        x * sideSizeX,
                        y * sideSizeY,
                        sideSizeX,
                        sideSizeY);

                    // ******************************
                    // ******** TILE LOADING ********
                    // ******************************
                    switch(layout[x, y])
                    {
                        // FLOOR
                        case '_':
                            returnArray[x, y] = new Floor(wallSpritesheet, sourceRect, positionRect);
                            break;

                        // EMITTER
                        case 'E':
                            returnArray[x, y] = new LightEmitter(
                                wallSpritesheet, 
                                new Rectangle(16 * 3, 16 * 3, 16, 16),
                                positionRect,
                                Direction.Down,
                                this);
                            break;

                        //RECEPTOR(for now, this one is for if it's pointed up)
                        case 'R':
                            returnArray[x, y] = new LightReceptor(
                                wallSpritesheet,
                                new Rectangle(16 * 8, 16 * 3, 16, 16),
                                new Rectangle(positionRect.X, positionRect.Y + 1, 16, 16),      //sets up the rectangle a bit lower
                                new Rectangle(positionRect.X, positionRect.Y - 1, 16, 16));     //sets the beam hitbox a bit higher
                            break;

                        // WALL
                        default:
                            returnArray[x, y] = new Wall(wallSpritesheet, sourceRect, positionRect);
                            break;
                    }
                }
            }

            return returnArray;
        }

        /// <summary>
        /// Loads and returns all hulls depending on bounds of each object in array
        /// </summary>
        /// <param name="levelObjects">2D array of all level objects</param>
        /// <returns>2D array of hulls</returns>
        public Hull[,] LoadHulls(GameObject[,] levelObjects)
        {
            int levelWidth = levelObjects.GetLength(0);
            int levelHeight = levelObjects.GetLength(1);

            Hull[,] returnArray = new Hull[levelWidth, levelHeight];

            for(int y = 0; y < levelHeight; y++)
            {
                for(int x = 0; x < levelWidth; x++)
                {
                    if(levelObjects[x, y] is Wall)
                    {
                        returnArray[x, y] = ((Wall)levelObjects[x, y]).Hull;
                    }
                }
            }

            return returnArray;
        }

        // returns a source rectangle in wall
        //   spritesheet based on surrounding tiles
        //
        // "num" is the number in the file read
        // "pos" is position of current tile to check
        private Rectangle DetermineSourceRect(
            char tileValue,
            int tilePosX,
            int tilePosY,
            Texture2D spritesheet,
            int spriteWidth,
            int spriteHeight)
        {
            // pixel dimensions divided by sizes of sprites,
            //   represents the number of sprites width and height wise
            int spritesheetWidth = spritesheet.Bounds.Width / spriteWidth;
            int spritesheetHeight = spritesheet.Bounds.Height / spriteHeight;

            // coordinates in spritesheet to multiply at end
            //   of method from where to draw the sprite.
            //
            // by default it is the completely empty black sprite
            Vector2 returnRectCoord = new Vector2(4, 1);

            // if floor char, use the floor coords
            if (tileValue == '_')
            {
                returnRectCoord.X = 1;
                returnRectCoord.Y = 1;
            }
            // else, run thru conditionals to check for correct bounds
            else
            {
                // 2D array same size as number of sprites in spritsheet,
                //   holds which sprite should be drawn at end of method
                bool[,] tileIsTrue = new bool[spritesheetWidth, spritesheetHeight];

                #region RegularWalls

                for (int y = 0; y < 3; y++)
                {
                    for (int x = 0; x < 3; x++)
                    {
                        // current positions of sub-array tile in big layout array
                        int arrayX = tilePosX - 1 + x;
                        int arrayY = tilePosY - 1 + y;

                        // calculates coordinates opposite of relative location of floor
                        int oppX = x - 2 * (x - 1);
                        int oppY = y - 2 * (y - 1);

                        // whether the current cell is in bounds of array
                        bool inBounds = arrayX >= 0 &&
                                        arrayX < levelLayout.GetLength(0) &&
                                        arrayY >= 0 &&
                                        arrayY < levelLayout.GetLength(1);

                        // only checks if it's in the bounds
                        if (inBounds)
                        {
                            // true if iterated coordinate is a floor ('_')
                            if (levelLayout[arrayX, arrayY] == '_')
                            {
                                // if a floor is detected NOT DIAGONALLY,
                                //   set all the diagonal values to false
                                if (x == 1 || y == 1)
                                {
                                    // double iteration loop for checking the array
                                    for (int i = 0; i < spritesheetHeight; i++)
                                    {
                                        for (int j = 0; j < spritesheetWidth; j++)
                                        {
                                            if (i != 1 && j != 1 && j != 4)
                                            {
                                                tileIsTrue[j, i] = false;
                                            }
                                        }
                                    }
                                }

                                tileIsTrue[oppX, oppY] = true;
                            }
                        }
                    }
                }

                #endregion

                #region InverseCorners

                // if bottom and right are true, clear and set to inverted top left
                if (tileIsTrue[1, 2] && tileIsTrue[2, 1])
                {
                    tileIsTrue = new bool[spritesheetWidth, spritesheetHeight];

                    tileIsTrue[3, 0] = true;
                }

                // if bottom and left are true, clear and set to inverted top right
                if (tileIsTrue[1, 2] && tileIsTrue[0, 1])
                {
                    tileIsTrue = new bool[spritesheetWidth, spritesheetHeight];

                    tileIsTrue[5, 0] = true;
                }

                // if top and right are true, clear and set to inverted bottom left
                if (tileIsTrue[1, 0] && tileIsTrue[2, 1])
                {
                    tileIsTrue = new bool[spritesheetWidth, spritesheetHeight];

                    tileIsTrue[3, 2] = true;
                }

                // if top and left are true, clear and set to inverted bottom right
                if (tileIsTrue[1, 0] && tileIsTrue[0, 1])
                {
                    tileIsTrue = new bool[spritesheetWidth, spritesheetHeight];

                    tileIsTrue[5, 2] = true;
                }

                #endregion

                // iteration thru bool array,
                //   SETS FINAL RETURN COORD VALUES
                for (int y = 0; y < spritesheetHeight; y++)
                {
                    for (int x = 0; x < spritesheetWidth; x++)
                    {
                        // only sets coordinates if true
                        if (tileIsTrue[x, y] == true)
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
