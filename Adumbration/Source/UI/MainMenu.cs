using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Adumbration
{
    public delegate void ExitGameDelegate();

    internal class MainMenu
    {
        #region SingletonStuff

        // static private instance of itself
        private static MainMenu instance = null;

        // PRIVATE constructor
        private MainMenu() { }

        /// <summary>
        /// LevelManager instance, singleton which allows 
        /// managing of all levels in the game. READ-ONLY
        /// </summary>
        public static MainMenu Instance
        {
            get
            {
                // if the instance wasn't created yet, create it
                if(instance == null)
                {
                    instance = new MainMenu();
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
                // "START" HOVERED
                case MenuButtons.Start:
                    if(Game1.IsKeyPressedOnce(Keys.Down, kbState, kbStatePrev))
                    {
                        selectedButton = MenuButtons.Help;
                    }

                    if(Game1.IsKeyPressedOnce(Keys.Enter, kbState, kbStatePrev))
                    {
                        Game1.GameState = GameState.Game;
                    }

                    break;

                // "HELP" HOVERED
                case MenuButtons.Help:
                    if(Game1.IsKeyPressedOnce(Keys.Down, kbState, kbStatePrev))
                    {
                        selectedButton = MenuButtons.Stats;
                    }

                    if(Game1.IsKeyPressedOnce(Keys.Up, kbState, kbStatePrev))
                    {
                        selectedButton = MenuButtons.Start;
                    }

                    if(Game1.IsKeyPressedOnce(Keys.Enter, kbState, kbStatePrev))
                    {
                        Game1.GameState = GameState.Help;
                    }

                    break;

                // "STATS" HOVERED
                case MenuButtons.Stats:
                    if(Game1.IsKeyPressedOnce(Keys.Down, kbState, kbStatePrev))
                    {
                        selectedButton = MenuButtons.Quit;
                    }

                    if(Game1.IsKeyPressedOnce(Keys.Up, kbState, kbStatePrev))
                    {
                        selectedButton = MenuButtons.Help;
                    }

                    if(Game1.IsKeyPressedOnce(Keys.Enter, kbState, kbStatePrev))
                    {
                        Game1.GameState = GameState.Stats;
                    }

                    break;

                // "QUIT" HOVERED
                case MenuButtons.Quit:
                    if(Game1.IsKeyPressedOnce(Keys.Up, kbState, kbStatePrev))
                    {
                        selectedButton = MenuButtons.Stats;
                    }

                    if(Game1.IsKeyPressedOnce(Keys.Enter, kbState, kbStatePrev))
                    {
                        Exit();
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
                case MenuButtons.Start:
                    sb.Draw(
                        textureDict["mainStart"],
                        screenRect,
                        Color.White);

                    break;

                case MenuButtons.Help:
                    sb.Draw(
                        textureDict["mainHelp"],
                        screenRect,
                        Color.White);

                    break;

                case MenuButtons.Stats:
                    sb.Draw(
                        textureDict["mainStats"],
                        screenRect,
                        Color.White);

                    break;

                case MenuButtons.Quit:
                    sb.Draw(
                        textureDict["mainQuit"],
                        screenRect,
                        Color.White);

                    break;
            }
        }
    }
}
