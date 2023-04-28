using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;

namespace Adumbration
{
    internal class HelpMenu
    {
        #region SingletonStuff

        // static private instance of itself
        private static HelpMenu instance = null;

        // PRIVATE constructor
        private HelpMenu() { }

        /// <summary>
        /// LevelManager instance, singleton which allows 
        /// managing of all levels in the game. READ-ONLY
        /// </summary>
        public static HelpMenu Instance
        {
            get
            {
                // if the instance wasn't created yet, create it
                if(instance == null)
                {
                    instance = new HelpMenu();
                }

                return instance;
            }
        }

        #endregion

        // menu enum
        private enum MenuButtons
        {
            Start,
            Help,
            Stats,
            Quit
        }

        // fields
        private MenuButtons selectedButton;
        private Dictionary<string, Texture2D> textureDict;
        public ExitGameDelegate Exit;
        private SoundEffectInstance backSound;

        /// <summary>
        /// Initializes the pause menu, must be run before using the menu
        /// </summary>
        /// <param name="textureDict">Texture dictionary</param>
        public void Initialize(Dictionary<string, Texture2D> textureDict, Dictionary<string, SoundEffect> soundDict)
        {
            this.textureDict = textureDict;
            backSound = soundDict["menuBack"].CreateInstance();
            backSound.Volume = 0.6f;
        }

        /// <summary>
        /// Updates the status of the help menu w/ selected options
        /// </summary>
        /// <param name="kbState">Current keyboard state</param>
        /// <param name="kbStatePrev">Previous keyboard state</param>
        public void Update(KeyboardState kbState, KeyboardState kbStatePrev)
        {
            if(Game1.IsKeyPressedOnce(Keys.Escape, kbState, kbStatePrev))
            {
                backSound.Play();

                // goes to previous state of menu (which is set in
                //   the menu transitions in each respective class
                //   before transitioning to this help menu), either
                //   main or pause menu
                Game1.GameState = Game1.PrevState;
            }
        }

        /// <summary>
        /// Draws the help menu
        /// </summary>
        /// <param name="sb">SpriteBatch to draw with</param>
        public void Draw(SpriteBatch sb, Rectangle screenRect)
        {
            sb.Draw(
                textureDict["help"],
                screenRect,
                Color.White);
        }
    }
}
