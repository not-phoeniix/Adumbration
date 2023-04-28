using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Penumbra;
using System;
using System.Collections.Generic;

namespace Adumbration
{
    /// <summary>
    /// Enumeration that holds all different possible levels
    /// </summary>
    public enum GameLevels
    {
        TestLevel,
        TestLevel2,

        Hub,
        Level1,
        Level2,
        Level3,
        Level4,
        End
    }

    /// <summary>
    /// Singleton LevelManager that can change levels. 
    /// Only one level can be loaded and used at a time.
    /// </summary>
    public sealed class LevelManager
    {
        #region SingletonStuff

        // static private instance of itself
        private static LevelManager instance = null;

        // PRIVATE constructor
        private LevelManager() { }

        /// <summary>
        /// LevelManager instance, singleton which allows 
        /// managing of all levels in the game. READ-ONLY
        /// </summary>
        public static LevelManager Instance
        {
            get
            {
                // if the instance wasn't created yet, create it
                if (instance == null)
                {
                    instance = new LevelManager();
                }

                return instance;
            }
        }

        #endregion

        // level info
        private Level currentLevel;
        private Player player;
        private GameLevels currentLevelEnum;

        /// <summary>
        /// Get-only property for current level
        /// </summary>
        public Level CurrentLevel
        {
            get { return currentLevel; }
        }

        public GameLevels CurrentLevelEnum
        {
            get { return currentLevelEnum; }
        }

        /// <summary>
        /// Initializes LevelManager singleton, must be run first for levels to work properly. LOADS HUB BY DEFAULT
        /// </summary>
        /// <param name="levelSpritesheet"></param>
        /// <param name="startingLevel"></param>
        public void Initialize(
            Dictionary<string, Texture2D> textureDict, 
            Dictionary<string, SoundEffect> soundDict,
            PenumbraComponent penumbra,
            Player player)
        {
            this.player = player;
            currentLevel = new Level(textureDict, soundDict, "Hub.txt", penumbra, player);
        }

        /// <summary>
        /// Loads level according to the GameLevels enum,
        /// which is an enum that holds all possible levels.
        /// </summary>
        /// <param name="level">Level to load</param>
        public void LoadLevel(GameLevels level)
        {
            currentLevelEnum = level;

            string levelDataPath;

            // FSM for what level is being loaded
            // SETS THE PATH
            switch(level)
            {
                case GameLevels.Hub:
                    levelDataPath = "Hub.txt";
                    break;

                case GameLevels.Level1:
                    levelDataPath = "Level_1.txt";
                    break;

                case GameLevels.Level2:
                    levelDataPath = "Level_2.txt";
                    break;

                case GameLevels.Level3:
                    levelDataPath = "Level_3.txt";
                    break;

                case GameLevels.Level4:
                    levelDataPath = "Level_4.txt";
                    break;

                case GameLevels.End:
                    levelDataPath = "End.txt";
                    break;

                // TEST LEVELS:

                case GameLevels.TestLevel:
                    levelDataPath = "BigLevelTest.txt";
                    break;

                case GameLevels.TestLevel2:
                    levelDataPath = "BigLevelTest2.txt";
                    break;

                // throw exception if any other value is inputted
                default:
                    throw new Exception($"Error: level {level} does not exist!");
            }

            currentLevel.SetupLevel(levelDataPath, player);
        }

        /// <summary>
        /// Resets current level
        /// </summary>
        public void ResetLevel() {
            LoadLevel(currentLevelEnum);
        }

        #region GameLoop

        /// <summary>
        /// Updates the logic of currently loaded level
        /// </summary>
        public void Update(GameTime gameTime, Player player)
        {
            currentLevel.Update(gameTime, player);
        }

        /// <summary>
        /// Draws the currently loaded level to the screen
        /// </summary>
        /// <param name="sb"></param>
        public void Draw(SpriteBatch sb)
        {
            currentLevel.Draw(sb);
        }

        #endregion
    }
}