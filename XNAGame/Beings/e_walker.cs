using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XNAGame.Beings;
using XNAGame.BeingTemplates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XNAGame.Beings
{
    /// <summary>
    /// An emeny that walks back and forth in a straight line.
    /// Yes, I am aware that it falls through floors occasionally, but when I first saw it,
    /// the texture looked like it was making a sad face, as though it didn't like falling through, and
    /// I wanted to keep that because it was funny to me.
    /// </summary>
    class e_walker : Enemy
    {
        /// <summary>
        /// Checks if this walker is moving left.
        /// </summary>
        bool isMovingLeft = true;

        /// <summary>
        /// Creates a new walker enemy.
        /// </summary>
        /// <param name="x">The starting x coordinate of this walker.</param>
        /// <param name="y">The starting y coordinate of this walker.</param>
        public e_walker(int x, int y)
            : base(x, y, "enemy_walker")
        {
            //This is the default walker's moving speed.
            Movement = new Vector2(-100, 0);
        }

        public override void updateLogic(double delta)
        {
            List<Being> colliding = Collision.getPossibleCollisions(this);
            if (commentRNG.Next(10000) < 10)
            {
                Main.addBeing(new eff_fly((int)Position.X, (int)Position.Y, Text.getMsg("general", "enemyComment_walker")));
            }
            foreach (Being b in colliding)
            {
                if (b is Weapon)
                {
                    Main.addBeing(new eff_fly((int)Position.X, (int)Position.Y, Text.getMsg("general", "eDefeated_walker")));
                    Main.removeBeing(this);
                    Main.removeBeing(b);
                }
                if (b is Wall)
                {
                    //Reverse X direction
                    Movement *= -Vector2.UnitX;
                    isMovingLeft = !isMovingLeft;
                }
            }
            try
            {
                //If it moves off the floor, affect it with gravity.
                if (Main.level.levelMap[(int)position.X / 40, (int)(position.Y / 40) + 1] != Level.WALL ||
                    Main.level.levelMap[(int)(position.X / 40) + 1, (int)(position.Y / 40) + 1] != Level.WALL)
                {
                    Movement += GRAVITY;
                }
                else
                {
                    //Otherwise, get rid of the y movement of gravity.
                    Movement *= Vector2.UnitX; //By the way, UnitX represents the vector (1, 0), so the y will go away and the x will stay the same.
                }
            }
            catch (IndexOutOfRangeException)
            {
                //Then it would have fallen off the map into a pit, so just remove this
                Main.removeBeing(this);
            }
        }

        public override void move(float delta)
        {
            //Don't actually do anything until on screen. We wouldn't want it to fall into a pit if it's on the other side of the level.
            if(isOnScreen(Position, this.width, this.height))
                base.move(delta);
        }

        public override void draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sb)
        {
            //Determines whether or not to reflect the sprite depending on the direction.
            if (isMovingLeft)
                base.draw(sb);
            else
                sb.Draw(this.sprite, this.HitBox, null, Color.White, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
        }
    }
}
