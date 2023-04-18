using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Penumbra;
using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;

namespace Adumbration
{
    /// <summary>
    /// Class representation of a level, has drawing and 
    /// updating methods and loads the layout from a file.
    /// </summary>
    public class Level
    {
        // level data
        private string currentLevel;
        private string[,] levelLayout;            // copy of level text file, just char's
        private GameObject[,] objectArray;      // full array of GameObject's
        private Hull[,] wallHulls;              // for shadow casting
        
        // misc
        private PenumbraComponent penumbra;
        private Vector2 spawnPoint;

        // texture stuff
        private Dictionary<string, Texture2D> textureDict;
        private Texture2D wallTexture;

        // level objects
        private List<LightBeam> allBeams;
        private List<Mirror> allMirrors;
        private KeyObject levelKey;

        // signal stuff
        private Dictionary<int, List<GameObject>> receiversDict;

        // Multiple beam testing
        private LightBeam testBeam;

        /// <summary>
        /// Creates a new level object, initializing and loading from a file
        /// </summary>
        /// <param name="wallSpritesheet">Texture2D wall spritesheet</param>
        /// <param name="dataFilePath">File path of layout data file (Already in LevelData folder, only file name needed)</param>
        public Level(Dictionary<string, Texture2D> textureDict, string dataFilePath, PenumbraComponent penumbra, Player player)
        {
            // setting fields
            this.textureDict = textureDict;
            this.penumbra = penumbra;
            this.wallTexture = textureDict["walls"];
            this.spawnPoint = new Vector2(0, 0);
            this.allBeams = new List<LightBeam>();
            this.allMirrors = new List<Mirror>();

            // dictionary inits
            receiversDict = new Dictionary<int, List<GameObject>>();

            // load whole level
            SetupLevel(dataFilePath, player);
        }

        #region Properties

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
        }

        internal List<Mirror> Mirrors
        {
            get { return allMirrors; }
        }

        internal KeyObject KeyObject
        {
            get { return levelKey; }
        }

        #endregion

        /// <summary>
        /// Loads a level from current file path
        /// </summary>
        /// <param name="dataFileName">Name of text file (including extension)</param>
        public void SetupLevel(string dataFileName, Player player) {
            currentLevel = "../../../Source/LevelData/" + dataFileName;

            // clears previous lists and objects
            allBeams.Clear();
            allMirrors.Clear();
            levelKey = null;
            receiversDict.Clear();
            penumbra.Lights.Clear();

            // loads and creates level from file path
            levelLayout = LoadLayoutFromFile(currentLevel);
            objectArray = LoadObjectsFromLayout(levelLayout);
            wallHulls = LoadHulls(objectArray);

            // clears and sets up hulls for level collision
            SetPenumbraHulls(penumbra);

            // teleports player to spawn point
            player.X = (int)spawnPoint.X;
            player.Y = (int)spawnPoint.Y;
        }

        /// <summary>
        /// Clears and reloads all hulls in level
        /// </summary>
        /// <param name="penumbra">Game1's PenumbraComponent</param>
        private void SetPenumbraHulls(PenumbraComponent penumbra) {
            penumbra.Hulls.Clear();

            // fills hulls
            for(int y = 0; y < wallHulls.GetLength(1); y++) {
                for(int x = 0; x < wallHulls.GetLength(0); x++) {
                    if(wallHulls[x, y] != null) {
                        penumbra.Hulls.Add(wallHulls[x, y]);
                    }
                }
            }
        }

