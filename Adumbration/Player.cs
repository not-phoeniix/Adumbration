using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Adumbration
{
    /// <summary>
    /// Representation of a player within Adumbration
    /// </summary>
    public class Player : GameObject
    {
        // Fields
        private bool hasDash;
        private int speed;

        // Properties
        /// <summary>
        /// Get property for whether the player has a dash or not
        /// </summary>
        public bool HasDash
        {
            get { return hasDash; }
        }

        // Constructor
        /// <summary>
        /// Player takes completely from Parent class
        /// for the constructor
        /// </summary>
        public Player()
            : base()
        {
            hasDash = false;
        }

        // Methods

        /// <summary>
        /// Updates the player.
        /// </summary>
        /// <param name="gameTime">State of the game's time.</param>
        public override void Update(GameTime gameTime)
        {
            // Player movement
            KeyboardState currentKbState = Keyboard.GetState();

            if (currentKbState.IsKeyDown(Keys.W))
            {

            }
        }

        /// <summary>
        /// Draws the player.
        /// </summary>
        /// <param name="gameTime">State of the game's time.</param>
        public override void Draw(GameTime gameTime)
        {
            
        }

        /// <summary>
        /// Checks for player collision with any GameObject.
        /// </summary>
        /// <param name="obj">Reference to any GameObject</param>
        /// <returns>True if player is colliding with a GameObject, otherwise false.</returns>
        public override bool isColliding(GameObject obj)
        {
            if (obj.RecPosition.Intersects(this.RecPosition))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
