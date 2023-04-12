using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adumbration
{
    /// <summary>
    /// this is the key class and it will 
    /// be what the player picks up
    /// so that they can unlock the final door in the end
    /// </summary>
    internal class KeyObject : GameObject
    {
        //field
        private Rectangle hitBox;
        private bool pickedUp;
        KeyboardState prevState;

        //property

        public Rectangle HitBox
        {
            get { return this.hitBox; }
        }

        /// <summary>
        /// a basic constructor that will load everything needed for this class
        /// </summary>
        /// <param name="spriteSheet"> the spritesheet of the things Nikki has drawn </param>
        /// <param name="sourceRect"> the spritesheet's position of the key </param>
        /// <param name="position"> the actual position of the rectangle </param>
        public KeyObject(Texture2D spriteSheet, Rectangle sourceRect, Rectangle position)
            : base(spriteSheet, sourceRect, position)
        {
            hitBox = new Rectangle(
                position.X,                     //x pos
                position.Y,                     //y pos
                position.Width + 1,             //width size
                position.Height + 1);           //height size

            pickedUp = false;
        }


        /// <summary>
        /// this is an override of the update method
        /// and depending on the things that happens to the key 
        /// it will delete the key from the screen
        /// aka you picked it up
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            KeyboardState currentState = Keyboard.GetState();
            if (!pickedUp)
            {

            }

            prevState = currentState;
        }
    }
}
