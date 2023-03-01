using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adumbration
{
    /// <summary>
    /// this is the abstract 
    /// parent class that almost EVERY
    /// object will use 
    /// </summary>
    internal abstract class GameObject
    {
        //all the protected fields that will be used in every child class
        protected Texture2D spriteSheet;
        protected Rectangle sourceRect;
        protected Rectangle recPosition;

        //a default constructor for this class
        //*will change later when the spritesheet is added to make things easier
        public GameObject()
        {}

        //all the abstract methods that will be used in all of the classes
        
        /// <summary>
        /// will be changed to update anything that happens to this SPECIFIC sprite
        /// </summary>
        public abstract void Update();

        /// <summary>
        /// will be changed to draw the specific object
        /// </summary>
        public abstract void Draw();

        /// <summary>
        /// is here to check if a specific object is colliding with another object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public abstract bool isColliding(GameObject obj);


    }
}
