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
        private string creationTime;
        private LevelManager instance = null;
        private Level[] allLevels;

        // Properties


        public LevelManager Instance
        {
            get { return instance; }
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
    }
}
