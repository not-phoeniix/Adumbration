using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Adumbration.Source.Level
{
    internal class LevelDoor : Door, ISignal
    {
        private bool isOpen;
        public int SignalNum { get; set; }

        public bool IsOpen
        {
            get { return isOpen; }
            set { isOpen = value; }
        }

        /// <summary>
        /// Creates a new LevelDoor object, should only be used in level design, not connecting levels
        /// </summary>
        /// <param name="texture">Door spritesheet</param>
        /// <param name="position">Position to draw</param>
        /// <param name="signal">Integer signal associated with it</param>
        public LevelDoor(Texture2D texture, Rectangle position, int signal)
            // hub level is irrelevant, just there to fill base
            : base(texture, position, GameLevels.Hub)   
        {
            isOpen = false;
            SignalNum = signal;
        }

        /// <summary>
        /// Draws this LevelDoor depending on its open state
        /// </summary>
        /// <param name="sb"></param>
        public override void Draw(SpriteBatch sb)
        {
            if(isOpen)
            {
                Hull.Enabled = false;
                sb.Draw(spriteSheet, positionRect, new Rectangle(1 * 16, 3 * 16, 16, 16), Color.White);
            }
            else
            {
                Hull.Enabled = true;
                sb.Draw(spriteSheet, positionRect, new Rectangle(0, 3 * 16, 16, 16), Color.White);
            }
        }
    }
}
