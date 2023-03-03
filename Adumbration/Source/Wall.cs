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
    /// a wall class that is here to stop the player from going out of bounds
    /// </summary>
    public class Wall : GameObject
    {
        /// <summary>
        /// constructor for this class
        /// </summary>
        /// <param name="spriteSheet"></param>
        /// <param name="sourceRect"></param>
        /// <param name="position"></param>
        public Wall(Texture2D spriteSheet, Rectangle sourceRect, Rectangle position)
            : base(spriteSheet, sourceRect, position)
        {

        }


        public override void Update(GameTime gameTime)
        {
            //there is nothing to update inside a wall brother
        }


        /// <summary>
        /// this is to get a bool value inside game1
        /// to stop the player from passing this point
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool IsColliding(GameObject obj)
        {
            return Position.Intersects(obj.Position);
        }
    }
}
