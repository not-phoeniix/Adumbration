using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Adumbration
{
    internal class PauseMenu
    {
        #region SingletonStuff

        // static private instance of itself
        private static PauseMenu instance = null;

        // PRIVATE constructor
        private PauseMenu() { }

        /// <summary>
        /// LevelManager instance, singleton which allows 
        /// managing of all levels in the game. READ-ONLY
        /// </summary>
        public static PauseMenu Instance
        {
            get
            {
                // if the instance wasn't created yet, create it
                if(instance == null)
                {
                    instance = new PauseMenu();
                }

                return instance;
            }
        }

        #endregion

        // menu enum
        private enum MenuButtons
        {
            Resume,
            Quit
        }
        
        // fields
        private MenuButtons selectedButton;
        private Dictionary<string, Texture2D> textureDict;

        /// <summary>
        /// Initializes the pause menu, must be run before using the menu
        /// </summary>
        /// <param name="textureDict">Texture dictionary</param>
        public void Initialize(Dictionary<string, Texture2D> textureDict)
        {
            this.textureDict = textureDict;
        }

        /// <summary>
        /// Updates the status of the pause menu w/ selected options
        /// </summary>
        /// <param name="kbState">Current keyboard state</param>
        /// <param name="kbStatePrev">Previous keyboard state</param>
        public void Update(KeyboardState kbState, KeyboardState kbStatePrev)
        {
            // FSM for currently selected menu items and moving between menu options
            switch(selectedButton)
            {
                case MenuButtons.Resume:
                    // transition to quit state
                    if(Game1.IsKeyPressedOnce(Keys.Right, kbState, kbStatePrev))
                    {
                        selectedButton = MenuButtons.Quit;
                    }

                    if(Game1.IsKeyPressedOnce(Keys.Enter, kbState, kbStatePrev))
                    {
                        Game1.GameState = GameState.Game;
                    }
                    break;

                case MenuButtons.Quit:
                    // transition to resume state
                    if(Game1.IsKeyPressedOnce(Keys.Left, kbState, kbStatePrev))
                    {
                        selectedButton = MenuButtons.Resume;
                    }

                    if(Game1.IsKeyPressedOnce(Keys.Enter, kbState, kbStatePrev))
                    {
                        Game1.GameState = GameState.MainMenu;
                    }

                    break;
            }
        }

        /// <summary>
        /// Draws the pause menu
        /// </summary>
        /// <param name="sb"></param>
        public void Draw(SpriteBatch sb, Rectangle screenRect)
        {
            // FSM for drawing the pause menu options
            switch(selectedButton)
            {
                // draw button "Resume" hovered
                case MenuButtons.Resume:
                    sb.Draw(
                        textureDict["pauseResume"],
                        screenRect,
                        Color.White);

                    break;

                // draw button "Quit" hovered
                case MenuButtons.Quit:
                    sb.Draw(
                        textureDict["pauseQuit"],
                        screenRect,
                        Color.White);

                    break;
            }
        }
    }
}
