using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private static int counter = 0;
        private static LevelManager instance = null;

        // Properties

        /// <summary>
        /// Get only for the singleton instance.
        /// </summary>
        public static LevelManager Instance
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

        // Private Constructor
        private LevelManager()
        {
            counter++;
            Console.WriteLine(ToString());
        }

        // Methods

        /// <summary>
        /// Updates the levels
        /// </summary>
        /// <param name="gameTime">State of the game's time.</param>
        public void Update(GameTime gameTime)
        {

        }

        /// <summary>
        /// Draws the levels
        /// </summary>
        /// <param name="gameTime">State of the game's time.</param>
        public void Draw(GameTime gameTime)
        {

        }

        public override string ToString()
        {
            return String.Format(
                "Levels: {0}",
                counter);
        }
    }
}
