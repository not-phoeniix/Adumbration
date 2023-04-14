using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adumbration
{
    /// <summary>
    /// Interface that defines a hitbox property and necessary methods.
    /// </summary>
    public interface IHitbox
    {
        // Property
        public Rectangle Hitbox { get; }

        // Methods
        /// <summary>
        /// Checks whether or not the class's hitbox is colliding with a GameObject.
        /// </summary>
        /// <param name="gameObj">Reference to an object.</param>
        /// <returns>True if the collision occurs, otherwise false.</returns>
        public bool IsColliding(GameObject gameObj);

        /// <summary>
        /// If the class's hitbox interacts with the player
        /// and the 'E' key is pressed, this method runs.
        /// </summary>
        /// <param name="myPlayer">Reference to Game1's player.</param>
        /// <param name="currentState">The current state of the keyboard.</param>
        /// <param name="previousState">The previous state of the keyboard.</param>
        public void Interact(Player myPlayer, KeyboardState currentState, KeyboardState previousState);
    }
}