        /// <summary>
        /// Updates the level's state of the game.
        /// </summary>
        /// <param name="gameTime">State of the game's time.</param>
        public void Update(GameTime gameTime, GameObject player)
        {
            // updates all GameObjects each frame
            foreach(GameObject obj in objectArray)
            {
                // updating all emitters
                if(obj is LightEmitter emitter)
                {
                    emitter.Update(gameTime);
                    
                    // adds main beam if not null
                    if(emitter.Beam != null)
                    {
                        if(!allBeams.Contains(emitter.Beam))
                        {
                            allBeams.Add(emitter.Beam);
                        }

                        // adds the beam's reflection as well if not null
                        if(emitter.Beam.ReflectedBeam != null && 
                            !allBeams.Contains(emitter.Beam.ReflectedBeam))
                        {
                            allBeams.Add(emitter.Beam.ReflectedBeam);
                        }
                    }
                }

                if(obj is LightReceptor receptor)
                {
                    receptor.Update(allBeams);

                    // signal checking, only works if signal is 1 or higher
                    int objSignal = receptor.SignalNum;
                    if(objSignal > 0 && receiversDict.ContainsKey(objSignal))
                    {
                        foreach(GameObject rec in receiversDict[objSignal])
                        {
                            if(rec is LightEmitter e)
                            {
                                e.Enabled = receptor.IsActivated;
                            }
                        }
                    }
                }
            }

            // updating all mirrors
            foreach(Mirror mirror in allMirrors)
            {
                mirror.Update(gameTime);
            }

            // updating all beams, they are called outside emitter objects to decouple
            foreach(LightBeam beam in allBeams)
            {
                beam.Update(gameTime);
            }

            for(int i = 0; i < allBeams.Count; i++)
            {
                allBeams[i].Update(gameTime);
            }

            // updating level key
            levelKey?.Update(gameTime);
        }

