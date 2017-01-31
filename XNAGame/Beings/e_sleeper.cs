using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using XNAGame.Beings;
using XNAGame.BeingTemplates;

namespace XNAGame.Beings
{
    /// <summary>
    /// An enemy that lulls about doing nothing, sleeping the days away...
    /// But can still kill you if you run into it.
    /// </summary>
    class e_sleeper : Enemy
    {
        public e_sleeper(int x, int y)
            : base(x, y, "enemy_sleeper")
        {

        }

        public override void updateLogic(double delta)
        {
            //Doesn't move... Just occasionally snores.
            List<Being> colliding = Collision.getPossibleCollisions(this);
            if (commentRNG.Next(10000) < 10)
            {
                Main.addBeing(new eff_fly((int)Position.X, (int)Position.Y, Text.getMsg("general", "enemyComment_sleeper")));
            }
            //...and reacts to weapons touching it.
            foreach (Being b in colliding)
            {
                if (b is Weapon)
                {
                    Main.addBeing(new eff_fly((int)Position.X, (int)Position.Y, Text.getMsg("general", "eDefeated_sleeper")));
                    Main.removeBeing(this);
                    Main.removeBeing(b);
                }
            }
        }
    }
}
