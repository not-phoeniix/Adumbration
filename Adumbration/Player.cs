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
    internal class Player : GameObject
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
        public override void Update()
        {
            // Player movement
            KeyboardState currentKbState = Keyboard.GetState();

            if (currentKbState.IsKeyDown(Keys.W))
            {

            }
        }


    }
}
