using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adumbration
{
    /// <summary>
    /// Alexander Gough & Nikki Murello
    /// Purpose
    /// </summary>
    public class Level
    {
        // Fields
        private GameObject[,] tileList;

        // Methods

        /// <summary>
        /// Abstract ResetLevel method to be reset a level.
        /// The contents are in the child classes.
        /// </summary>
        private void ResetLevel()
        {

        }

        /// <summary>
        /// Abstract Update method to update the state of the game.
        /// The contents are in the child classes.
        /// </summary>
        /// <param name="gameTime">State of the game's time.</param>
        private void Update(GameTime gameTime)
        {

        }

        /// <summary>
        /// Abstract Draw method to draw object(s).
        /// The contents are in the child classes.
        /// </summary>
        /// <param name="gameTime">State of the game's time.</param>
        private void Draw(GameTime gameTime)
        {

        }
    }
}
