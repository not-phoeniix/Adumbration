using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Adumbration
{
    /// <summary>
    /// Alexander Gough
    /// Purpose: It is a Singleton that creates the Levels
    /// Restrictions: Since it is a sealed class, no classes inherit from it.
    /// </summary>
    public class LevelManager
    {
        // Fields
        private int instanceCounter = 0;
        private LevelManager instance = null;
        private Level[] levels = new Level[4];

        // Level test stuff
        private Player lvlPlayer;
        private Texture2D wallSpritesheet;
        private Level levelTest;

        // Properties

        /// <summary>
        /// Get only for the singleton instance.
        /// If instance is null, instantiate it, then return it.
        /// </summary>
        public LevelManager GetInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new LevelManager(levelTest);
                }
                return instance;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Level[] GetLevels
        {
            get { return levels; }
        }

        /// <summary>
        /// Private Constructor
        /// </summary>
        public LevelManager(Level level)
        {
            levelTest = level;
        }

        // Methods


        public void Update(GameTime gameTime)
        {
            lvlPlayer.Update(gameTime, levelTest);
        }


        public void Draw(SpriteBatch sb)
        {
            levelTest.Draw(sb);
        }
    }
}