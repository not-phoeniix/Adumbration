using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Adumbration
{
    /// <summary>
    /// This class will be 
    /// the final door that the 
    /// player needs to unlock
    /// </summary>
    internal class FinalDoor : Wall
    {
        // Fields
        private bool isOpen;
        private bool unlocked;
        private Rectangle unlockHitbox;
        private Rectangle hitbox;
        private KeyboardState previousState;
        private Dictionary<string, Texture2D> textureDict;

        public FinalDoor(Texture2D spriteSheet, Rectangle sourceRect, Rectangle position)
             : base(spriteSheet, sourceRect, position)
        {
            // Create door hitboxes
            unlockHitbox = new Rectangle(
                position.X - position.Width / 2,
                position.Y - position.Height / 2,
                position.Width * 2,
                position.Height * 2);

            hitbox = new Rectangle(
                position.X - 1,
                position.Y - 1,
                position.Width + 2,
                position.Height + 2);

            isOpen = false;

            //door is locked until all keys are collected
            unlocked = false;
        }

        /// <summary>
        /// a method that updates the texture of the door depending
        /// on how many keys the player is holding at the moment
        /// </summary>
        /// <param name="gametime"></param>
        /// <param name="player"></param>
        public void Update(GameTime gametime, Player player)
        {
            KeyboardState currentState = Keyboard.GetState();

            //these if statements changes the sprite
            //of the final door depending on how many 
            //keys are collected so far
            if (player.CollectedKeys[0] == true)
            {
                sourceRect.X = 16;
            }

            if (player.CollectedKeys[1] == true)
            {
                sourceRect.X = 32;
                unlocked = true;
            }

            if (player.CollectedKeys[2] == true)
            {
                sourceRect.X = 48;
            }

            if (player.CollectedKeys[3] == true)
            {
                sourceRect.X = 64;
                unlocked = true;
            }

            //if the player presses e on the door and the 
            //door is unlocked it will open and teleport the user to
            //the final room(the hub is a placeholder)
            if (previousState.IsKeyUp(Keys.E) && currentState.IsKeyDown(Keys.E)
                && unlocked && hitbox.Intersects(player.Position))
            {
                sourceRect.X = 80;
                LevelManager.Instance.LoadLevel(GameLevels.End);
                player.ResetKeys();
                sourceRect.X = 0;
            }

            previousState = currentState;
        }
    }
}
