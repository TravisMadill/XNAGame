using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XNAGame.Beings;

namespace XNAGame.BeingTemplates
{
    /// <summary>
    /// This class embodies everything that damages enemies in the game.
    /// Everything except the kitchen sink.
    /// </summary>
    public abstract class Weapon : Being
    {
        /// <summary>
        /// The power behind the firing of the weapon.
        /// </summary>
        protected int firePower;

        /// <summary>
        /// Whether or not this weapon will be affected by gravity.
        /// </summary>
        protected bool isAffectedByGravity;

        /// <summary>
        /// The rotation of this sprite, in radians.
        /// </summary>
        float rotation;

        /// <summary>
        /// Creates a new weapon type.
        /// </summary>
        /// <param name="x">The x coordinate of this projectile.</param>
        /// <param name="y">The y coordinate of this projectile.</param>
        /// <param name="spriteName">The name of the sprite in the sprites folder.</param>
        /// <param name="mouseX">The x coordinate of the mouse.</param>
        /// <param name="mouseY">The y coordinate of the mouse.</param>
        /// <param name="firePower">The firepower of the projectile.</param>
        /// <param name="isAffectedByGravity">Whether or not this projectile is affected by gravity.</param>
        public Weapon(int x, int y, string spriteName, int mouseX, int mouseY, int firePower, bool isAffectedByGravity)
            : base(x, y, spriteName)
        {
            this.firePower = firePower;
            this.isAffectedByGravity = isAffectedByGravity;

            //Doing some trig to find out the rotation
            this.rotation = (float)(-Math.Atan2(mouseX - Being.scrollOffset.X - x - Main.TileSize / 2, mouseY - Being.scrollOffset.Y - y - Main.TileSize / 2) + Math.PI);
            Movement = new Vector2((float)Math.Sin(rotation) * firePower, (float)-Math.Cos(rotation) * firePower);
        }

        /// <summary>
        /// What this weapon should do when being fired by the player.
        /// </summary>
        /// <param name="x">The x coordinate of the player.</param>
        /// <param name="y">The y coordinate of the player.</param>
        /// <param name="mouseX">The x coordinate of the mouse.</param>
        /// <param name="mouseY">The y coordinate of the mouse.</param>
        public abstract void fire(int x, int y, int mouseX, int mouseY);


        public override void updateLogic(double delta)
        {
            //Affect with gravity if necessary
            if (isAffectedByGravity)
                Movement += GRAVITY;

            //Collision; If the weapon hits a wall, get rid of it.
            List<Being> collisions = Collision.getPossibleCollisions(this);
            if (collisions.Count > 0)
            {
                foreach (Being b in collisions)
                {
                    if (b is Wall)
                    {
                        Main.removeBeing(this);
                    }
                }
            }

            //If the projectile flies offscreen, get rid of it, so as not to keep updating this being unneccesarily, especially if this falls through a pit, where there are no walls to hit.
            if (!isOnScreen(this.position, this.width, this.height))
            {
                Main.removeBeing(this);
            }
        }

        public override void draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sb)
        {
            //Basically, draw the sprite with the rotation of this being.
            if (isOnScreen(Position, width, height))
            {
                sb.Draw(sprite, HitBox, null, Color.White, this.rotation, Vector2.Zero, SpriteEffects.None, 0);
            }
        }
    }
}
