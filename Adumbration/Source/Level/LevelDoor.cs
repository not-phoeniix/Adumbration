using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Adumbration.Source.Level
{
    internal class LevelDoor : Door, ISignal
    {
        /// <summary>
        /// The number that corresponds to a signal
        /// </summary>
        public int SignalNum { get; set; }

        /// <summary>
        /// Determine whether the level door is open
        /// </summary>
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
        public LevelDoor(Texture2D texture, Rectangle position, Direction dir, int signal)
            // hub level is irrelevant, just there to fill base
            : base(texture, null, position, GameLevels.Hub, dir)
        {
            SignalNum = signal;
            sourceRect.Y = 3 * 16;
        }

        /// <summary>
        /// Draws the correct Level Door texture
        /// </summary>
        /// <param name="sb"></param>
        public override void Draw(SpriteBatch sb)
        {
            if(isOpen)
            {
                Hull.Enabled = false;
                sourceRect.Y = 4 * 16;
            }
            else
            {
                Hull.Enabled = true;
                sourceRect.Y = 3 * 16;
            }

            base.Draw(sb);
        }

        /// <summary>
        /// Updating LevelDoors does nothing, meaning they won't load levels
        /// </summary>
        /// <param name="myPlayer"></param>
        public override void Update(Player myPlayer) {}
    }
}
