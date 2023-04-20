﻿using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

// Describes the two types of mirrors we have
public enum MirrorType
{
    Forward,
    Backward
}

namespace Adumbration
{
    internal class Mirror : GameObject, IHitbox
    {
        // Fields
        private MirrorType type;
        private Rectangle hitbox;

        // Constructor
        /// <summary>
        /// Parameterized constructor for the mirror class.
        /// </summary>
        /// <param name="textureDict">The dictionary to get textures from.</param>
        /// <param name="position">The game position.</param>
        /// <param name="type">The type of mirror.</param>
        public Mirror(Texture2D mirrorTexture, Rectangle position, MirrorType type)
             : base(
                   mirrorTexture,
                   new Rectangle(
                       0,
                       0,
                       mirrorTexture.Width,
                       mirrorTexture.Height
                       ),
                   position)
        {
            this.type = type;

            // Create hitbox rectangle
            hitbox = new Rectangle(
                position.X - 2,
                position.Y - 2,
                position.Width + 3,
                position.Height + 3);
        }

        /// <summary>
        /// The Orientation type of mirror it is
        /// </summary>
        public MirrorType Type
        {
            get { return type; }
        }

        /// <summary>
        /// Hit box where player can interact
        /// </summary>
        public Rectangle Hitbox
        {
            get { return hitbox; }
            set { hitbox = value; }
        }

        /// <summary>
        /// Updates position dependent on Player's input
        /// </summary>
        /// <param name="myPlayer">The Player interacting with the mirror</param>
        /// <param name="currentLevel">The level the player and mirror are in</param>
        /// <param name="gameTime">Time passed in game</param>
        public virtual void Update(Player myPlayer, Level currentLevel, GameTime gameTime)
        {
            KeyboardState currentKbState = Keyboard.GetState();

            // If the player is within the interaction hit box
            // And is holding space while moving
            if (hitbox.Intersects(myPlayer.Position) && currentKbState.IsKeyDown(Keys.Space) 
                && currentKbState.IsKeyDown(Keys.W))
            {
                // Change both postitions of mirror and it's hit box
                positionRect.Y -= myPlayer.Speed;
                hitbox.Y -= myPlayer.Speed;

                // While moving in the North direction
                foreach (GameObject tile in currentLevel.TileList)
                {
                    // If it is colliding with a wall
                    if (tile is Wall && IsColliding(tile))
                    {
                        // Snap both positions of mirror and hitbox
                        positionRect.Y = tile.Position.Height + tile.Position.Y;
                        hitbox.Y = tile.Position.Height + tile.Position.Y;
                    }
                }
            }

            if (hitbox.Intersects(myPlayer.Position) && currentKbState.IsKeyDown(Keys.Space)
                && currentKbState.IsKeyDown(Keys.A))
            {
                positionRect.X -= myPlayer.Speed;
                hitbox.X -= myPlayer.Speed;

                // While moving in the North direction
                foreach (GameObject tile in currentLevel.TileList)
                {
                    // If it is colliding with a wall
                    if (tile is Wall && IsColliding(tile))
                    {
                        positionRect.X = tile.Position.Width + tile.Position.X;
                        hitbox.X = tile.Position.Width + tile.Position.X;
                    }
                }
            }

            if (hitbox.Intersects(myPlayer.Position) && currentKbState.IsKeyDown(Keys.Space)
                && currentKbState.IsKeyDown(Keys.S))
            {
                positionRect.Y += myPlayer.Speed;
                hitbox.Y += myPlayer.Speed;

                // While moving in the North direction
                foreach (GameObject tile in currentLevel.TileList)
                {
                    // If it is colliding with a wall
                    if (tile is Wall && IsColliding(tile))
                    {
                        positionRect.Y = tile.Position.Y - positionRect.Height;
                        hitbox.Y = tile.Position.Y - positionRect.Height;
                    }
                }
            }

            if (hitbox.Intersects(myPlayer.Position) && currentKbState.IsKeyDown(Keys.Space)
                && currentKbState.IsKeyDown(Keys.D))
            {
                positionRect.X += myPlayer.Speed;
                hitbox.X += myPlayer.Speed;

                // While moving in the North direction
                foreach (GameObject tile in currentLevel.TileList)
                {
                    // If it is colliding with a wall
                    if (tile is Wall && IsColliding(tile))
                    {
                        positionRect.X = tile.Position.X - positionRect.Width;
                    }
                }
            }
        }

        /// <summary>
        /// Checks for a collision between an object and the mirror.
        /// If the object is a LightBeam,
        /// check if the mirror's position intersects the object's.
        /// Otherwise, check if the mirror's hitbox intersects the object's position.
        /// </summary>
        /// <param name="obj">Reference to a game object</param>
        /// <returns>True if the collision occurs, otherwise false.</returns>
        public override bool IsColliding(GameObject obj)
        {
            if (obj is not LightBeam)
            {
                return hitbox.Intersects(obj.Position);
            }

            return false;
        }

        /// <summary>
        /// Draw method that uses the base class's method.
        /// </summary>
        /// <param name="sb">SpriteBatch object to draw with.</param>
        public override void Draw(SpriteBatch sb)
        {
            // shorthand if/else that flips mirror sprite horizontally depending on type
            SpriteEffects fx = (type == MirrorType.Forward) ?
                SpriteEffects.FlipHorizontally :
                SpriteEffects.None;

            sb.Draw(spriteSheet, positionRect, null, Color.White, 0, Vector2.Zero, fx, 0);
        }
  
    }
}
