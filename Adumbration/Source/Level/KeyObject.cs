﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Adumbration
{
    /// <summary>
    /// this is the key class and it will 
    /// be what the player picks up
    /// so that they can unlock the final door in the end
    /// </summary>
    internal class KeyObject : GameObject, IHitbox
    {
        // Fields
        private Rectangle hitbox;
        private bool pickedUp;
        KeyboardState prevState;
        private bool colliding;
        private SoundEffectInstance pickupSound;

        // Properties
        /// <summary>
        /// Interact Hitbox of Key object
        /// </summary>
        public Rectangle Hitbox
        {
            get { return hitbox; }
        }

        /// <summary>
        /// Whether the Key was picked up or not
        /// </summary>
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
        public KeyObject(Texture2D spriteSheet, SoundEffect sound, Rectangle sourceRect, Rectangle position)
            : base(spriteSheet, sourceRect, position)
        {
            hitbox = new Rectangle(
                position.X,                     //x pos
                position.Y,                     //y pos
                position.Width + 1,             //width size
                position.Height + 1);           //height size

            pickupSound = sound.CreateInstance();
            pickupSound.Volume = 0.8f;

            pickedUp = false;
            colliding = false;
        }


        /// <summary>
        /// this is an override of the update method
        /// and depending on the things that happens to the key 
        /// it will delete the key from the screen
        /// aka you picked it up
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime, Player player, LevelManager manager)
        {
            KeyboardState currentState = Keyboard.GetState();

            //checks to see if the user clicked on the key
            if (IsColliding(player))
            {
                //if the key isn't picked up and the user presses e
                //then it will pick up the key
                if (!pickedUp && currentState.IsKeyDown(Keys.Space) && prevState.IsKeyUp(Keys.Space))
                {
                    positionRect.Width = 0;
                    positionRect.Height = 0;
                    System.Diagnostics.Debug.WriteLine("key taken");
                    pickupSound.Play();
                    pickedUp = true;

                    //if the key is picked up it will add to the list once
                    if (pickedUp && manager.CurrentLevelEnum == GameLevels.Level1)
                    {
                        player.CollectedKeys[0] = true;
                    }

                    if (pickedUp && manager.CurrentLevelEnum == GameLevels.Level2)
                    {
                        player.CollectedKeys[1] = true;
                    }

                    if (pickedUp && manager.CurrentLevelEnum == GameLevels.Level3)
                    {
                        player.CollectedKeys[2] = true;
                    }
                    
                    if (pickedUp && manager.CurrentLevelEnum == GameLevels.Level4)
                    {
                        player.CollectedKeys[3] = true;
                    }
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
