using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XNAGame.BeingTemplates;

namespace XNAGame.Beings
{
    /// <summary>
    /// This is a laser beam that flies across the screen unaffected by gravity.
    /// Also relatively blinding, so don't look at it.
    /// </summary>
    public class w_laser : Weapon
    {
        /// <summary>
        /// Creates a new laser. (Parameterless)
        /// </summary>
        public w_laser()
            : base(0, 0, "laser", 0, 0, 500, false)
        {

        }

        /// <summary>
        /// Creates a new laser beam.
        /// </summary>
        /// <param name="x">The x coordinate of the laser.</param>
        /// <param name="y">The y coordinate of the laser.</param>
        /// <param name="mouseX">The x coordinate of the mouse.</param>
        /// <param name="mouseY">The y coordinate of the mouse.</param>
        public w_laser(int x, int y, int mouseX, int mouseY)
            : base(x, y, "laser", mouseX, mouseY, 500, false)
        {

        }

        public override void fire(int x, int y, int mouseX, int mouseY)
        {
            Main.addBeing(new w_laser(x, y, mouseX, mouseY));
        }
    }
}
