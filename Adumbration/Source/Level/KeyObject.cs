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
    internal class KeyObject : GameObject, IHitbox
    {
        //field
        private Rectangle hitbox;
        private bool pickedUp;
        KeyboardState prevState;
        private bool colliding;
        private int originY;

        //property

        public Rectangle Hitbox
        {
            get { return hitbox; }
        }

        public bool PickedUp
        {
            get { return pickedUp; }
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
            hitbox = new Rectangle(
                position.X,                     //x pos
                position.Y,                     //y pos
                position.Width + 1,             //width size
                position.Height + 1);           //height size

            pickedUp = false;
            colliding = false;
            originY = position.Y;
        }


        /// <summary>
        /// this is an override of the update method
        /// and depending on the things that happens to the key 
        /// it will delete the key from the screen
        /// aka you picked it up
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime, Player player)
        {
            KeyboardState currentState = Keyboard.GetState();

            //checks to see if the user clicked on the key
            if (IsColliding(player))
            {
                //if the key isn't picked up and the user presses e
                //then it will pick up the key
                if (!pickedUp && currentState.IsKeyDown(Keys.E) && prevState.IsKeyUp(Keys.E))
                {
                    positionRect.Width = 0;
                    positionRect.Height = 0;
                    System.Diagnostics.Debug.WriteLine("key taken");
                    pickedUp = true;
                }

                //if the key is picked up it will add to the list once
                if (pickedUp)
                {
                    player.CollectedKeys.Add(true);
                }
            }

            prevState = currentState;


            //in progress

            //makes the key keep bouncing
            //if (positionRect.Y < originY + 1)
            //{
            //    positionRect.Y += 1;
            //}
            //else
            //{
            //    positionRect.Y -= 1;
            //}
        }

        /// <summary>
        /// Checks if a key is colliding with an object.
        /// </summary>
        /// <param name="obj">Reference to a game object.</param>
        /// <returns>True if the collision occurs, otherwise false.</returns>
        public override bool IsColliding(GameObject obj)
        {
            colliding = positionRect.Intersects(obj.Position);
            return colliding;
        }
    }
}
