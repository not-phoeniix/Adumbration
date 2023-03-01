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
        public bool HasDash
        {
            get { return hasDash; }
        }

        // Constructor
        public Player()
            : base()
        {

        }

        // Methods
        public void override Update()
        {

        }
    }
}
