﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Adumbration
{
    /// <summary>
    /// Enumeration that holds all different possible levels
    /// </summary>
    public enum GameLevels
    {
        TestLevel,

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
                if(instance == null)
                {
                    instance = new LevelManager();
                }

                return instance;
            }
        }

        #endregion

        // level info
        private Texture2D levelSpritesheet;
        private Level currentLevel;

        /// <summary>
        /// Get-only property for current level
        /// </summary>
        public Level CurrentLevel
        {
            get { return currentLevel; }
        }

        /// <summary>
        /// Initializes LevelManager singleton, must be run first for levels to work properly.
        /// </summary>
        /// <param name="levelSpritesheet"></param>
        /// <param name="startingLevel"></param>
        public void Initialize(Texture2D levelSpritesheet, GameLevels startingLevel)
        {
            this.levelSpritesheet = levelSpritesheet;
            LoadLevel(startingLevel);
        }

        /// <summary>
        /// Loads level according to the GameLevels enum,
        /// which is an enum that holds all possible levels.
        /// </summary>
        /// <param name="level">Level to load</param>
        public void LoadLevel(GameLevels level)
        {
            // FSM for what level is being loaded
            switch(level)
            {
                // test level
                case GameLevels.TestLevel:
                    currentLevel = new Level(
                        levelSpritesheet, 
                        "BigLevelTest.txt");

                    break;

                // throw exception if any other value is inputted
                default:
                    throw new Exception($"Error: level {level} does not exist!");
            }
        }

        #region GameLoop

        /// <summary>
        /// Updates the logic of currently loaded level
        /// </summary>
        public void Update(GameTime gameTime)
        {
            currentLevel.Update(gameTime);
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