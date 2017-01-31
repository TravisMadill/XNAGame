using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XNAGame.BeingTemplates;

namespace XNAGame.Beings
{
    /// <summary>
    /// A rocket that flies and falls back into the ground.
    /// These aren't very good rockets... They're supposed to go to space...
    /// </summary>
    public class w_rocket : Weapon
    {
        /// <summary>
        /// Creates a new rocket (Parameterless)
        /// </summary>
        public w_rocket()
            : base(0, 0, "bullet", 0, 0, 750, true)
        {

        }

        /// <summary>
        /// Creates a new rocket to fire.
        /// </summary>
        /// <param name="x">The x coordinate of the rocket.</param>
        /// <param name="y">The y coordinate of the rocket.</param>
        /// <param name="mouseX">The x coordinate of the mouse.</param>
        /// <param name="mouseY">The y coordinate of the mouse.</param>
        public w_rocket(int x, int y, int mouseX, int mouseY)
            : base(x, y, "bullet", mouseX, mouseY, 750, true)
        {
            
        }

        public override void fire(int x, int y, int mouseX, int mouseY)
        {
            Main.addBeing(new w_rocket(x, y, mouseX, mouseY));
        }
    }
}
