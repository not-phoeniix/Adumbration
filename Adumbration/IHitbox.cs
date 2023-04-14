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

        // Method

        /// <summary>
        /// Checks whether or not the class's hitbox is colliding with a GameObject.
        /// </summary>
        /// <param name="gameObj">Reference to an object.</param>
        /// <returns>True if the collision occurs, otherwise false.</returns>
        public bool IsColliding(GameObject gameObj);
    }
}
