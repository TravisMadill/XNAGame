using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XNAGame.BeingTemplates;

namespace XNAGame.Beings
{
    /// <summary>
    /// This item gives the player a laser gun.
    /// The laser gun is unaffected by gravity, and can allow for more precise shots, but
    /// if the player already has one, this being will remove itself from the map.
    /// </summary>
    class i_laser : Item
    {
        /// <summary>
        /// Creates a new instance of the laser gun pickup.
        /// </summary>
        /// <param name="x">The x coordinate of the laser gun.</param>
        /// <param name="y">The y coordinate of the laser gun.</param>
        public i_laser(int x, int y)
            : base(x, y, "laserGun", 0, 1)
        {
            //If the player already has a laser, then there's no need for another one.
            foreach (Weapon w in Main.curSaveData.weapons)
            {
                if (w is w_laser)
                {
                    Main.removeBeing(this);
                }
            }
        }

        public override void takeEffect()
        {
            //Add a laser to the weapons list that you can fire
            Main.addBeing(new eff_fly((int)position.X, (int)position.Y, Text.getMsg("general", "playerComment_getLaser")));
            Main.curSaveData.weapons.Add(new w_laser(0, 0, 0, 0));
        }

        public override void draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sb)
        {
            if(!Main.curSaveData.weapons.Contains(new w_laser(0, 0, 0, 0)))
                base.draw(sb);
            else
                Main.removeBeing(this);
        }
    }
}