        /// <summary>
        /// Draws entire level.
        /// </summary>
        public void Draw(SpriteBatch sb)
        {
            // this loop draws all objects in the tileList array
            for(int y = 0; y < objectArray.GetLength(1); y++)
            {
                for(int x = 0; x < objectArray.GetLength(0); x++)
                {
                    // draws object
                    objectArray[x, y].Draw(sb);
                }
            }

            // draws level key
            levelKey?.Draw(sb);

            // draws all light beams after tile drawing
            foreach(LightBeam beam in allBeams)
            {
                beam.Draw(sb);
            }

            // draws all mirrors
            foreach(Mirror mirror in allMirrors)
            {
                mirror.Draw(sb);
            }
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
        public string[,] LoadLayoutFromFile(string filename)
        {
            string[,] returnLayout = new string[1, 1];

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
                        returnLayout = new string[levelWidth, levelHeight];
                    }

                    // loading level & filling array ==========================
                    else
                    {
                        // fills row in array with the split string
                        for(int i = 0; i < splitString.Length; i++)
                        {
                            // removes space and parses to char
                            string trimmedString = splitString[i].Trim();

                            // adds to layout
                            returnLayout[i, lineNum - 2] = trimmedString;
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
        public GameObject[,] LoadObjectsFromLayout(string[,] layout)
        {
            int levelWidth = layout.GetLength(0);
            int levelHeight = layout.GetLength(1);

            GameObject[,] returnArray = new GameObject[levelWidth, levelHeight];

            // iterates through array and adds objects
            for(int y = 0; y < levelHeight; y++)
            {
                for(int x = 0; x < levelWidth; x++)
                {
                    int sideSizeX = 16;
                    int sideSizeY = 16;

                    // determines what source rect to add to list depending on neighboring tiles
                    Rectangle sourceRect = DetermineSourceRect(
                        layout[x, y][0],    // char value of object in layout
                        x,                  // tile position X
                        y,                  // tile position Y
                        wallTexture,        // Texture2D spritesheet
                        sideSizeX,          // sprite width
                        sideSizeY);         // sprite height

                    // full pos on screen
                    Rectangle positionRect = new Rectangle(
                        x * sideSizeX,
                        y * sideSizeY,
                        sideSizeX,
                        sideSizeY);

                    #region // Direction logic

                    // tracks if there is a floor to the left/right/up/down of current coords in layout:

                    bool floorAbove = false;
                    bool floorBelow = false;
                    bool floorLeft = false;
                    bool floorRight = false;

                    if(y > 0 && layout[x, y - 1][0] == '_') { floorAbove = true; }
                    if(y < levelHeight - 1 && layout[x, y + 1][0] == '_') { floorBelow = true; }
                    if(x > 0 && layout[x - 1, y][0] == '_') { floorLeft = true; }
                    if(x < levelWidth - 1 && layout[x + 1, y][0] == '_') { floorRight = true; }

                    Direction dir = Direction.Down;

                    // sets beam direction based on neighboring floors
                    if(floorAbove)
                    {
                        dir = Direction.Up;
                    }
                    else if(floorBelow)
                    {
                        dir = Direction.Down;
                    }
                    else if(floorLeft)
                    {
                        dir = Direction.Left;
                    }
                    else if(floorRight)
                    {
                        dir = Direction.Right;
                    }

                    #endregion

                    int signal = -1;

                    // ******************************
                    // ******** TILE LOADING ********
                    // ******************************
                    switch(layout[x, y][0])
                    {
                        // FORWARD MIRROR
                        case '/':
                            returnArray[x, y] = new Floor(wallTexture, sourceRect, positionRect);
                            allMirrors.Add(new Mirror(
                                textureDict["mirror"],
                                new Rectangle(
                                    positionRect.X + 2,
                                    positionRect.Y + 2,
                                    textureDict["mirror"].Width, 
                                    textureDict["mirror"].Height),
                                MirrorType.Forward));
                            break;

                        // BACKWARD MIRROR
                        case '\\':
                            returnArray[x, y] = new Floor(wallTexture, sourceRect, positionRect);
                            allMirrors.Add(new Mirror(
                                textureDict["mirror"],
                                new Rectangle(
                                    positionRect.X + 2,
                                    positionRect.Y + 2,
                                    textureDict["mirror"].Width,
                                    textureDict["mirror"].Height),
                                MirrorType.Backward));
                            break;

                        // KEY
                        case 'K':
                            returnArray[x, y] = new Floor(wallTexture, sourceRect, positionRect);

                            levelKey = new KeyObject(
                                textureDict["key"],
                                new Rectangle(0, 0, 12, 12),
                                new Rectangle(positionRect.X + 3, positionRect.Y + 3, 10, 10));
                            break;

                        // SPAWN POINT
                        case 'S':
                            returnArray[x, y] = new Floor(wallTexture, sourceRect, positionRect);
                            spawnPoint = new Vector2(positionRect.X, positionRect.Y);
                            break;

                        // FLOOR
                        case '_':
                            returnArray[x, y] = new Floor(wallTexture, sourceRect, positionRect);
                            break;

                        // EMITTER
                        case 'e':
                        case 'E':
                            if(layout[x, y].Length > 1)
                            {
                                signal = int.Parse(layout[x, y][1].ToString());
                            }
                            else
                            {
                                throw new Exception($"ERROR: Emitter at layout[{x}, {y}] does not contain a " +
                                    "signal associated! (\"E#\", where # is an int 0-9)");
                            }

                            // uppercase E means enabled emitter, lowercase means disabled
                            bool beamEnabled = layout[x, y][0] == 'E' ? true : false;

                            returnArray[x, y] = new LightEmitter(
                                textureDict,
                                positionRect,
                                dir,
                                beamEnabled,           // enabled or not at start
                                signal);

                            // creates a new list if it doesn't exist yet
                            if(!receiversDict.ContainsKey(signal))
                            {
                                receiversDict[signal] = new List<GameObject>();
                            }

                            // adds this object to the receiver list
                            receiversDict[signal].Add(returnArray[x, y]);

                            break;

                        // RECEPTOR
                        case 'R':
                            if(layout[x, y].Length > 1)
                            {
                                signal = int.Parse(layout[x, y][1].ToString());
                            }
                            else
                            {
                                throw new Exception($"ERROR: Receptor at layout[{x}, {y}] does " +
                                    "not contain a signal associated! (\"E#\", where # is an int 0-9)");
                            }

                            returnArray[x, y] = new LightReceptor(
                                wallTexture,
                                new Rectangle(positionRect.X, positionRect.Y, 16, 16),
                                dir,
                                signal);

                            break;

                        // WALL
                        case '0':
                            returnArray[x, y] = new Wall(wallTexture, sourceRect, positionRect);
                            break;
                        
                        // DEFAULT PRINT ERROR MESSAGE
                        default:
                            Debug.WriteLine($"ERROR: Character [{layout[x, y]}] not recognized in the layout!");
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

        /// <summary>
        /// Determines the source rectangle of a wall according to surrounding tiles.
        /// Automatically finds whether to use a corner/edge depending on surrounding floors
        /// </summary>
        /// <param name="tileValue">Current tile being checked</param>
        /// <param name="tilePosX">X position of current tile in array</param>
        /// <param name="tilePosY">X position of current tile in array</param>
        /// <param name="spritesheet">Full spritesheet</param>
        /// <param name="spriteWidth">Sprite's width</param>
        /// <param name="spriteHeight">Sprite's height</param>
        /// <returns>Calculated source rectangle</returns>
        private Rectangle DetermineSourceRect(
            char tileValue,
            int tilePosX,
            int tilePosY,
            Texture2D spritesheet,
            int spriteWidth,
            int spriteHeight)
        {
            // coords guide:
            //  3,0     4,0     5,0
            //  3,1     4,1     5,1
            //  3,2     4,2     5,2

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
            if( tileValue == '_' || 
                tileValue == 'S' ||
                tileValue == 'K' ||
                tileValue == '/' ||
                tileValue == '\\')
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

                for(int y = 0; y < 3; y++)
                {
                    for(int x = 0; x < 3; x++)
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
                        if(inBounds)
                        {
                            bool correctChar =
                                levelLayout[arrayX, arrayY][0] == '_' ||
                                levelLayout[arrayX, arrayY][0] == 'S' ||
                                levelLayout[arrayX, arrayY][0] == 'K';

                            // true if iterated coordinate is a floor ('_')
                            if(correctChar)
                            {
                                // if a floor is detected NOT DIAGONALLY,
                                //   set all the diagonal values to false
                                if(x == 1 || y == 1)
                                {
                                    // double iteration loop for checking the array
                                    for(int i = 0; i < spritesheetHeight; i++)
                                    {
                                        for(int j = 0; j < spritesheetWidth; j++)
                                        {
                                            if(i != 1 && j != 1 && j != 4)
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
                if(tileIsTrue[1, 2] && tileIsTrue[2, 1])
                {
                    tileIsTrue = new bool[spritesheetWidth, spritesheetHeight];

                    tileIsTrue[3, 0] = true;
                }

                // if bottom and left are true, clear and set to inverted top right
                if(tileIsTrue[1, 2] && tileIsTrue[0, 1])
                {
                    tileIsTrue = new bool[spritesheetWidth, spritesheetHeight];

                    tileIsTrue[5, 0] = true;
                }

                // if top and right are true, clear and set to inverted bottom left
                if(tileIsTrue[1, 0] && tileIsTrue[2, 1])
                {
                    tileIsTrue = new bool[spritesheetWidth, spritesheetHeight];

                    tileIsTrue[3, 2] = true;
                }

                // if top and left are true, clear and set to inverted bottom right
                if(tileIsTrue[1, 0] && tileIsTrue[0, 1])
                {
                    tileIsTrue = new bool[spritesheetWidth, spritesheetHeight];

                    tileIsTrue[5, 2] = true;
                }

                #endregion

                // iteration thru bool array,
                //   SETS FINAL RETURN COORD VALUES
                for(int y = 0; y < spritesheetHeight; y++)
                {
                    for(int x = 0; x < spritesheetWidth; x++)
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
