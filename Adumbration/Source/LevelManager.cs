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
    public sealed class LevelManager
    {
        // Fields
        private static LevelManager instance = null;
        private static Level[] levels = new Level[4];

        // Level test stuff
        private static Texture2D wallSpritesheet;
        private static Level levelTest;

        // Properties

        /// <summary>
        /// Get only for the singleton instance.
        /// If instance is null, instantiate it, then return it.
        /// </summary>
        public static LevelManager GetInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new LevelManager();
                }
                return instance;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static Level[] GetLevels
        {
            get { return levels; }
        }

        // Private Constructor
        private LevelManager()
        {
            levelTest = new Level(wallSpritesheet, levelScale, );
        }

        // Methods

        /// <summary>
        /// Takes the number of levels and converts to a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format(
                "Levels: {0}",
                levels.Length);
        }


        public void Update(GameTime gameTime)
        {

        }


        public void Draw(SpriteBatch sb)
        {
            levelTest.Draw(sb);
        }
    }
}